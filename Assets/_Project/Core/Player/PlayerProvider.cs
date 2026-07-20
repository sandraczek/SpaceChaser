using System;
using SpaceChaser.Core.Highscore;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SpaceChaser.Core.Player
{
    public class PlayerProvider : IPlayerProvider, IPlayerSpawner, IHighscore
    {
        private readonly GameObject _playerPrefab;
        private readonly IObjectResolver _resolver;
        private Player _player;
        public Transform Transform => _player.transform;
        private PlayerController _controller;
        public bool IsPlayerSpawned => _player != null;

        public event Action OnPlayerRegistered;
        public event Action OnPlayerUnregistered;

        //Highscore
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

            player.Initialize();
            _player = player;
            _controller = controller;

            Debug.Log($"<color=#4AF626>[GAMEPLAY]:</color> Player spawned at {spawnPosition}");

            OnPlayerRegistered?.Invoke();
        }
        public void UnregisterPlayer()
        {
            OnPlayerUnregistered?.Invoke();
            _player = null;
        }

    }
}