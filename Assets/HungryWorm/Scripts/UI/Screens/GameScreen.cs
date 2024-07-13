
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HungryWorm
{
    public class GameScreen : UIScreen
    {
        //private InputSystem_Actions m_inputSystemActions;
        
        [SerializeField] private Slider m_healthBar;
        
        [SerializeField] private TMP_Text m_scoreText;

        [SerializeField] private TMP_Text m_timerText;
        
        [SerializeField] private VariableJoystick m_variableJoystick;

        /*private void Start()
        {
            m_inputSystemActions = new InputSystem_Actions();
        }*/

        public override void Initialize()
        {
            SubscribeEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            SettingsEvents.JoystickTypeChanged += SettingsEvents_ModelJoystickTypeChanged;
            
            //m_inputSystemActions.UI.Cancel.performed += CancelAction;
            
        }
        
        public void OnPauseButtonPressed()
        {
            UIEvents.PauseScreenShown?.Invoke();
        }

        private void UnsubscribeEvents()
        {
            SettingsEvents.JoystickTypeChanged -= SettingsEvents_ModelJoystickTypeChanged;
        }

        private void SettingsEvents_ModelJoystickTypeChanged(JoystickType joystickType)
        {
            Debug.Log("GameScreen Joystick changed : " + joystickType);
            
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