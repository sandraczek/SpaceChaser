using System;
using UnityEngine;

namespace SpaceChaser.Core.Inputs
{
    public interface IInputReader
    {
        Vector2 MovementInput { get; }
        Vector2 CursorScreenPosition { get; }
        bool JumpInput { get; }
        float JumpPressedTime { get; }
        public void ConsumeJump();

        event Action<bool> OnPrimaryActionHeld;
        event Action<bool> OnSecondaryActionHeld;
        public bool RotateRightHeld { get; }
        public bool RotateLeftHeld { get; }
        public event Action<int> OnNumberKeyPressed;
        public Vector2 GetWorldAimPosition();
    }
}