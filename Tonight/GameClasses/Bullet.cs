using System;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace Tonight
{
    public class Bullet:Sprite, IUpdatable
    {
        public bool IsAlive { get; private set; }
        private Vector2f direction;
        private float speed = 700;
        private static Image bulletImage = new Image("images/bullet.png");
        private Map map;
        public Bullet(Vector2f heroPosition, Vector2f sightPosition, Map map)
        {
            this.map = map;
            bulletImage.CreateMaskFromColor(Color.Black);
            Texture = new Texture(bulletImage);
            TextureRect = new IntRect(0, 0, 16, 16);
            direction = Normalize(sightPosition - heroPosition);
            Position = heroPosition;
            IsAlive = true;
        }

        public static Vector2f Normalize(Vector2f vector)
        {
            var length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            return new Vector2f(vector.X / length, vector.Y / length);
        }

        public void Move(GameTime gameTime)
        {
            var moveVector = speed * direction * gameTime.DeltaTime;
            Position += moveVector;
            if (CheckCollisions(moveVector))
            {
                IsAlive = false;
            }
        }
        
        public bool CheckCollisions(Vector2f delta)
        {
            var objects = map.mapObjects["collision"];
            
            var collisionRect = GetGlobalBounds();
            collisionRect.Left += delta.X;
            collisionRect.Top += delta.Y;
            foreach (var obj in objects)
            {
                if (obj.Rect.Intersects(collisionRect))
                {
                    return true;
                }
            }
            foreach (var enemy in map.Enemies)
            {
                if (enemy.GetGlobalBounds().Intersects(collisionRect))
                {
                    enemy.IsAlive = false;
                    map.Enemies = map.Enemies.Where(e => e.IsAlive).ToList();
                    return true;
                }
            }
            return false;
        }
        
        public void Update(GameTime gameTime)
        {
            Move(gameTime);
        }
    }
}