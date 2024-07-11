using System;
using Unity.VisualScripting;
using UnityEngine;

namespace HungryWorm
{
    public class GameManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SettingsEvents.JoystickTypeChanged += SettingsEvents_OnJoystickTypeChanged;

            Initialize();
        }
        
        private void OnDisable()
        {
            SettingsEvents.JoystickTypeChanged -= SettingsEvents_OnJoystickTypeChanged;
        }
        
        private void Initialize()
        {
            NullRefChecker.Validate(this);
        }
        
        
        private void SettingsEvents_OnJoystickTypeChanged(JoystickType joystickType)
        {
            
        }
        
    }
}