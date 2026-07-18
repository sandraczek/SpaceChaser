using SpaceChaser.Core.Inputs;
using UnityEngine;

namespace SpaceChaser.Core.Player.FSM
{
    public abstract class PlayerBaseState
    {
        protected PlayerFSMContext _context;
        protected PlayerStateFactory _factory;
        public PlayerBaseState(PlayerFSMContext context, PlayerStateFactory factory)
        {
            _context = context;
            _factory = factory;
        }
        public abstract void Enter();
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void Exit();
        public abstract void CheckSwitchStates();

        protected void ApplyStandardMovement(float accelerationMult = 1f, float decelerationMult = 1f, float speedMult = 1f)
        {
            float xInput = _context.Inputs.MovementInput.x;
            float targetSpeed = xInput * _context.Config.MoveSpeed * speedMult;

            bool isAccelerating = Mathf.Abs(xInput) > _context.Config.MinAccelerationInput;
            float currentAccel = isAccelerating ?
                _context.Config.Acceleration * accelerationMult : _context.Config.Deceleration * decelerationMult;

            float newVelocityX = Mathf.MoveTowards(
                _context.Controller.VelocityX,
                targetSpeed,
                currentAccel * Time.fixedDeltaTime
            );

            _context.Controller.VelocityX = newVelocityX;

        }
    }
}