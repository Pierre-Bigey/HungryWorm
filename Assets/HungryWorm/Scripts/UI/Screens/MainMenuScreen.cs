namespace HungryWorm
{
    public class MainMenuScreen : UIScreen
    {
        
        
        
        
        public void SettingsButtonClicked()
        {
            UIEvents.SettingsShown?.Invoke();
        }

        public void PlayButtonClicked()
        {
            UIEvents.GameScreenShown?.Invoke();
        }

        public void QuitButtonClicked()
        {
            SceneEvents.ExitApplication?.Invoke();
        }

    }
}