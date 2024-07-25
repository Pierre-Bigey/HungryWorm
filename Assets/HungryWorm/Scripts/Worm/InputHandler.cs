using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace  HungryWorm
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private VariableJoystick m_variableJoystick;
        
        private bool _canMove = true;
        private void OnEnable()
        {
            GameEvents.GameStarted += AllowMovement;
            GameEvents.GameEnded += StopMovement;
            GameEvents.GamePaused += StopMovement;
            GameEvents.GameUnpaused += AllowMovement;
            
            SettingsEvents.JoystickTypeChanged += SettingsEvents_ModelJoystickTypeChanged;
            PlayerPrefManager playerPrefManager = new PlayerPrefManager();
            SettingsEvents_ModelJoystickTypeChanged(playerPrefManager.GetJoystickType());
        }

        private void OnDisable()
        {
            GameEvents.GameStarted -= AllowMovement;
            GameEvents.GameEnded -= StopMovement;
            GameEvents.GamePaused -= StopMovement;
            GameEvents.GameUnpaused -= AllowMovement;
            
            SettingsEvents.JoystickTypeChanged -= SettingsEvents_ModelJoystickTypeChanged;
        }
        
        private void AllowMovement()
        {
            _canMove = true;
        }
        
        private void StopMovement()
        {
            _canMove = false;
        }

        private void FixedUpdate()
        {
            if (!_canMove)
            {
                return;
            }
            Vector2 direction = Vector2.up * m_variableJoystick.Vertical + Vector2.right * m_variableJoystick.Horizontal;
            WormEvents.WormGoToDirection?.Invoke(direction);
        }
        
        private void SettingsEvents_ModelJoystickTypeChanged(JoystickType joystickType)
        {
            // Debug.Log("GameScreen Joystick changed : " + joystickType);
            
            switch (joystickType)
            {
                case JoystickType.FIXED:
                    m_variableJoystick.SetMode(global::JoystickType.Fixed);
                    break;
                case JoystickType.FLOATING:
                    m_variableJoystick.SetMode(global::JoystickType.Floating);
                    break;
                case JoystickType.DYNAMIC:
                    m_variableJoystick.SetMode(global::JoystickType.Dynamic);
                    break;
            }
        }
    }
}