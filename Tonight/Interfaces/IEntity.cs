using SFML.Graphics;

namespace Tonight
{
    public interface IEntity
    {
       bool IsAlive { get; set; }
       FloatRect GetSpriteRectangleWithoutRotation();
    }
}