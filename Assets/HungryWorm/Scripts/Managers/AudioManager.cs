using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace HungryWorm
{
    public class AudioManager : MonoBehaviour
    {
        
        [Tooltip("AudioMixer for audio")]
        [SerializeField] AudioMixer m_AudioMixer;
        
        // AudioMixer group names
        const string k_SFXVolume = "SFXVolume";
        const string k_MusicVolume = "MusicVolume";
        const string k_MasterVolume = "MasterVolume";
        
        [Tooltip("AudioSource dedicated to playing sound effects")]
        [SerializeField] AudioSource m_SFXAudioSource;

        [Tooltip("AudioSource dedicated to playing music")]
        [SerializeField] [Optional] AudioSource m_MusicAudioSource;
        
        
        
        
        private void OnEnable()
        {
            SettingsEvents.MasterVolumeChanged += SettingsEvents_OnMasterVolumeChanged;
            SettingsEvents.SFXVolumeChanged += SettingsEvents_OnSFXVolumeChanged;
            SettingsEvents.MusicVolumeChanged += SettingsEvents_OnMusicVolumeChanged;

            Initialize();
        }
        
        // Event unsubscriptions
        private void OnDisable()
        {
            SettingsEvents.MasterVolumeChanged -= SettingsEvents_OnMasterVolumeChanged;
            SettingsEvents.SFXVolumeChanged -= SettingsEvents_OnSFXVolumeChanged;
            SettingsEvents.MusicVolumeChanged -= SettingsEvents_OnMusicVolumeChanged;
        }

        private void Initialize()
        {
            NullRefChecker.Validate(this);
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
        
        #region Methods

        // Play a sound effect at the specified position.
        public void PlaySFXAtPoint(AudioClip clip, Vector3 position, float delay = 0f, bool loop = false)
        {
            m_SFXAudioSource.Stop();
            StartCoroutine(PlaySFXAtPointDelayed(clip, position, delay, loop));
        }

        // Coroutine to play a sound effect at the specified position with a delay.
        private IEnumerator PlaySFXAtPointDelayed(AudioClip clip, Vector3 position, float delay, bool loop)
        {
            yield return new WaitForSeconds(delay);

            m_SFXAudioSource.transform.position = position;
            m_SFXAudioSource.loop = loop;

            if (clip != null)
                m_SFXAudioSource.PlayOneShot(clip);
            else
                m_SFXAudioSource.Stop();
        }

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