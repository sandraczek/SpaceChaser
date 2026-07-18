
using SpaceChaser.Core.Inputs;

namespace SpaceChaser.Core.Player.FSM
{
    public sealed class PlayerStateFactory
    {
        public PlayerBaseState InitialState;
        public PlayerBaseState Idle { get; private set; }
        public PlayerBaseState Move { get; private set; }
        public PlayerBaseState Jump { get; private set; }
        public PlayerBaseState Fall { get; private set; }
        public PlayerStateFactory(PlayerFSMContext context)
        {
            Idle = new PlayerIdleState(context, this);
            Move = new PlayerMoveState(context, this);
            Jump = new PlayerJumpState(context, this);
            Fall = new PlayerFallState(context, this);

            InitialState = Idle;
        }
    }
}