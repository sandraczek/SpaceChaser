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

        event Action OnPrimaryActionPressed;
    }
}