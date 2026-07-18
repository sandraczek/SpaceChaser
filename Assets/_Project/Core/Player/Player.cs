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
        private PlayerController _controller;
        private PlayerConfig _config;

        private PlayerStateMachine _stateMachine;
        private PlayerStateFactory _factory;

        [Inject]
        public void Construct(PlayerConfig config, IInputReader inputs)
        {
            _controller = GetComponent<PlayerController>();
            _stateMachine = GetComponent<PlayerStateMachine>();
            _config = config;

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
    }
}