namespace HungryWorm
{
    public class PauseScreen : UIScreen
    {
        
        
        
        
        
        public void OnResumeButtonClicked()
        {
            Clicked();
            UIEvents.ScreenClosed?.Invoke();
            GameEvents.GameUnpaused?.Invoke();
        }
        
        public void OnOptionsButtonClicked()
        {
            Clicked();
            UIEvents.SettingsShown?.Invoke();
        }
        
        public void OnQuitButtonClicked()
        {
            Clicked();
            UIEvents.MainMenuShown?.Invoke();
            GameEvents.GameClosed?.Invoke();
            GameEvents.GameEnded?.Invoke();
        }
        
    }
}