namespace HungryWorm
{
    public class PauseScreen : UIScreen
    {
        
        
        
        
        
        public void OnResumeButtonClicked()
        {
            UIEvents.ScreenClosed?.Invoke();
        }
        
        public void OnOptionsButtonClicked()
        {
            UIEvents.GameSettingsScreenShown?.Invoke();
        }
        
        public void OnQuitButtonClicked()
        {
            UIEvents.MainMenuShown?.Invoke();
        }
        
    }
}