using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace HungryWorm
{
    public class SettingsScreen : UIScreen
    {
        [Header("Audio Sliders")]
        [Tooltip("Master volume slider")]
        [SerializeField] private Slider m_MasterVolumeSlider;
        [Tooltip("SFX volume slider")]
        [SerializeField] private Slider m_SFXVolumeSlider;
        [Tooltip("Music volume slider")]
        [SerializeField] private Slider m_MusicVolumeSlider;
        
        [Header("Audio Labels")]
        [Tooltip("Master volume label")]
        [SerializeField] private TMP_Text m_MasterVolumeLabel;
        [Tooltip("SFX volume label")]
        [SerializeField] private TMP_Text m_SFXVolumeLabel;
        [Tooltip("Music volume label")]
        [SerializeField] private TMP_Text m_MusicVolumeLabel;

        [Header("Joysticks toggles")]
        [Tooltip("Fixed joystick toggle")]
        [SerializeField] private Toggle m_FixedJoystickToggle;
        [Tooltip("Floating joystick toggle")]
        [SerializeField] private Toggle m_FloatingJoystickToggle;
        [Tooltip("Dynamic joystick toggle")]
        [SerializeField] private Toggle m_DynamicJoystickToggle;
        
        public override void Initialize()
        {
            SubscribeToEvents();
            //SettingsEvents.SettingInitialized?.Invoke();
        }
        
        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }


        private void SubscribeToEvents()
        {
            // Subscribe to "*Set" events from Presenter Initialization
            SettingsEvents.MasterSliderSet += MasterVolumeSetHandler;
            SettingsEvents.SFXSliderSet += SFXVolumeSetHandler;
            SettingsEvents.MusicSliderSet += MusicVolumeSetHandler;
            SettingsEvents.JoystickTypeButtonSet += JoystickTypeButtonSetHandler;
        }

        private void UnsubscribeFromEvents()
        {
            // Unsubscribe from "*Set" events
            SettingsEvents.MasterSliderSet -= MasterVolumeSetHandler;
            SettingsEvents.SFXSliderSet -= SFXVolumeSetHandler;
            SettingsEvents.MusicSliderSet -= MusicVolumeSetHandler;
            SettingsEvents.JoystickTypeButtonSet -= JoystickTypeButtonSetHandler;
        }
        
        // Receive notifications from the Presenter

        // Update master volume values from Presenter Initializtion
        private void MasterVolumeSetHandler(float volume)
        {
            m_MasterVolumeSlider.value = volume;
            m_MasterVolumeLabel.text = volume.ToString("F0");
        }

        // Update SFX volume values from Presenter Initializtion
        private void SFXVolumeSetHandler(float volume)
        {
            m_SFXVolumeSlider.value = volume;
            m_SFXVolumeLabel.text = volume.ToString("F0");
        }

        // Update music volume values from Presenter Initializtion
        private void MusicVolumeSetHandler(float volume)
        {
            m_MusicVolumeSlider.value = volume;
            m_MusicVolumeLabel.text = volume.ToString("F0");
        }
        
        private void JoystickTypeButtonSetHandler(JoystickType joystickType)
        {
            m_FixedJoystickToggle.isOn = joystickType == JoystickType.FIXED;
            m_DynamicJoystickToggle.isOn = joystickType == JoystickType.DYNAMIC;
            m_FloatingJoystickToggle.isOn = joystickType == JoystickType.FLOATING;
        }

        public void BackButtonClicked()
        {
            SettingsEvents.SaveAll?.Invoke();
            UIEvents.ScreenClosed?.Invoke();
        }
        
        public void ResetButtonClicked()
        {
            SettingsEvents.ResetAll?.Invoke();
        }
        
        public void MasterVolumeChangeHandler(float newValue)
        {
            m_MasterVolumeLabel.text = newValue.ToString("F0");
            SettingsEvents.MasterSliderChanged?.Invoke(newValue);
        }

        public void SFXVolumeChangeHandler(float newValue)
        {
            m_SFXVolumeLabel.text = newValue.ToString("F0");
            SettingsEvents.SFXSliderChanged?.Invoke(newValue);
        }
        
        public void MusicVolumeChangeHandler(float newValue)
        {
            m_MusicVolumeLabel.text = newValue.ToString("F0");
            SettingsEvents.MusicSliderChanged?.Invoke(newValue);
        }
        
        public void FixedJoystickToggleHandler(bool isOn)
        {
            if (isOn)
            {
                SettingsEvents.JoystickTypeButtonChanged(JoystickType.FIXED);
            }
        }
        
        public void FloatingJoystickToggleHandler(bool isOn)
        {
            if (isOn)
            {
                SettingsEvents.JoystickTypeButtonChanged(JoystickType.FLOATING);
            }
        }
        
        public void DynamicJoystickToggleHandler(bool isOn)
        {
            if (isOn)
            {
                SettingsEvents.JoystickTypeButtonChanged(JoystickType.DYNAMIC);
            }
        }
    }
}