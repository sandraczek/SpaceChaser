using System.Collections.Generic;
using SpaceChaser.Core.Inputs;
using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Building
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class BuildPreview : MonoBehaviour
    {
        private IInputReader _inputs;

        private PolygonCollider2D _col;
        private SpriteRenderer _sprite;

        [SerializeField] private float _transparency = 0.5f;

        private ContactFilter2D _contactFilter = new();
        private readonly List<Build> _overlappedBuildsCashed = new();
        private readonly List<Foundation> _overlappedFoundationsCashed = new();
        private readonly List<Collider2D> _overlapResults = new();
        private GameObject _currentPrefab;
        [SerializeField] private LayerMask _allTypeBuildsLayer;

        [Inject]
        public void Construct(IInputReader inputs)
        {
            _inputs = inputs;
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
            transform.position = _inputs.GetWorldAimPosition();
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

        public IReadOnlyList<Build> GetAllBuildContacts(GameObject prefab)
        {
            if (_currentPrefab != prefab) SetTarget(prefab);

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

        public IReadOnlyList<Foundation> GetAllFoundationContacts(GameObject prefab)
        {
            if (_currentPrefab != prefab) SetTarget(prefab);

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

            Debug.Log($"touching {hits} hits: {_overlappedBuildsCashed.Count} foundations");

            return _overlappedFoundationsCashed;
        }

        public List<Collider2D> GetOnCursor()
        {
            Physics2D.SyncTransforms();
            Physics2D.OverlapPoint(_inputs.GetWorldAimPosition(), _contactFilter, _overlapResults);

            return _overlapResults;
        }
    }
}