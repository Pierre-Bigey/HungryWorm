using System;
using UnityEngine;
using UnityEngine.UI;

namespace HungryWorm
{
    public class SplashScreen : UIScreen
    {
        [SerializeField] private Slider m_ProgressBar;
        
        public override void Initialize()
        {
            SceneEvents.LoadProgressUpdated += SceneEvents_LoadProgressUpdated;
        }
        
        private void OnDisable()
        {
            SceneEvents.LoadProgressUpdated -= SceneEvents_LoadProgressUpdated;
        }
        
        private void SceneEvents_LoadProgressUpdated(float progress)
        {
            //update the progress bar given the percentage in the action
            m_ProgressBar.value = progress/100;
        }
    }
}