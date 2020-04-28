using SFML.System;

namespace Tonight
{
    public static class Directions
    {
        private const float DELTA = 1;
        private const float SQRT2 = (float) 1.41421356;
        public static Vector2f Up = new Vector2f(0, -DELTA);
        public static Vector2f Down = new Vector2f(0, DELTA);
        public static Vector2f Left = new Vector2f(-DELTA, 0);
        public static Vector2f Right = new Vector2f(DELTA, 0);
        public static Vector2f UpRight = new Vector2f(DELTA, -DELTA) / SQRT2;
        public static Vector2f UpLeft = new Vector2f(-DELTA, -DELTA) / SQRT2;
        public static Vector2f DownRight = new Vector2f(DELTA, DELTA) / SQRT2;
        public static Vector2f DownLeft = new Vector2f(-DELTA, DELTA) / SQRT2;
    }
}