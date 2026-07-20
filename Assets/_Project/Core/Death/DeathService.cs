using System;
using SpaceChaser.Core.Highscore;
using SpaceChaser.Core.Player;
using VContainer.Unity;

namespace SpaceChaser.Core.Death
{
    public class DeathService : IDeathService, IFixedTickable, IStartable, IDisposable
    {
        private readonly IHighscore _highscore;
        private readonly IPlayerProvider _player;
        private readonly GameConfig _config;

        private bool _canDie = false;

        public event Action OnDeath;
        public DeathService(IHighscore highscore, IPlayerProvider player, GameConfig config)
        {
            _highscore = highscore;
            _config = config;
            _player = player;
        }

        public void Start()
        {
            _player.OnPlayerRegistered += OnPlayerRegistered;
        }
        public void Dispose()
        {
            _player.OnPlayerRegistered -= OnPlayerRegistered;
        }

        public void FixedTick()
        {
            if (!_player.IsPlayerSpawned) return;
            if (_highscore.High - _highscore.Current > _config.DeathDistance && _canDie)
            {
                RegisterDeath();
            }
        }

        private void OnPlayerRegistered()
        {
            _canDie = true;
        }

        private void RegisterDeath()
        {
            OnDeath?.Invoke();
            _canDie = false;
        }
    }
}