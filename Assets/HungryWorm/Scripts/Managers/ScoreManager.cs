using System;
using UnityEngine;

namespace HungryWorm
{
    public class ScoreManager: MonoBehaviour
    {
        
        private float m_score;

        private void Start()
        {
            m_score = 0;
        }


        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            GameEvents.ScoreUpdated += GameEvents_OnScoreUpdated;
            GameEvents.GameStarted += GameEvents_OnGameStarted;
        }
        
        private void UnsubscribeEvents()
        {
            GameEvents.ScoreUpdated -= GameEvents_OnScoreUpdated;
            GameEvents.GameStarted -= GameEvents_OnGameStarted;
        }

        private void GameEvents_OnScoreUpdated(float _score)
        {
            Debug.Log("Score updated: " + m_score);
            m_score += _score;
            UIEvents.ScoreUpdated?.Invoke(m_score);
        }
        
        private void GameEvents_OnGameStarted()
        {
            m_score = 0;
            UIEvents.ScoreUpdated?.Invoke(m_score);
        }
        


    }
}