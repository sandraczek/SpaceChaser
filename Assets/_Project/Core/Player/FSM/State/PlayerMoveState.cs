using SpaceChaser.Core.Inputs;

namespace SpaceChaser.Core.Player.FSM
{
    public class PlayerMoveState : PlayerGroundState
    {
        public PlayerMoveState(PlayerFSMContext context, PlayerStateFactory factory) : base(context, factory)
        {

        }
        public override void Enter()
        {
            base.Enter();
        }
        public override void Update()
        {
            base.Update();
            _context.Controller.CheckForFlip(_context.Inputs.MovementInput.x);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void CheckSwitchStates()
        {
            base.CheckSwitchStates();
            if (_context.Inputs.MovementInput.x == 0f)
            {
                _context.States.SwitchState(_factory.Idle);
                return;
            }
        }
    }
}