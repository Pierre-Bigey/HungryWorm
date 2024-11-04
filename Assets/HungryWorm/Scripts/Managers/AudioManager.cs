using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace HungryWorm
{
    public class AudioManager : MonoBehaviour
    {
        
        [Tooltip("AudioMixer for audio")]
        [SerializeField] private AudioMixer m_AudioMixer;
        [SerializeField] private AudioSourceManager m_AudioSourceManager;
        
        // AudioMixer group names
        const string k_SFXVolume = "SFX";
        const string k_MusicVolume = "Music";
        const string k_MasterVolume = "Master";
        
        [Tooltip("AudioSource dedicated to playing music")]
        [SerializeField] [Optional] AudioSource m_MusicAudioSource;
        
        [Header("Audio Clips")]
        [SerializeField] private AudioClip m_buttonClickClip;
        [SerializeField] private AudioClip m_explosionClip;
        [SerializeField] private AudioClip m_bloodSplatterClip;
        
        
        
        private void OnEnable()
        {
            Initialize();
            SubscribeEvents();
        }
        
        // Event unsubscriptions
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void Initialize()
        {
            NullRefChecker.Validate(this);
        }
        
        private void SubscribeEvents()
        {
            SettingsEvents.MasterVolumeChanged += SettingsEvents_OnMasterVolumeChanged;
            SettingsEvents.SFXVolumeChanged += SettingsEvents_OnSFXVolumeChanged;
            SettingsEvents.MusicVolumeChanged += SettingsEvents_OnMusicVolumeChanged;
            
            UIEvents.ButtonClicked += PlayButtonSound;
            
            WormEvents.MineExploded += WormEvents_MineExploded;
            WormEvents.BloodSplatter += WormEvents_BloodSplatter;
        }
        
        private void UnsubscribeEvents()
        {
            SettingsEvents.MasterVolumeChanged -= SettingsEvents_OnMasterVolumeChanged;
            SettingsEvents.SFXVolumeChanged -= SettingsEvents_OnSFXVolumeChanged;
            SettingsEvents.MusicVolumeChanged -= SettingsEvents_OnMusicVolumeChanged;
            
            UIEvents.ButtonClicked -= PlayButtonSound;
            
            WormEvents.MineExploded -= WormEvents_MineExploded;
            WormEvents.BloodSplatter -= WormEvents_BloodSplatter;
        }
        
        #region Event-handling methods (responds to volume change events, raised by the SettingsPresenter)
        
        private void SettingsEvents_OnMasterVolumeChanged(float volume)
        {
            float decibelVolume = ConvertLinearToDecibel(volume);
            m_AudioMixer.SetFloat(k_MasterVolume, decibelVolume);
        }

        private void SettingsEvents_OnMusicVolumeChanged(float volume)
        {
            float decibelVolume = ConvertLinearToDecibel(volume);
            m_AudioMixer.SetFloat(k_MusicVolume, decibelVolume);
        }

        private void SettingsEvents_OnSFXVolumeChanged(float volume)
        {
            float decibelVolume = ConvertLinearToDecibel(volume);
            m_AudioMixer.SetFloat(k_SFXVolume, decibelVolume);
        }
        
        #endregion

        private void PlayButtonSound()
        {
            m_AudioSourceManager.PlayClip(m_buttonClickClip);
        }
        
        private void WormEvents_MineExploded(Vector3 position)
        {
            m_AudioSourceManager.PlayClip(m_explosionClip);
        }
        
        private void WormEvents_BloodSplatter(Vector3 position)
        {
            m_AudioSourceManager.PlayClip(m_bloodSplatterClip);
        }
        
        #region Methods
        
        // Play music with the specified AudioClip (unused in this demo)
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            m_MusicAudioSource.clip = clip;
            m_MusicAudioSource.loop = loop;
            m_MusicAudioSource.Play();
        }

        // Convert from the logarithmic AudioMixer scale (-80dB to 0dB) to linear UI scale (0 to 1) and vice versa
        public static float ConvertLinearToDecibel(float linearVolume)
        {
            return Mathf.Log10(Mathf.Max(0.0001f, linearVolume)) * 20.0f;
        }

        public static float ConvertDecibelToLinear(float decibelVolume)
        {
            return Mathf.Pow(10, decibelVolume / 20.0f);
        }
        #endregion
    }
}