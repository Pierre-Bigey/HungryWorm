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
            UIEvents.SettingsShown?.Invoke();
        }

        public void PlayButtonClicked()
        {
            //UIEvents.GameScreenShown?.Invoke();
            SceneEvents.StartGame?.Invoke();
        }

        public void QuitButtonClicked()
        {
            SceneEvents.ExitApplication?.Invoke();
        }
    }

}