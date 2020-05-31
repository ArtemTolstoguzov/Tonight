using System;
using SFML.Graphics;
using SFML.System;

namespace Tonight
{
    public class Segment: Drawable
    {
        private RectangleShape rect;
        public Segment(Vector2f start, Vector2f end)
        {
            var dir = end - start;
            rect = new RectangleShape(new Vector2f(Directions.Length(dir), 1));
            rect.Position = start;
            var angleRad = (float)Math.Atan2(dir.Y, dir.X);
            var angleGrad = angleRad * 180 / (float)Math.PI;
            rect.Rotation = angleGrad;
        }

        public FloatRect GetGlobalBounds()
        {
            return rect.GetGlobalBounds();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(rect);
        }
    }
}