using System.Runtime.CompilerServices;
using System.Security;
using SFML.Graphics;
using SFML.System;

namespace Tonight
{
    public class Camera : View
    {
        public Camera(float width, float height) : base(new View(new FloatRect(0, 0, width, height)))
        {
        }
        
        public void Move(Vector2f center, Map map)
        {
            var moveVector = center;
            if (2 * center.X <= Size.X)
                moveVector.X = Size.X / 2;
            if (2 * center.X >= 2 * map.Width - Size.X)
                moveVector.X = map.Width - Size.X / 2;

            if (2 * center.Y <= Size.Y)
                moveVector.Y = Size.Y / 2;
            if (2 * center.Y >= 2 * map.Height - Size.Y)
                moveVector.Y = map.Height - Size.Y / 2;

            Center = moveVector;
        }
        
    }
}