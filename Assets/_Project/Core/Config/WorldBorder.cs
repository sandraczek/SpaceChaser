using UnityEngine;
using VContainer;

namespace SpaceChaser.Core
{
    public class WorldBorder : MonoBehaviour
    {
        private GameConfig _config;
        [SerializeField] private BoxCollider2D left;
        [SerializeField] private BoxCollider2D right;
        [SerializeField] private BoxCollider2D up;

        [Inject]
        public void Construct(GameConfig config)
        {
            _config = config;
        }
        private void Start()
        {
            left.size = new(1, _config.MaxBounds.y * 2f);
            right.size = new(1, _config.MaxBounds.y * 2f);
            left.offset = new(_config.MinBounds.x, 0f);
            right.offset = new(_config.MaxBounds.x, 0f);
            up.offset = new(0f, _config.MaxBounds.y);
            up.size = new((_config.MaxBounds.x - _config.MinBounds.x) * 2f, 1f);
        }
    }
}