using System;
using System.Runtime.CompilerServices;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace Tonight
{
    public class Sight : Drawable, IUpdatable
    {
        private ConvexShape triangle;
        private Vector2f position;
        private Window2D window;
        public Vector2f Position
        {
            get => triangle.Position;
            set => triangle.Position = value;
        }

        public Sight(Window2D window2D, float radius = 20, float angle = (float) Math.PI / 6)
        {
            angle /= 2;
            triangle = new ConvexShape(3);
            triangle.SetPoint(0, new Vector2f(0, 0));
            triangle.SetPoint(1, new Vector2f(radius * (float) Math.Cos(angle), radius * (float) Math.Sin(angle)));
            triangle.SetPoint(2, new Vector2f(0, 2 * radius * (float)Math.Sin(angle)));
            triangle.Origin = triangle.GetPoint(1) + new Vector2f(3, 0);
            //triangle.Position = new Vector2f(250, 150);
            window = window2D;
        }

        public void Move(Vector2f vector)
        {
            Position = (Vector2f) Mouse.GetPosition(window);
            Position += vector;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            for (uint i = 0; i < 4; i++)
            {
                target.Draw(triangle);
                triangle.Rotation += 90;
            }
        }

        public void Update(GameTime gameTime)
        {
            var cameraCenter = window.GetView().Center;
            Move(cameraCenter - window.GetView().Size / 2);
        }
    }
}