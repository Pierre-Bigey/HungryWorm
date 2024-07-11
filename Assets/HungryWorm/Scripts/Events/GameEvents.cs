using System;

namespace HungryWorm
{
    public static class GameEvents
    {
        #region Game state change events:

        // Start the game
        public static Action GameStarted;

        // Pause the game during gameplay
        public static Action GamePaused;

        // Return to gameplay from the pause screen
        public static Action GameUnpaused;

        // Quit the game while on the pause screen
        public static Action GameAborted;

        // The worm is dead
        public static Action GameEnded;

        
        #endregion
    }
}