using SpaceChaser.Core.Player;
using VContainer.Unity;

namespace SpaceChaser.Core.Islands
{
    public class IslandService : IIslandService, IFixedTickable
    {
        private GameConfig _config;
        private IPlayerProvider _player;
        private float _highestIsland = float.MinValue;
        private float _nextDistance = 0f;
        public float DistanceToSpawn = 300f;
        public void IIslandService(GameConfig config, IPlayerProvider player)
        {
            _config = config;
            _player = player;
        }

        public void FixedTick()
        {
            if (_highestIsland + _nextDistance - _player.Transform.position.y >= DistanceToSpawn) return;
        }


    }
}