namespace HungryWorm
{
    public class PauseScreen : UIScreen
    {
        
        
        
        
        
        public void OnResumeButtonClicked()
        {
            UIEvents.ScreenClosed?.Invoke();
            GameEvents.GameUnpaused?.Invoke();
        }
        
        public void OnOptionsButtonClicked()
        {
            UIEvents.SettingsShown?.Invoke();
        }
        
        public void OnQuitButtonClicked()
        {
            UIEvents.MainMenuShown?.Invoke();
            GameEvents.GameClosed?.Invoke();
            GameEvents.GameEnded?.Invoke();
        }
        
    }
}