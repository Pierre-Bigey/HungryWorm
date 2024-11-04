namespace HungryWorm
{
    public class LeaderboardScreen : UIScreen
    {
        
        public void OnRestartClicked()
        {
            Clicked();
            SceneEvents.StartGame?.Invoke();
        }
        
        public void OnMainMenuClicked()
        {
            Clicked();
            UIEvents.MainMenuShown?.Invoke();
        }
    }
}