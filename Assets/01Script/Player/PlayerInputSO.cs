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
        
        
        public void OnMove(InputAction.CallbackContext context)
        {
            onMovement?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
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

        public void OnClick(InputAction.CallbackContext context)
        {
            onAttack?.Invoke();
        }

        public void OnPick(InputAction.CallbackContext context)
        {
            onPickUp?.Invoke();
        }
    }
}
