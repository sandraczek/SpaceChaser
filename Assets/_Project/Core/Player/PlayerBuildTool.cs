using System.Collections.Generic;
using SpaceChaser.Core.Building;
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

        private readonly List<BuildData> _builds = new(10);
        private readonly List<StrutData> _struts = new(10);
        private readonly List<FoundationData> _foundations = new(10);
        private int _index;
        private BuildMode _mode = BuildMode.Cursor;
        private float _currentRotation = 0f;


        [Inject]
        public void Construct(
            IBuildService buildService,
            IInputReader inputs,
            BuildPreview preview,
            IInventoryService inventoryService,
            GameConfig config,
            IAssetRegistry<BuildData> buildRegistry,
            IAssetRegistry<StrutData> strutRegistry,
            IAssetRegistry<FoundationData> foundationRegistry
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
        }
        private void Start()
        {
            RandomizeBuilds();

            //UnselectBuild();
            SelectBuildAt(0);
        }
        public void OnEnable()
        {
            _inputs.OnPrimaryActionPressed += HandlePrimaryAction;
            _inputs.OnSecondaryActionPressed += HandleSecondaryAction;
        }
        public void OnDisable()
        {
            _inputs.OnPrimaryActionPressed -= HandlePrimaryAction;
            _inputs.OnSecondaryActionPressed -= HandleSecondaryAction;
        }

        private void HandlePrimaryAction()
        {
            if (_index < 0) return;

            bool res = false;
            List<ItemAmount> recipe = null;

            switch (_mode)
            {
                case BuildMode.Cursor:
                    return;
                case BuildMode.Build:
                    var build = _builds[_index];
                    recipe = build.Recipe;
                    if (_inventory.Has(build.Recipe))
                        res = _buildService.Build(build, _inputs.GetWorldAimPosition(), _currentRotation);

                    break;
                case BuildMode.Strut:
                    var strut = _struts[_index];
                    recipe = strut.Recipe;
                    if (_inventory.Has(strut.Recipe))
                        res = _buildService.BuildStrut(strut, _preview.GetAllBuildContacts(strut.Prefab.gameObject), _inputs.GetWorldAimPosition(), _currentRotation);

                    break;
                case BuildMode.Foundation:
                    var foundation = _foundations[_index];
                    recipe = foundation.Recipe;
                    if (_inventory.Has(foundation.Recipe))
                        res = _buildService.BuildFoundation(foundation, _preview.GetAllFoundationContacts(foundation.Prefab.gameObject), _inputs.GetWorldAimPosition(), 45f);

                    ////////// FIXED ROTATION -------------------------/// -------------    HERE

                    break;
            }

            if (res)
            {
                _inventory.Remove(recipe);
            }
        }

        private void HandleSecondaryAction()
        {
            var cols = _preview.GetOnCursor();
            foreach (var col in cols)
            {
                if (col.TryGetComponent(out Strut strut))
                {
                    if (_buildService.RemoveStrut(strut))
                        _inventory.Add(new List<ItemAmount>(strut.Data.Recipe), _config.SalvageRate);
                }
                else if (col.TryGetComponent(out Build build))
                {
                    if (_buildService.RemoveBuild(build))
                        _inventory.Add(new List<ItemAmount>(build.Data.Recipe), _config.SalvageRate);
                }
                else if (col.TryGetComponent(out Foundation foundation))
                {
                    if (_buildService.RemoveFoundation(foundation))
                        _inventory.Add(new List<ItemAmount>(foundation.Data.Recipe), _config.SalvageRate);

                }
                else if (col.TryGetComponent(out Resource resource))
                {
                    if (_buildService.RemoveResource(resource))
                        _inventory.Add(new List<ItemAmount>(resource.Data.Resources));

                }
                else continue;

                break;
            }

        }

        private void SelectBuildAt(int index)
        {
            if (_builds == null || _builds.Count <= index) return;

            _index = index;
            _mode = BuildMode.Build;
            var build = _builds[index];
            _preview.SetTarget(build.Prefab.gameObject);

        }
        private void SelectStrutAt(int index)
        {
            if (_struts == null || _struts.Count <= index) return;

            _index = index;
            _mode = BuildMode.Strut;
            var strut = _struts[index];
            _preview.SetTarget(strut.Prefab.gameObject);

        }
        private void SelectFoundationAt(int index)
        {
            if (_foundations == null || _foundations.Count <= index) return;

            _index = index;
            _mode = BuildMode.Foundation;
            var foundation = _foundations[index];
            _preview.SetTarget(foundation.Prefab.gameObject);

        }
        private void UnselectBuild()
        {
            _index = -1;
            _preview.RemoveTarget();
            _mode = BuildMode.Cursor;
        }

        private void RandomizeBuilds()
        {
            _builds.Clear();

            int assetCount = _buildRegistry.Assets.Count;

            for (int i = 0; i < _config.InventorySlots; i++)
            {
                int randI = Random.Range(0, assetCount);
                _builds.Add(_buildRegistry.Assets[randI]);
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