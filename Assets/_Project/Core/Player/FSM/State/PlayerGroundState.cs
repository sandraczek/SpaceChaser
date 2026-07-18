using SpaceChaser.Core.Inputs;
using UnityEngine;

namespace SpaceChaser.Core.Player.FSM
{
    public abstract class PlayerGroundState : PlayerBaseState
    {
        public PlayerGroundState(PlayerFSMContext context, PlayerStateFactory factory) : base(context, factory)
        {

        }

        public override void CheckSwitchStates()
        {
            if (Time.time < _context.Inputs.JumpPressedTime + _context.Config.JumpBuffor)
            {
                _context.States.SwitchState(_factory.Jump);
                return;
            }
            if (Time.time > _context.Controller.LastGroundedTime + _context.Config.CoyoteTime)
            {
                _context.States.SwitchState(_factory.Fall);
                return;
            }
        }

        public override void FixedUpdate()
        {
            ApplyStandardMovement();
        }
        public override void Enter()
        {

        }
        public override void Update()
        {

        }
        public override void Exit()
        {

        }
    }
}