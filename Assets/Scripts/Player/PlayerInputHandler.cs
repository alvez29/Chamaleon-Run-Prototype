using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public event Action OnJumpStarted;
        public event Action OnJumpCanceled;
        public event Action OnSwitchColorStarted;
        
        private PlayerActionMap m_playerActionMap;
        private bool m_isJumpPressed;
        
        public bool IsJumpPressed => m_isJumpPressed;

        private void Awake()
        {
            m_playerActionMap = new PlayerActionMap();
            BindInputActions();
        }
        
        private void OnEnable()
        {
            m_playerActionMap?.Enable();
        }

        private void OnDisable()
        {
            m_playerActionMap?.Disable();
        }

        private void OnDestroy()
        {
            UnbindInputActions();
        }

        private void BindInputActions()
        {
            if (m_playerActionMap == null) return;
            
            m_playerActionMap.Player.Jump.started += HandleJumpStarted;
            m_playerActionMap.Player.Jump.canceled += HandleJumpCanceled;
                
            m_playerActionMap.Player.SwitchColor.started += HandleSwitchColorStarted;
        }

        private void UnbindInputActions()
        {
            if (m_playerActionMap == null) return;

            m_playerActionMap.Player.Jump.started -= HandleJumpStarted;
            m_playerActionMap.Player.Jump.canceled -= HandleJumpCanceled;
                
            m_playerActionMap.Player.SwitchColor.started -= HandleSwitchColorStarted;
        }
        
        private void HandleJumpStarted(InputAction.CallbackContext ctx)
        {
            m_isJumpPressed = true;
            OnJumpStarted?.Invoke();   
        }
        
        private void HandleJumpCanceled(InputAction.CallbackContext ctx)
        {
            m_isJumpPressed = false;
            OnJumpCanceled?.Invoke();
        }
        
        private void HandleSwitchColorStarted(InputAction.CallbackContext ctx) => OnSwitchColorStarted?.Invoke();
    }
}
