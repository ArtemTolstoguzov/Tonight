using System;
using System.Runtime.CompilerServices;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace Tonight
{
    public class Sight : Drawable
    {
        private ConvexShape triangle;
        private Vector2f position;
        public Vector2f Position
        {
            get => triangle.Position;
            set => triangle.Position = value;
        }
        public Sight(Window2D window2D, float radius, float angle)
        {
            angle /= 2;
            triangle = new ConvexShape(3);
            triangle.SetPoint(0, new Vector2f(0, 0));
            triangle.SetPoint(1, new Vector2f(radius * (float) Math.Cos(angle), radius * (float) Math.Sin(angle)));
            triangle.SetPoint(2, new Vector2f(0, 2 * radius * (float)Math.Sin(angle)));
            triangle.Origin = triangle.GetPoint(1) + new Vector2f(3, 0);
            triangle.Position = (Vector2f)Mouse.GetPosition();
            window2D.MouseMoved += Move;
        }

        private void Move(object sender, MouseMoveEventArgs e)
        {
            triangle.Position = new Vector2f(e.X, e.Y);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            for (uint i = 0; i < 4; i++)
            {
                target.Draw(triangle);
                triangle.Rotation += 90;
            }
        }
    }
}