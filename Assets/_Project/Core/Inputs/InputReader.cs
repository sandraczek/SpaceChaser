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

        public event Action<bool> OnPrimaryActionHeld;
        public event Action<bool> OnSecondaryActionHeld;
        public bool RotateRightHeld { get; private set; } = false;
        public bool RotateLeftHeld { get; private set; } = false;
        public event Action<int> OnNumberKeyPressed;

        public void Initialize()
        {
            if (_inputActions != null) return;
            _inputActions = new GameInput();

            _inputActions.Player.SetCallbacks(this);
            _inputActions.Player.Enable();
        }

        public void OnPrimaryAction(InputAction.CallbackContext context)
        {
            if (context.performed) OnPrimaryActionHeld?.Invoke(true);
            if (context.canceled) OnPrimaryActionHeld?.Invoke(false);
        }

        public void OnSecondaryAction(InputAction.CallbackContext context)
        {
            if (context.performed) OnSecondaryActionHeld?.Invoke(true);
            if (context.canceled) OnSecondaryActionHeld?.Invoke(false);
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
        public Vector2 GetWorldAimPosition()
        {
            return Camera.main.ScreenToWorldPoint(CursorScreenPosition);
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

        public void OnRotateRight(InputAction.CallbackContext context)
        {
            if (context.performed) RotateRightHeld = true;
            if (context.canceled) RotateRightHeld = false;
        }

        public void OnRotateLeft(InputAction.CallbackContext context)
        {
            if (context.performed) RotateLeftHeld = true;
            if (context.canceled) RotateLeftHeld = false;
        }

        public void OnKeyn(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(0);
        }
        public void OnKey1(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(1);
        }

        public void OnKey2(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(2);
        }

        public void OnKey3(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(3);
        }

        public void OnKey4(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(4);
        }

        public void OnKey5(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(5);
        }

        public void OnKey6(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(6);
        }

        public void OnKey7(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(7);
        }

        public void OnKey8(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(8);
        }

        public void OnKey9(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(9);
        }

        public void OnKey10(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(10);
        }
        public void OnKey11(InputAction.CallbackContext context)
        {
            if (context.performed) OnNumberKeyPressed?.Invoke(11);
        }
        public void OnKey12(InputAction.CallbackContext context)
        {
            //if (context.performed) OnNumberKeyPressed?.Invoke(12);
        }
    }
}