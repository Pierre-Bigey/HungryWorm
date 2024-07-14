namespace HungryWorm
{
    public class LeaderboardScreen : UIScreen
    {
        
        public void OnRestartClicked()
        {
            SceneEvents.StartGame?.Invoke();
        }
        
        public void OnMainMenuClicked()
        {
            UIEvents.MainMenuShown?.Invoke();
        }
    }
}