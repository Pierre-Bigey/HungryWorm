
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
        
        private TimeSpan timePlaying;

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
            UIEvents.TimerUpdated += UIEvents_TimerUpdated;
            UIEvents.HealthBarUpdated += UIEvents_HealthBarUpdated;
            UIEvents.ScoreUpdated += UIEvents_ScoreUpdated;
        }
        
        private void UnsubscribeEvents()
        {
            UIEvents.TimerUpdated -= UIEvents_TimerUpdated;
            UIEvents.HealthBarUpdated -= UIEvents_HealthBarUpdated;
            UIEvents.ScoreUpdated -= UIEvents_ScoreUpdated;
        }
        
        public void OnPauseButtonPressed()
        {
            Clicked();
            UIEvents.PauseScreenShown?.Invoke();
            GameEvents.GamePaused?.Invoke();
        }


        
        private void UIEvents_TimerUpdated(float time)
        {
            timePlaying = TimeSpan.FromSeconds(time);
            string timePlaying_Str = timePlaying.ToString("mm':'ss");
            m_timerText.text = timePlaying_Str;
        }
        
        private void UIEvents_HealthBarUpdated(float value)
        {
            m_healthBar.value = value;
        }
        
        private void UIEvents_ScoreUpdated(float score)
        {
            m_scoreText.text = score.ToString("F0");
        }
        
    }
}