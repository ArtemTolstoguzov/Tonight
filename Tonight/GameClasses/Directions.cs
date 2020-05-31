using System;
using SFML.System;

namespace Tonight
{
    public enum DirectionsEnum
    {
        Up,
        Down,
        Left,
        Right,
        UpRight,
        UpLeft,
        DownRight,
        DownLeft
    }
    public static class Directions
    {
        private const int DELTA = 1;
        private const float epsilon = 5;
        private const float SQRT2 = (float) 1.41421356;
        public static Vector2f Up = new Vector2f(0, -DELTA);
        public static Vector2f Down = new Vector2f(0, DELTA);
        public static Vector2f Left = new Vector2f(-DELTA, 0);
        public static Vector2f Right = new Vector2f(DELTA, 0);
        public static Vector2f UpRight = new Vector2f(DELTA, -DELTA) / SQRT2;
        public static Vector2f UpLeft = new Vector2f(-DELTA, -DELTA) / SQRT2;
        public static Vector2f DownRight = new Vector2f(DELTA, DELTA) / SQRT2;
        public static Vector2f DownLeft = new Vector2f(-DELTA, DELTA) / SQRT2;
        public static Vector2i UpInt = new Vector2i(0, -DELTA);
        public static Vector2i DownInt = new Vector2i(0, DELTA);
        public static Vector2i LeftInt = new Vector2i(-DELTA, 0);
        public static Vector2i RightInt = new Vector2i(DELTA, 0);
        public static Vector2i UpRightInt = new Vector2i(DELTA, -DELTA);
        public static Vector2i UpLeftInt = new Vector2i(-DELTA, -DELTA);
        public static Vector2i DownRightInt = new Vector2i(DELTA, DELTA);
        public static Vector2i DownLeftInt = new Vector2i(-DELTA, DELTA);

        public static Vector2i GetIntDirection(DirectionsEnum dir)
        {
            switch (dir)
            {
                case DirectionsEnum.Up:
                    return UpInt;
                case DirectionsEnum.Down:
                    return DownInt;
                case DirectionsEnum.Left:
                    return LeftInt;
                case DirectionsEnum.Right:
                    return RightInt;
                case DirectionsEnum.DownLeft:
                    return DownLeftInt;
                case DirectionsEnum.DownRight:
                    return DownRightInt;
                case DirectionsEnum.UpLeft:
                    return UpLeftInt;
                case DirectionsEnum.UpRight:
                    return UpRightInt;
            }

            return UpInt;
        }
        public static Vector2f Normalize(Vector2f vector)
        {
            var length = Length(vector);
            return vector / length;
        }

        public static float Length(Vector2f vector)
        {
            return (float) Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static bool IsVectorsEqual(Vector2f first, Vector2f second)
        {
            return Math.Abs(first.X - second.X) < epsilon && Math.Abs(first.Y - second.Y) < epsilon;
        }
    }
}