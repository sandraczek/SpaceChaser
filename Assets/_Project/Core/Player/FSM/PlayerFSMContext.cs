
using SpaceChaser.Core.Inputs;

namespace SpaceChaser.Core.Player.FSM
{
    public struct PlayerFSMContext
    {
        public IInputReader Inputs;
        public PlayerConfig Config;
        public PlayerController Controller;
        public PlayerStateMachine States;
    }
}