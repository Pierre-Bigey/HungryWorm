using System;
using UnityEngine;

namespace HungryWorm
{
    public static class WormEvents
    {

        public static Action DirtEnter;
        
        public static Action DirtExit;
        
        
        // The worm has eaten a piece of food
        public static Action<float> EnemyEaten;
        
        public static Action<float> DamageTaken;

        public static Action<Vector2> WormGoToDirection;

        
        public static Action WormDied;

    }
}