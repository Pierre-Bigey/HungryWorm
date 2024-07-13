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
            LoadGameScene();
        }

        public void QuitButtonClicked()
        {
            SceneEvents.ExitApplication?.Invoke();
        }

        private void LoadGameScene()
        {
            SceneEvents.LoadSceneByIndex?.Invoke(1);
        }
    }

}