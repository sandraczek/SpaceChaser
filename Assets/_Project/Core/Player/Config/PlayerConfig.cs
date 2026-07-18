using UnityEngine;

namespace SpaceChaser.Core.Player
{
    [CreateAssetMenu(menuName = "Player/Config")]
    public sealed class PlayerConfig : ScriptableObject
    {
        public float GroundCheckSizeY;
        public float GroundCheckDistance;
        public float GravityScale;

        public float MoveSpeed;
        public float Acceleration;
        public float Deceleration;
        public float MoveSpeedAirMult;
        public float AirAccelerationMult;
        public float AirDecelerationMult;
        public float MinAccelerationInput;

        public float JumpForce;
        public float MinJumpDuration;
        public float JumpBuffor;
        public float CoyoteTime;
        public float FallGravityMult;
        public float LowJumpGravityMultiplier;
    }
}