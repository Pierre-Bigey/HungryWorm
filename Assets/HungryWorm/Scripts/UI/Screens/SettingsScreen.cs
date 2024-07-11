using System;
using UnityEngine.UIElements;

namespace HungryWorm
{
    public class SettingsScreen : UIScreen
    {
        Slider m_MasterVolumeSlider;
        Slider m_SFXVolumeSlider;
        Slider m_MusicVolumeSlider;
        
        Label m_MasterVolumeLabel;
        Label m_SFXVolumeLabel;
        Label m_MusicVolumeLabel;


        private void OnEnable()
        {
            SubscribeToEvents();
            
            m_IsTransparent = true;
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
        
        private void JoystickTypeButtonSetHandler(JoystickType obj)
        {
            throw new System.NotImplementedException();
        }

        public void BackButtonClicked()
        {
            SettingsEvents.SaveAll?.Invoke();
            UIEvents.ScreenClosed?.Invoke();
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
        
        public void JoystickTypeChangeHandler(int joystickType)
        {

        }
    }
}