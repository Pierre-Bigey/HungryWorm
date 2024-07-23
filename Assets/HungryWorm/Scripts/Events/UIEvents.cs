using System;

namespace HungryWorm
{
    /// <summary>
    /// Public static delegates to manage UI changes (note these are "events" in the conceptual sense
    /// and not the strict C# sense).
    /// </summary>
    public static class UIEvents
    {
        #region Menu/screen events:

        // Close the screen and go back
        public static Action ScreenClosed;

        public static Action SplashScreenShown;

        // Show the Main Menu selection (Settings, ...)
        public static Action MainMenuShown;

        // Show the user settings (sound volume, language, etc.)
        public static Action SettingsShown;

        // Show the main gameplay screen UI
        public static Action GameScreenShown;

        // Show a pause screen during gameplay to abort the game
        public static Action PauseScreenShown;
        
        //Show the gameSettings screen
        public static Action GameSettingsScreenShown;

        // Show the results of the game
        public static Action EndScreenShown;
        
        // Show the leaderboard screen
        public static Action LeaderboardScreenShown;

        #endregion
        
        public static Action ButtonClicked;

        
        #region GamePLay events:

        // Draw the health bar
        public static Action<float> HealthBarUpdated;
        
        // Call the score
        public static Action<float> ScoreUpdated;
        
        public static Action<float> TimerUpdated;
        
        #endregion
        

        #region End screen events:
        
        // Show score
        public static Action<string> ScoreMessageShown;

        // Show text description of the main menu button
        public static Action<string> MenuDescriptionShown;

        public static Action<string> UrlOpened;

        #endregion

    }
}
