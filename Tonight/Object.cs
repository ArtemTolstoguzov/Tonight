using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using TiledSharp;

namespace Tonight
{
    public enum ObjectType : byte
    {
        Rectangle = 0,
        Ellipse = 1
    }

    public class Object
    {
        private readonly View view;
        public string Name;
        public string Type;
        public ObjectType ObjectType;
        public Vector2f Position;
        public Vector2f Size;
        public Shape Shape;
        public FloatRect Rect;

        public Object(View view, TmxObject obj)
        {
            this.view = view;
            Name = obj.Name;
            Type = obj.Type;
            Position = new Vector2f((float)obj.X, (float)obj.Y);
            Size = new Vector2f((float)obj.Width, (float)obj.Height);
            Rect = new FloatRect(Position, Size);

            switch (obj.ObjectType)
            {
                case TmxObjectType.Basic:
                    ObjectType = ObjectType.Rectangle;
                    Shape = new RectangleShape(Size);
                    Shape.Position = Position;
                    break;
                case TmxObjectType.Ellipse:
                    ObjectType = ObjectType.Ellipse;
                    Shape = new CircleShape((float)obj.Width / 2);
                    Shape.Position = Position;
                    break;
            }
        }
    }
}
