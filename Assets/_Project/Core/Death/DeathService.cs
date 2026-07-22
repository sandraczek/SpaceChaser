using System;
using System.Collections.Generic;
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
        private readonly IReadOnlyList<IResetable> _resetables;

        private bool _canDie = false;

        public event Action OnDeath;
        public DeathService(IHighscore highscore, IPlayerProvider player, GameConfig config, IReadOnlyList<IResetable> resetables)
        {
            _highscore = highscore;
            _config = config;
            _player = player;
            _resetables = resetables;
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

            ResetAll();
        }

        public void ResetAll()
        {
            foreach (var resetable in _resetables)
            {
                resetable.Reset();
            }

            _canDie = true;
        }
    }
}