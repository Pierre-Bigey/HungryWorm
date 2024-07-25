using System;
using UnityEngine;

namespace HungryWorm
{
    public class ScoreManager: MonoBehaviour
    {
        //Singleton pattern
        public static ScoreManager Instance { get; private set; }
        
        public float Score => m_score;
        public int MinesExploded => m_MinesExploded;
        public int HumansEaten => m_HumansEaten;
        
        private float m_score;
        private int m_MinesExploded;
        private int m_HumansEaten;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

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
            
            WormEvents.MineExploded += WormEvents_OnMineExploded;
            WormEvents.EnemyEaten += WormEvents_OnEnemyEaten;
        }
        
        private void UnsubscribeEvents()
        {
            GameEvents.ScoreUpdated -= GameEvents_OnScoreUpdated;
            GameEvents.GameStarted -= GameEvents_OnGameStarted;
            
            WormEvents.MineExploded -= WormEvents_OnMineExploded;
            WormEvents.EnemyEaten -= WormEvents_OnEnemyEaten;
        }

        private void GameEvents_OnScoreUpdated(float _score)
        {
            m_score += _score;
            UIEvents.ScoreUpdated?.Invoke(m_score);
        }
        
        private void GameEvents_OnGameStarted()
        {
            m_score = 0;
            UIEvents.ScoreUpdated?.Invoke(m_score);
        }
        
        private void WormEvents_OnMineExploded(Vector3 _)
        {
            m_MinesExploded++;
        }
        
        private void WormEvents_OnEnemyEaten(float _)
        {
            m_HumansEaten++;
        }


    }
}