using System;
using System.Collections.Generic;
using System.Linq;
using SpaceChaser.Core.Death;
using SpaceChaser.Core.Player;
using UnityEngine;
using VContainer.Unity;

namespace SpaceChaser.Core.Islands
{
    public class IslandService : IIslandService, IStartable, IFixedTickable, IResetable
    {
        private readonly GameConfig _config;
        private readonly IPlayerProvider _player;
        private readonly IIslandFactory _factory;

        private readonly Queue<Island> _queue = new();

        private float _highestIsland = 0f;
        private float _lastX = 0f;
        private float _nextHeight = 0f;
        private readonly float _distanceToSpawn = 100f;

        public Vector2 NextIslandPosition { get; private set; } = new(0f, float.MinValue);

        public IslandService(GameConfig config, IPlayerProvider player, IIslandFactory factory)
        {
            _config = config;
            _player = player;
            _factory = factory;
        }

        public void Start()
        {
            _highestIsland = _config.FloorY;
            _lastX = 0f;
            _nextHeight = 0f;
            _queue.Clear();
            CalculateNextDistance();
        }

        public void FixedTick()
        {

            if (!_player.IsPlayerSpawned) return;

            if (_nextHeight - _player.Transform.position.y < _distanceToSpawn)
                SpawnNewIsland();

            CheckPlayerAboveNextIsland();
        }

        private void CalculateNextDistance()
        {
            _nextHeight = _highestIsland + _config.AverageDistance + 2f * (UnityEngine.Random.value - 0.5f) * _config.DistanceFlactuation;
        }

        private void SpawnNewIsland()
        {

            float x = _config.IslandMaxXFlactuation * (UnityEngine.Random.value - 0.5f) * 2f + _lastX * 0.5f;
            x = Mathf.Clamp(x, _config.IslandMinX, _config.IslandMaxX);
            float y = _nextHeight;
            Island island = _factory.Create(new(x, y));

            if (_queue.Count == 0)
            {
                NextIslandPosition = new(x, y);
            }
            _queue.Enqueue(island);

            _highestIsland = y;
            _lastX = x;
            CalculateNextDistance();
        }

        public void SetupIslands()
        {
            while (true)
            {
                if (_nextHeight - _config.FloorY >= _distanceToSpawn) break;

                SpawnNewIsland();
            }
        }

        private void CheckPlayerAboveNextIsland()
        {
            if (_player.Transform.position.y > NextIslandPosition.y)
                NextIslandPosition = _queue.Dequeue().transform.position;
        }

        public void Reset()
        {
            Start();
            SetupIslands();
        }
    }
}