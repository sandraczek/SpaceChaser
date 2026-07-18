using System;
using UnityEngine;

namespace SpaceChaser.Core.Player.FSM
{
    public class PlayerAirState : PlayerBaseState
    {
        public PlayerAirState(PlayerFSMContext context, PlayerStateFactory playerStateFactory) : base(context, playerStateFactory) { }

        public override void CheckSwitchStates()
        {
            if (Time.time >= _context.Controller.LastJumpTime + _context.Config.MinJumpDuration && _context.Controller.IsGrounded)
            {
                if (Math.Abs(_context.Inputs.MovementInput.x) > 0.1f)
                {
                    _context.States.SwitchState(_factory.Move);
                    return;
                }
                else
                {
                    _context.States.SwitchState(_factory.Idle);
                    return;
                }
            }
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {

        }

        public override void FixedUpdate()
        {

            ApplyStandardMovement(_context.Config.AirAccelerationMult, _context.Config.AirDecelerationMult, _context.Config.MoveSpeedAirMult);
        }

        public override void Update()
        {
            _context.Controller.CheckForFlip(_context.Inputs.MovementInput.x);
        }
    }
}