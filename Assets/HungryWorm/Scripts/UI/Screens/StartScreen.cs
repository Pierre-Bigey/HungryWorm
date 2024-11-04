namespace HungryWorm
{
    public class StartScreen : UIScreen
    {


        public void StartButtonPressed()
        {
            Clicked();
            UIEvents.MainMenuShown?.Invoke();
        }
    }
}