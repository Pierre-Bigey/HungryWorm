using UnityEngine;

namespace HungryWorm
{
    public class MainMenuScreen : UIScreen
    {
        public override void Initialize()
        {
            SceneEvents.UnloadLastScene?.Invoke();
            
            //reset the camera position
            Camera.main.transform.position = new Vector3(0, 0, -10);
        }


        public void SettingsButtonClicked()
        {
            Clicked();
            UIEvents.SettingsShown?.Invoke();
        }

        public void PlayButtonClicked()
        {
            Clicked();
            //UIEvents.GameScreenShown?.Invoke();
            SceneEvents.StartGame?.Invoke();
        }

        public void QuitButtonClicked()
        {
            Clicked();
            SceneEvents.ExitApplication?.Invoke();
        }
    }

}