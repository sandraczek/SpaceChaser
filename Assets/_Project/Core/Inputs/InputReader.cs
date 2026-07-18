using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace SpaceChaser.Core.Inputs
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Inputs/Input Reader")]
    public sealed class InputReader : ScriptableObject, GameInput.IPlayerActions, IInputReader
    {
        private GameInput _inputActions;

        public Vector2 MovementInput { get; private set; }
        public Vector2 CursorScreenPosition { get; private set; }
        public bool JumpInput { get; private set; }
        public float JumpPressedTime { get; private set; } = float.MinValue;

        public event Action OnPrimaryActionPressed;

        public void Initialize()
        {
            if (_inputActions != null) return;
            _inputActions = new GameInput();

            _inputActions.Player.SetCallbacks(this);
            _inputActions.Player.Enable();
        }

        public void OnPrimaryAction(InputAction.CallbackContext context)
        {
            if (context.performed) OnPrimaryActionPressed?.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {

        }

        public void OnInteract(InputAction.CallbackContext context)
        {

        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                JumpInput = true;
                JumpPressedTime = (float)context.startTime;
            }
            if (context.canceled)
            {
                JumpInput = false;
            }
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            CursorScreenPosition = context.ReadValue<Vector2>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnNext(InputAction.CallbackContext context)
        {

        }

        public void OnPrevious(InputAction.CallbackContext context)
        {

        }

        public void OnSprint(InputAction.CallbackContext context)
        {

        }
        public void ConsumeJump()
        {
            JumpPressedTime = float.MinValue;
        }
    }
}