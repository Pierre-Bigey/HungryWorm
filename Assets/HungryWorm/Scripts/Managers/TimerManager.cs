using System;
using UnityEngine;

namespace HungryWorm
{
    public class TimerManager: MonoBehaviour
    {

        private Timer m_timer;
        
        private void Awake()
        {
            m_timer = new Timer();
        }
        
        
        private void OnEnable()
        {
            GameEvents.GameStarted += GameEvents_GameStarted;
            GameEvents.GamePaused += GameEvents_GamePaused;
            GameEvents.GameUnpaused += GameEvents_GameUnpaused;
            GameEvents.GameEnded += GameEvents_GameEnded;
        }
        
        private void OnDisable()
        {
            GameEvents.GameStarted -= GameEvents_GameStarted;
            GameEvents.GamePaused -= GameEvents_GamePaused;
            GameEvents.GameUnpaused -= GameEvents_GameUnpaused;
            GameEvents.GameEnded -= GameEvents_GameEnded;
        }
        
        private void GameEvents_GameStarted()
        {
            Time.timeScale = 1;
            m_timer.BeginTimer();    
        }
        
        private void GameEvents_GamePaused()
        {
            Time.timeScale = 0;
        }
        
        private void GameEvents_GameUnpaused()
        {
            Time.timeScale = 1;
        }
        
        private void GameEvents_GameEnded()
        {
            Time.timeScale = 1;
            m_timer.StopTimer();
        }
        
        
    }
}