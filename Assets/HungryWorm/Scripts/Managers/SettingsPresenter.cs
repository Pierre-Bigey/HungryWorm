using UnityEngine;

namespace HungryWorm
{
    public class SettingsPresenter : MonoBehaviour
    {

        private PlayerPrefManager _playerPrefManager;
        
        private void Start()
        {
            _playerPrefManager = new PlayerPrefManager();
            Initialize();
        }

        // Set defaults
        private void Initialize()
        {

            // Update sliders with the saved values from the PlayerPrefManager
            float masterVolume = _playerPrefManager.GetMasterVolume();
            float sfxVolume = _playerPrefManager.GetSoundVolume();
            float musicVolume = _playerPrefManager.GetMusicVolume();
            JoystickType joystickType = _playerPrefManager.GetJoystickType();
                
            // Notify the View of saved values from the Storage
            SettingsEvents.MasterSliderSet?.Invoke(masterVolume * 100f);
            SettingsEvents.SFXSliderSet?.Invoke(sfxVolume * 100f);
            SettingsEvents.MusicSliderSet?.Invoke(musicVolume * 100f);
            SettingsEvents.JoystickTypeButtonSet?.Invoke(joystickType);
            
            // Notify the AudioManager of saved values from the Storage
            SettingsEvents.MasterVolumeChanged?.Invoke(masterVolume);
            SettingsEvents.SFXVolumeChanged?.Invoke(sfxVolume);
            SettingsEvents.MusicVolumeChanged?.Invoke(musicVolume);
        }

        // Event subscriptions
        private void OnEnable()
        {
            SettingsEvents.SettingInitialized += Initialize;
            
            // Listen for events from the View/UI
            SettingsEvents.MasterSliderChanged += SettingsEvents_MasterSliderChanged;
            SettingsEvents.SFXSliderChanged += SettingsEvents_SFXSliderChanged;
            SettingsEvents.MusicSliderChanged += SettingsEvents_MusicSliderChanged;
            SettingsEvents.JoystickTypeButtonChanged += SettingsEvents_JoystickTypeButtonChanged;
            
            // Listen for events from the Model
            SettingsEvents.ModelMasterVolumeChanged += SettingsEvents_ModelMasterVolumeChanged;
            SettingsEvents.ModelSFXVolumeChanged += SettingsEvents_ModelSFXVolumeChanged;
            SettingsEvents.ModelMusicVolumeChanged += SettingsEvents_ModelMusicVolumeChanged;
            SettingsEvents.ModelJoystickTypeChanged += SettingsEvents_ModelJoystickTypeChanged;

            SettingsEvents.SaveAll += SettingsEvents_SaveAll;
            SettingsEvents.ResetAll += SettingsEvents_ResetAll;
        }
        
        // Event unsubscriptions
        private void OnDisable()
        {
            SettingsEvents.SettingInitialized -= Initialize;
            
            SettingsEvents.MasterSliderChanged -= SettingsEvents_MasterSliderChanged;
            SettingsEvents.SFXSliderChanged -= SettingsEvents_SFXSliderChanged;
            SettingsEvents.MusicSliderChanged -= SettingsEvents_MusicSliderChanged;
            SettingsEvents.JoystickTypeButtonChanged -= SettingsEvents_JoystickTypeButtonChanged;
            
            SettingsEvents.ModelMasterVolumeChanged -= SettingsEvents_ModelMasterVolumeChanged;
            SettingsEvents.ModelSFXVolumeChanged -= SettingsEvents_ModelSFXVolumeChanged;
            SettingsEvents.ModelMusicVolumeChanged -= SettingsEvents_ModelMusicVolumeChanged;
            SettingsEvents.ModelJoystickTypeChanged -= SettingsEvents_ModelJoystickTypeChanged;
            
            SettingsEvents.SaveAll -= SettingsEvents_SaveAll;
            SettingsEvents.ResetAll -= SettingsEvents_ResetAll;
        }
        
        private void SettingsEvents_MasterSliderChanged(float sliderValue)
        {
            float volume = sliderValue / 100f;
            SettingsEvents.MasterVolumeChanged?.Invoke(volume);
            _playerPrefManager.SetMasterVolume(volume);
        }

        private void SettingsEvents_SFXSliderChanged(float sliderValue)
        {
            float volume = sliderValue / 100f;
            SettingsEvents.SFXVolumeChanged?.Invoke(volume);
            _playerPrefManager.SetSoundVolume(volume);
        }
        
        private void SettingsEvents_MusicSliderChanged(float sliderValue)
        {
            float volume = sliderValue / 100f;
            SettingsEvents.MusicVolumeChanged?.Invoke(volume);
            _playerPrefManager.SetMusicVolume(volume);
        }
        
        private void SettingsEvents_JoystickTypeButtonChanged(JoystickType joystickType)
        {
            SettingsEvents.JoystickTypeChanged?.Invoke(joystickType);
            _playerPrefManager.SetJoystickType(joystickType);
        }
        
        
        private void SettingsEvents_ModelMasterVolumeChanged(float volume)
        {
            // Process the Master volume change from the Model
            SettingsEvents.MasterSliderSet?.Invoke(volume);
        }
        
        private void SettingsEvents_ModelSFXVolumeChanged(float volume)
        {
            // Process the SFX volume change from the Model
            SettingsEvents.SFXSliderSet?.Invoke(volume);
        }
        
        private void SettingsEvents_ModelMusicVolumeChanged(float volume)
        {
            // Process the Music volume change from the Model
            SettingsEvents.MusicSliderSet?.Invoke(volume);
        }
        
        private void SettingsEvents_ModelJoystickTypeChanged(JoystickType joystickType)
        {
            throw new System.NotImplementedException();
        }

        private void SettingsEvents_SaveAll()
        {
            _playerPrefManager.SaveAll();
        }

        private void SettingsEvents_ResetAll()
        {
            _playerPrefManager.ResetAll();
            Initialize();
        }

    }
}