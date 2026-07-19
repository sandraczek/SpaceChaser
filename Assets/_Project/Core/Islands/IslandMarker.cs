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

        [Inject]
        public void Construct(IIslandService island, IPlayerProvider player, GameConfig config)
        {
            _island = island;
            _player = player;
            _config = config;
        }

        private void FixedUpdate()
        {
            if (!_player.IsPlayerSpawned) return;

            Vector2 playerPos = _player.Transform.position;
            Vector2 targetPos = _island.NextIslandPosition;

            Vector2 heading = targetPos - playerPos;
            float playerDistance = heading.magnitude;

            if (playerDistance <= 0.001f) return;

            Vector2 directionVector = heading / playerDistance;

            float playerDistanceRatio = Mathf.Clamp01(playerDistance / _config.MarkerMaxPlayerDistance);
            float markerDistance = _config.MarkerMaxDistance * playerDistanceRatio;

            Vector2 finalPosition = playerPos + (directionVector * markerDistance);

            float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;

            Quaternion finalRotation = Quaternion.Euler(0f, 0f, angle);

            transform.SetPositionAndRotation(finalPosition, finalRotation);
        }
    }
}