using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace _01Script.Player
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/PlayerInputSO")]
    public class PlayerInputSO : ScriptableObject,InputSystem_Actions.IPlayerActions
    {
        public event Action<Vector2> onMovement;
        public event Action onJumpPressed;
        public event Action<bool> onRunKey;
        public event Action<bool> onSitKey;
        public event Action onAttack;
        public event Action onPickUp;

        private InputSystem_Actions _input;
        private Vector2 move; 

        private void OnEnable()
        {
            if (_input == null)
            {
                _input = new InputSystem_Actions();
                _input.Player.SetCallbacks(this);
            }

            _input.Player.Enable();
        }

        private void OnDisable()
        {
            _input.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        public void UpdateMove()
        {
            onMovement?.Invoke(move);   
            
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                onJumpPressed?.Invoke();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onRunKey?.Invoke(true);
            }
            else if (context.canceled)
            {
                onRunKey?.Invoke(false);
            }
        }

        public void OnSit(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onSitKey?.Invoke(true);
            }
            else if (context.canceled)
            {
                onSitKey?.Invoke(false);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            onAttack?.Invoke();
        }
        public void OnPick(InputAction.CallbackContext context)
        {
            onPickUp?.Invoke();
        }
    }
}
