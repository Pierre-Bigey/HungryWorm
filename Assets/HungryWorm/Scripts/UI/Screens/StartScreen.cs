namespace HungryWorm
{
    public class StartScreen : UIScreen
    {


        public void StartButtonPressed()
        {
            UIEvents.MainMenuShown?.Invoke();
        }
    }
}