using System;
using System.Collections.Generic;
using SpaceChaser.Core.Building;
using SpaceChaser.Core.HUD;
using SpaceChaser.Core.Inputs;
using SpaceChaser.Core.Inventory;
using SpaceChaser.Core.Registry;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SpaceChaser.Core.Player
{

    [RequireComponent(typeof(PlayerController))]
    public class PlayerBuildTool : MonoBehaviour
    {
        private GameConfig _config;
        private IBuildService _buildService;
        private IInputReader _inputs;
        private IInventoryService _inventory;
        private IAssetRegistry<BuildData> _buildRegistry;
        private IAssetRegistry<StrutData> _strutRegistry;
        private IAssetRegistry<FoundationData> _foundationRegistry;
        private BuildPreview _preview;
        private InventoryView _inventoryView;

        private readonly List<BuildData> _builds = new(10);
        private readonly List<StrutData> _struts = new(10);
        private readonly List<FoundationData> _foundations = new(10);
        private int _index;
        private BuildMode _mode = BuildMode.Cursor;
        private float _currentRotation = 0f;
        private float _buildTimer = 0f;
        private bool _primaryHeld = false;
        private bool _secondaryHeld = false;

        private ContactFilter2D _previewContactFilter = new();
        private IReadOnlyList<Build> _cashedBuilds;
        private IReadOnlyList<Foundation> _cashedFoundations;
        private readonly List<Collider2D> _overlapResults = new();
        [SerializeField] private LayerMask _previewLayer;

        private event Action _finishSalvage;
        private Collider2D _currentSalvagable;

        [Inject]
        public void Construct(
            IBuildService buildService,
            IInputReader inputs,
            BuildPreview preview,
            IInventoryService inventoryService,
            GameConfig config,
            IAssetRegistry<BuildData> buildRegistry,
            IAssetRegistry<StrutData> strutRegistry,
            IAssetRegistry<FoundationData> foundationRegistry,
            InventoryView inventoryView
            )
        {
            _buildService = buildService;
            _inputs = inputs;
            _preview = preview;
            _inventory = inventoryService;
            _config = config;
            _buildRegistry = buildRegistry;
            _strutRegistry = strutRegistry;
            _foundationRegistry = foundationRegistry;
            _inventoryView = inventoryView;

            _previewContactFilter.useTriggers = false;
            _previewContactFilter.useLayerMask = true;
            _previewContactFilter.layerMask = _previewLayer;
        }
        private void Start()
        {
            InitializeStruts();
            InitializeFoundations();
            RandomizeBuilds();


            //UnselectBuild();
            SelectBuildAt(0);
        }
        private void FixedUpdate()
        {
            if (_buildTimer > 0f && (_preview.transform.position - transform.position).sqrMagnitude > _config.BuildingDistance * _config.BuildingDistance)
            {
                EndBuildMode();
            }
            if (_primaryHeld && Physics2D.OverlapPoint(_inputs.GetWorldAimPosition(), _previewContactFilter, _overlapResults) > 0)
            {
                _buildTimer += Time.fixedDeltaTime;

                if (_buildTimer >= _config.BuildingTime)
                {
                    FinishBuilding();
                }
            }
            if (_secondaryHeld && _currentSalvagable.OverlapPoint(_inputs.GetWorldAimPosition()))
            {
                _buildTimer += Time.fixedDeltaTime;

                if (_buildTimer >= _config.BuildingTime)
                {
                    FinishSalvaging();
                }
            }
        }
        public void OnEnable()
        {
            _inputs.OnPrimaryActionHeld += HandlePrimaryAction;
            _inputs.OnSecondaryActionHeld += HandleSecondaryAction;
            _inventoryView.OnSelected += SelectAt;
        }
        public void OnDisable()
        {
            _inputs.OnPrimaryActionHeld -= HandlePrimaryAction;
            _inputs.OnSecondaryActionHeld -= HandleSecondaryAction;
            _inventoryView.OnSelected -= SelectAt;
        }

        private void HandlePrimaryAction(bool held)
        {
            if (_index < 0) return;
            if (held && _buildTimer == 0f)
            {
                if (!CheckAny()) return;
                _preview.StartBuilding();
                _buildTimer = 0.00001f;

            }
            _primaryHeld = held;
        }
        private void HandleSecondaryAction(bool held)
        {
            _secondaryHeld = false;
            if (held) UnselectBuild();
            if (held && _buildTimer == 0f)
            {
                var cols = _preview.GetOnCursor();
                foreach (var col in cols)
                {
                    if (col.TryGetComponent(out Strut strut))
                    {
                        _finishSalvage = () =>
                        {
                            _buildService.RemoveStrut(strut);
                            _inventory.Add(new List<ItemAmount>(strut.Data.Recipe), _config.SalvageRate);
                        };
                    }
                    else if (col.TryGetComponent(out Build build))
                    {
                        _finishSalvage = () =>
                        {
                            _buildService.RemoveBuild(build);
                            _inventory.Add(new List<ItemAmount>(build.Data.Recipe), _config.SalvageRate);
                        };
                    }
                    else if (col.TryGetComponent(out Foundation foundation))
                    {
                        if (!CheckRemoveFoundation(foundation)) continue;
                        _finishSalvage = () =>
                        {
                            _buildService.RemoveFoundation(foundation);
                            _inventory.Add(new List<ItemAmount>(foundation.Data.Recipe), _config.SalvageRate);
                        };

                    }
                    else if (col.TryGetComponent(out Resource resource))
                    {
                        _finishSalvage = () =>
                        {
                            _buildService.RemoveResource(resource);
                            _inventory.Add(new List<ItemAmount>(resource.Data.Resources));
                        };
                    }
                    else continue;

                    _currentSalvagable = col;
                    _secondaryHeld = true;

                    break;
                }
            }
        }
        private bool CheckAny()
        {
            if ((_preview.transform.position - transform.position).sqrMagnitude > _config.BuildingDistance * _config.BuildingDistance) return false;
            return _mode switch
            {
                BuildMode.Cursor => false,
                BuildMode.Build => CheckBuild(),
                BuildMode.Strut => CheckStrut(),
                BuildMode.Foundation => CheckFoundation(),
                _ => false,
            };
        }
        private bool CheckBuild()
        {
            if (!_inventory.Has(_builds[_index].Recipe)) return false;
            IReadOnlyList<Build> buildContacts = _preview.GetAllBuildContacts();
            if (buildContacts.Count != 0) return false;

            IReadOnlyList<Foundation> foundations = _preview.GetAllFoundationContacts();
            if (foundations.Count != 0) return false;

            return true;
        }
        private bool CheckStrut()
        {
            if (!_inventory.Has(_struts[_index].Recipe)) return false;
            _cashedBuilds = _preview.GetAllBuildContacts();
            if (_cashedBuilds.Count <= 1) return false;

            IReadOnlyList<Strut> struts = _preview.GetAllStrutContacts();
            if (struts.Count != 0) return false;

            return true;
        }
        private bool CheckFoundation()
        {
            if (!_inventory.Has(_foundations[_index].Recipe)) return false;
            _cashedFoundations = _preview.GetAllFoundationContacts();
            if (_cashedFoundations.Count == 0) return false;

            IReadOnlyList<Build> builds = _preview.GetAllBuildContacts();
            if (builds.Count != 0) return false;

            return true;
        }
        private bool CheckRemoveFoundation(Foundation foundation)
        {
            if (!foundation.Salvagable) return false;

            foreach (Foundation contact in foundation.GetAllContacts())
            {
                if (contact.GetAllContacts().Count <= 1 && !contact.Static) return false;
            }

            return true;
        }



        private void FinishSalvaging()
        {
            _finishSalvage?.Invoke();
            EndBuildMode();
        }

        private void FinishBuilding()
        {
            EndBuildMode();
            if (_index < 0) return;

            List<ItemAmount> recipe = null;

            switch (_mode)
            {
                case BuildMode.Cursor:
                    return;
                case BuildMode.Build:
                    var build = _builds[_index];
                    recipe = build.Recipe;
                    _buildService.Build(build, _preview.transform.position, _currentRotation);

                    break;
                case BuildMode.Strut:
                    var strut = _struts[_index];
                    recipe = strut.Recipe;
                    _buildService.BuildStrut(strut, _cashedBuilds, _preview.transform.position, _currentRotation);

                    break;
                case BuildMode.Foundation:
                    var foundation = _foundations[_index];
                    recipe = foundation.Recipe;
                    _buildService.BuildFoundation(foundation, _cashedFoundations, _preview.transform.position, 0f);

                    ////////// FIXED ROTATION -------------------------/// -------------    HERE

                    break;
            }
            _inventory.Remove(recipe);
            RandomizeBuilds();
        }
        private void SelectAt(int index)
        {
            if (index < 0) UnselectBuild();
            else if (index < _config.BuildingSlots) SelectBuildAt(index);
            else if (index < _config.BuildingSlots + _config.StrutSlots) SelectStrutAt(index - _config.BuildingSlots);
            else if (index < _config.BuildingSlots + _config.StrutSlots + _config.FoundationSlots) SelectFoundationAt(index - _config.BuildingSlots - _config.StrutSlots);
            else Debug.LogWarning($"Trying to select a building at too large index: {index} (from view)");

            Debug.Log($"Selecting a building at index: {index} (from view)");
        }
        private void SelectBuildAt(int index)
        {
            if (_builds == null || _builds.Count <= index) return;

            _index = index;
            _mode = BuildMode.Build;
            var build = _builds[index];
            _preview.SetTarget(build.Prefab.gameObject);

            EndBuildMode();
        }
        private void SelectStrutAt(int index)
        {
            if (_struts == null || _struts.Count <= index) return;

            _index = index;
            _mode = BuildMode.Strut;
            var strut = _struts[index];
            _preview.SetTarget(strut.Prefab.gameObject);

            EndBuildMode();
        }
        private void SelectFoundationAt(int index)
        {
            if (_foundations == null || _foundations.Count <= index) return;

            _index = index;
            _mode = BuildMode.Foundation;
            var foundation = _foundations[index];
            _preview.SetTarget(foundation.Prefab.gameObject);

            EndBuildMode();
        }

        private void EndBuildMode()
        {
            _buildTimer = 0f;
            _primaryHeld = false;
            _secondaryHeld = false;

            _preview.ExitBuilding();
        }
        private void UnselectBuild()
        {
            _index = -1;
            _preview.RemoveTarget();
            _mode = BuildMode.Cursor;

            _buildTimer = 0f;
            _primaryHeld = false;

            _preview.ExitBuilding();
        }

        private void RandomizeBuilds()
        {
            _builds.Clear();

            int assetCount = _buildRegistry.Assets.Count;

            for (int i = 0; i < _config.BuildingSlots; i++)
            {
                int randI = UnityEngine.Random.Range(0, assetCount);
                BuildData data = _buildRegistry.Assets[randI];
                _builds.Add(data);
                _inventoryView.SetOnIndex(i, data.Icon);
            }

        }
        private void InitializeStruts()
        {
            _struts.Clear();

            int assetCount = _strutRegistry.Assets.Count;

            for (int i = 0; i < _config.StrutSlots; i++)
            {
                var data = _strutRegistry.Assets[i % assetCount];
                _struts.Add(data);
                _inventoryView.SetOnIndex(_config.BuildingSlots + i, data.Icon);
            }
        }
        private void InitializeFoundations()
        {
            _foundations.Clear();

            int assetCount = _foundationRegistry.Assets.Count;

            for (int i = 0; i < _config.FoundationSlots; i++)
            {
                var data = _foundationRegistry.Assets[i % assetCount];
                _foundations.Add(data);
                _inventoryView.SetOnIndex(_config.BuildingSlots + _config.StrutSlots + i, data.Icon);
            }
        }

        private enum BuildMode
        {
            Cursor = -1,
            Build,
            Strut,
            Foundation
        }

    }
}