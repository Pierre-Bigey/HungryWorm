
namespace HungryWorm
{
    public class PlayerPrefManager
    {
        private static string _highScoreKey = "HighScore";
        private static string _soundVolumeKey = "SoundVolume";
        private static string _musicVolumeKey = "MusicVolume";
        
        private const float DefaultMasterVolume = 0.8f;
        private const float DefaultSoundVolume = 0.7f;
        private const float DefaultMusicVolume = 0.6f;
        
        
        public void SetHighScore(int score)
        {
            UnityEngine.PlayerPrefs.SetInt(_highScoreKey, score);
        }
        
        public int GetHighScore()
        {
            return UnityEngine.PlayerPrefs.GetInt(_highScoreKey, 0);
        }
        
        public void SetMasterVolume(float volume)
        {
            UnityEngine.PlayerPrefs.SetFloat("MasterVolume", volume);
        }
        
        public float GetMasterVolume()
        {
            return UnityEngine.PlayerPrefs.GetFloat("MasterVolume", DefaultMasterVolume);
        }
        
        public void SetSoundVolume(float volume)
        {
            UnityEngine.PlayerPrefs.SetFloat(_soundVolumeKey, volume);
        }
        
        public float GetSoundVolume()
        {
            return UnityEngine.PlayerPrefs.GetFloat(_soundVolumeKey, DefaultSoundVolume);
        }
        
        public void SetMusicVolume(float volume)
        {
            UnityEngine.PlayerPrefs.SetFloat(_musicVolumeKey, volume);
        }

        public float GetMusicVolume()
        {
            return UnityEngine.PlayerPrefs.GetFloat(_musicVolumeKey, DefaultMusicVolume);
        }
        
        public void SetJoystickType(JoystickType joystickType)
        {
            UnityEngine.PlayerPrefs.SetInt("JoystickType", (int)joystickType);
        }
        
        public JoystickType GetJoystickType()
        {
            return (JoystickType)UnityEngine.PlayerPrefs.GetInt("JoystickType", 0);
        }
        
        public void ResetAll()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
            SetMasterVolume(DefaultMasterVolume);
            SetSoundVolume(DefaultSoundVolume);
            SetMusicVolume(DefaultMusicVolume);
            SaveAll();
        }

        public void SaveAll()
        {
            UnityEngine.PlayerPrefs.Save();
        }

    }
}