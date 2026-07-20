using SpaceChaser.Core.Death;
using SpaceChaser.Core.Inputs;
using SpaceChaser.Core.Player.FSM;
using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerStateMachine))]
    public class Player : MonoBehaviour
    {
        private PlayerConfig _config;
        private IDeathService _death;

        private PlayerController _controller;
        private PlayerStateMachine _stateMachine;
        private PlayerStateFactory _factory;

        [Inject]
        public void Construct(PlayerConfig config, IInputReader inputs, IDeathService death)
        {
            _controller = GetComponent<PlayerController>();
            _stateMachine = GetComponent<PlayerStateMachine>();
            _config = config;
            _death = death;

            PlayerFSMContext context = new()
            {
                Inputs = inputs,
                States = _stateMachine,
                Controller = _controller,
                Config = _config
            };
            _factory = new(context);
        }

        public void Initialize()
        {
            _stateMachine.SwitchState(_factory.InitialState);
        }

        private void OnEnable()
        {
            _death.OnDeath += HandleDeath;
        }
        private void OnDisable()
        {
            _death.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {

        }
    }
}