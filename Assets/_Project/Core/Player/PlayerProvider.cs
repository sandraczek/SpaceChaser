using System;
using SpaceChaser.Core.Death;
using SpaceChaser.Core.Highscore;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SpaceChaser.Core.Player
{
    public class PlayerProvider : IPlayerProvider, IPlayerSpawner, IHighscore, IResetable
    {
        private readonly GameObject _playerPrefab;
        private readonly IObjectResolver _resolver;
        private Player _player;
        public Transform Transform => _player.transform;
        private PlayerController _controller;
        private PlayerBuildTool _build;
        public bool IsPlayerSpawned => _player != null;
        private Vector2 _spawnPosition;

        public event Action OnPlayerRegistered;
        public event Action OnPlayerUnregistered;

        //Highscore
        public float AllTimeHigh { get; private set; } = -1000f;
        public float High => _controller.MaxElevation;
        public float Current => _controller.Elevation;
        public bool Available => IsPlayerSpawned;


        public PlayerProvider(IObjectResolver resolver, GameObject playerPrefab)
        {
            _playerPrefab = playerPrefab;
            _resolver = resolver;
        }
        public void SpawnPlayer(Vector2 spawnPosition)
        {
            if (float.IsNaN(spawnPosition.x) || float.IsNaN(spawnPosition.y) || float.IsInfinity(spawnPosition.x) || float.IsInfinity(spawnPosition.y))
            {
                Debug.LogError($"Tried spawning player on corrupted coordinates: {spawnPosition}.");
                return;
            }
            _spawnPosition = spawnPosition;

            GameObject playerInstance = _resolver.Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);
            playerInstance.name = "Player";

            if (!playerInstance.TryGetComponent(out Player player))
            {
                Debug.LogError("Tried spawning player with no Player component. Aborting");
                GameObject.Destroy(playerInstance);
                return;
            }
            if (!playerInstance.TryGetComponent(out PlayerController controller))
            {
                Debug.LogError("Tried spawning player with no PlayerController component. Aborting");
                GameObject.Destroy(playerInstance);
                return;
            }
            if (!playerInstance.TryGetComponent(out PlayerBuildTool build))
            {
                Debug.LogError("Tried spawning player with no PlayerBuildComponent component. Aborting");
                GameObject.Destroy(playerInstance);
                return;
            }

            player.Initialize();
            _player = player;

            controller.Initialize();
            _controller = controller;

            _build = build;


            Debug.Log($"<color=#4AF626>[GAMEPLAY]:</color> Player spawned at {spawnPosition}");

            OnPlayerRegistered?.Invoke();
        }
        public void UnregisterPlayer()
        {
            OnPlayerUnregistered?.Invoke();
            _player = null;
        }

        public void Reset()
        {
            Transform.position = _spawnPosition;
            AllTimeHigh = High;
            _controller.Initialize();
            _build.ResetState();
        }
    }
}