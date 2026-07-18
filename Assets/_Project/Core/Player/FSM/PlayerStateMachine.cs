using SpaceChaser.Core.Player.FSM;
using UnityEngine;

namespace SpaceChaser.Core.Player.FSM
{
    public sealed class PlayerStateMachine : MonoBehaviour
    {
        public PlayerBaseState CurrentState { get; private set; }
        [SerializeField] private string _nameCurrentState;

        public void Update()
        {
            CurrentState?.Update();
            CurrentState?.CheckSwitchStates();
        }

        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        public void SwitchState(PlayerBaseState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();

            _nameCurrentState = CurrentState.GetType().Name;
            string _ = _nameCurrentState;
        }
    }
}