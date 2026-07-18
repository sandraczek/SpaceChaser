using UnityEngine;

namespace SpaceChaser.Core.Player.FSM
{
    public sealed class PlayerJumpState : PlayerAirState
    {
        public PlayerJumpState(PlayerFSMContext context, PlayerStateFactory playerStateFactory) : base(context, playerStateFactory) { }

        public override void CheckSwitchStates()
        {
            base.CheckSwitchStates();
            if (Time.time >= _context.Controller.LastJumpTime + _context.Config.MinJumpDuration && _context.Controller.VelocityY < 0f)
            {
                _context.States.SwitchState(_factory.Fall);
                return;
            }
        }

        public override void Enter()
        {
            base.Enter();
            _context.Controller.VelocityY = _context.Config.JumpForce;

            _context.Inputs.ConsumeJump();
            _context.Controller.LastJumpTime = Time.time;
        }

        public override void Exit()
        {
            _context.Controller.SetGravity(_context.Config.GravityScale);
            base.Exit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void Update()
        {
            base.Update();
            if (!_context.Inputs.JumpInput && _context.Controller.VelocityY > 0f)
            {
                _context.Controller.SetGravity(_context.Config.GravityScale * _context.Config.LowJumpGravityMultiplier);
            }
        }
    }
}