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
        
        
        public static Action<float> ScoreUpdated;
        
        public static Action<float> TimeUpdated;
        
        #region Gameplay events:
        // The worm has eaten a piece of food
        public static Action<float> FoodEaten;
        
        public static Action<float> DamageTaken;
        
        #endregion
        
    }
}