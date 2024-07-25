using System;
using UnityEngine.Rendering.Universal;

namespace HungryWorm
{
    public static class SettingsEvents
    {
        // Presenter --> View: update UI 
        public static Action<float> MasterSliderSet;
        public static Action<float> SFXSliderSet;
        public static Action<float> MusicSliderSet;
        public static Action<JoystickType> JoystickTypeButtonSet;

        // View -> Presenter: update UI sliders
        public static Action<float> MasterSliderChanged;
        public static Action<float> SFXSliderChanged;
        public static Action<float> MusicSliderChanged;
        public static Action<JoystickType> JoystickTypeButtonChanged;

        //// Presenter -> Model: update volume settings
        public static Action<float> MasterVolumeChanged;
        public static Action<float> SFXVolumeChanged;
        public static Action<float> MusicVolumeChanged;
        public static Action<JoystickType> JoystickTypeChanged;
        

        // Model -> Presenter: model values changed (e.g. loading saved values)
        public static Action<float> ModelMasterVolumeChanged;
        public static Action<float> ModelSFXVolumeChanged;
        public static Action<float> ModelMusicVolumeChanged;
        public static Action<JoystickType> ModelJoystickTypeChanged;

        public static Action SaveAll;
        public static Action ResetAll;

        public static Action SettingInitialized;
    }
}