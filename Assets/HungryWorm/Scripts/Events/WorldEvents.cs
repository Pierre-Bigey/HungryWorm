using System;

namespace HungryWorm
{
    public static class WorldEvents
    {
        public static Action<float> LeftEdgeUpdated;
        public static Action<float> RightEdgeUpdated;

        public static Action<Direction> MoveSlice;
    }
}