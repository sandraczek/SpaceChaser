using System.Collections.Generic;
using SpaceChaser.Core.Inputs;
using SpaceChaser.Core.Player;
using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Building
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class BuildPreview : MonoBehaviour
    {
        private IInputReader _inputs;
        private IPlayerProvider _player;
        private GameConfig _config;

        private PolygonCollider2D _col;
        private SpriteRenderer _sprite;

        [SerializeField] private float _transparency = 0.5f;

        private ContactFilter2D _contactFilter = new();
        private readonly List<Build> _overlappedBuildsCashed = new();
        private readonly List<Strut> _overlappedStrutsCashed = new();
        private readonly List<Foundation> _overlappedFoundationsCashed = new();
        private readonly List<Collider2D> _overlapResults = new();
        private GameObject _currentPrefab;
        [SerializeField] private LayerMask _allTypeBuildsLayer;
        private bool _buildMode = false;

        [Inject]
        public void Construct(IInputReader inputs, IPlayerProvider player, GameConfig config)
        {
            _inputs = inputs;
            _player = player;
            _config = config;
        }

        private void Awake()
        {
            _col = GetComponent<PolygonCollider2D>();
            _sprite = GetComponentInChildren<SpriteRenderer>();

            _contactFilter.useTriggers = false;
            _contactFilter.useLayerMask = true;
            _contactFilter.layerMask = _allTypeBuildsLayer;
        }
        private void Start()
        {
            Color color = _sprite.color;
            color.a = _transparency;
            _sprite.color = color;
        }
        private void Update()
        {
            if (!_player.IsPlayerSpawned) return;
            if (!_buildMode)
            {
                transform.position = _inputs.GetWorldAimPosition();
                if ((_player.Transform.position - transform.position).sqrMagnitude > _config.BuildingDistance * _config.BuildingDistance)
                    _sprite.enabled = false;
                else
                    _sprite.enabled = true;
            }
        }
        public void RemoveTarget()
        {
            gameObject.SetActive(false);
        }
        public void SetTarget(GameObject prefab)
        {
            _currentPrefab = prefab;
            var targetSprite = prefab.GetComponentInChildren<SpriteRenderer>();
            var targetCollider = prefab.GetComponent<PolygonCollider2D>();

            if (targetSprite != null)
            {
                _sprite.enabled = true;
                _sprite.sprite = targetSprite.sprite;
                _sprite.transform.localScale = targetSprite.transform.localScale;
                Color color = targetSprite.color;
                color.a = _transparency;
                _sprite.color = color;
            }

            if (targetCollider != null)
            {
                _col.points = targetCollider.points;
            }
            gameObject.SetActive(true);
        }

        public void SetRotation(float rotation)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }

        public IReadOnlyList<Build> GetAllBuildContacts()
        {
            Physics2D.SyncTransforms();
            int hits = _col.Overlap(_contactFilter, _overlapResults);

            _overlappedBuildsCashed.Clear();
            for (int i = 0; i < hits; i++)
            {
                if (_overlapResults[i] == _col) continue;

                if (_overlapResults[i].TryGetComponent(out Build build))
                {
                    _overlappedBuildsCashed.Add(build);
                }
            }

            Debug.Log($"touching {hits} hits: {_overlappedBuildsCashed.Count} builds");

            return _overlappedBuildsCashed;
        }
        public IReadOnlyList<Strut> GetAllStrutContacts()
        {

            Physics2D.SyncTransforms();
            int hits = _col.Overlap(_contactFilter, _overlapResults);

            _overlappedStrutsCashed.Clear();
            for (int i = 0; i < hits; i++)
            {
                if (_overlapResults[i] == _col) continue;

                if (_overlapResults[i].TryGetComponent(out Strut strut))
                {
                    _overlappedStrutsCashed.Add(strut);
                }
            }

            Debug.Log($"touching {hits} hits: {_overlappedStrutsCashed.Count} struts");

            return _overlappedStrutsCashed;
        }

        public IReadOnlyList<Foundation> GetAllFoundationContacts()
        {
            Physics2D.SyncTransforms();
            int hits = _col.Overlap(_contactFilter, _overlapResults);

            _overlappedFoundationsCashed.Clear();
            for (int i = 0; i < hits; i++)
            {
                if (_overlapResults[i] == _col) continue;

                if (_overlapResults[i].TryGetComponent(out Foundation foundation))
                {
                    _overlappedFoundationsCashed.Add(foundation);
                }

                Debug.Log(_overlapResults[i].name);
            }

            Debug.Log($"touching {hits} hits: {_overlappedFoundationsCashed.Count} foundations");

            return _overlappedFoundationsCashed;
        }

        public List<Collider2D> GetOnCursor()
        {
            Physics2D.SyncTransforms();
            Physics2D.OverlapPoint(_inputs.GetWorldAimPosition(), _contactFilter, _overlapResults);

            return _overlapResults;
        }

        public void StartBuilding()
        {
            _buildMode = true;
        }

        public void ExitBuilding()
        {
            _buildMode = false;
        }
    }
}