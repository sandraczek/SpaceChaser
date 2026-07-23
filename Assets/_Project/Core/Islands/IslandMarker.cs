using SpaceChaser.Core.Player;
using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Islands
{
    public class IslandMarker : MonoBehaviour
    {
        private IIslandService _island;
        private IPlayerProvider _player;
        private GameConfig _config;

        private Camera _mainCamera;

        [Inject]
        public void Construct(IIslandService island, IPlayerProvider player, GameConfig config)
        {
            _island = island;
            _player = player;
            _config = config;
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (!_player.IsPlayerSpawned) return;

            Vector2 anchorPos = _mainCamera.transform.position;
            Vector2 targetPos = _island.NextIslandPosition;

            Vector2 heading = targetPos - anchorPos;
            float distanceToTarget = heading.magnitude;

            if (distanceToTarget <= 0.001f) return;

            Vector2 directionVector = heading / distanceToTarget;

            float distanceRatio = Mathf.Clamp01(distanceToTarget / _config.MarkerMaxPlayerDistance);
            float markerDistance = _config.MarkerMaxDistance * distanceRatio;

            Vector2 finalPosition = anchorPos + (directionVector * markerDistance);
            float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
            Quaternion finalRotation = Quaternion.Euler(0f, 0f, angle);

            transform.SetPositionAndRotation(finalPosition, finalRotation);
        }
    }
}