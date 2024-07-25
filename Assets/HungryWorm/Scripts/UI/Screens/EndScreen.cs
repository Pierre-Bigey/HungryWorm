using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace HungryWorm
{
    public class EndScreen : UIScreen
    {

        [SerializeField] private TMP_Text m_ScoreText;
        [SerializeField] private TMP_Text m_MinesExplodedText;
        [SerializeField] private TMP_Text m_HumansEatenText;

        private new void OnEnable()
        {
            base.OnEnable();
            SetScores();
        }

        private void SetScores()
        {
            m_ScoreText.text = ScoreManager.Instance.Score.ToString();
            m_MinesExplodedText.text = ScoreManager.Instance.MinesExploded.ToString();
            m_HumansEatenText.text = ScoreManager.Instance.HumansEaten.ToString();
        }
        

        public void OnNextClicked()
        {
            UIEvents.LeaderboardScreenShown?.Invoke();
            GameEvents.GameClosed?.Invoke();
        }
        
    }
}