namespace HungryWorm
{
    public class EndScreen : UIScreen
    {

        public void OnNextClicked()
        {
            UIEvents.LeaderboardScreenShown?.Invoke();
            GameEvents.GameClosed?.Invoke();
        }
        
    }
}