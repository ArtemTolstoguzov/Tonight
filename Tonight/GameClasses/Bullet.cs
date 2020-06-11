using System;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;

namespace Tonight
{
    public class Bullet:Sprite, IUpdatable, IEntity
    {
        public bool IsAlive { get; set; }
        private IEntity Owner;
        public FloatRect GetSpriteRectangleWithoutRotation()
        {
            var tempRotation = Rotation;
            Rotation = 0;
            var rect = GetGlobalBounds();
            Rotation = tempRotation;
            return rect;
        }

        private readonly Vector2f direction;
        private float speed = 1000;
        private static readonly Image BulletImage = new Image("images/bullet.png");

        private readonly Level level;
        public Bullet(Vector2f heroPosition, Vector2f sightPosition, Level level, IEntity owner)
        {
            Owner = owner;
            this.level = level;
            BulletImage.CreateMaskFromColor(Color.Black);
            Texture = new Texture(BulletImage);
            TextureRect = new IntRect(0, 0, 21, 10); //16 16
            direction = Normalize(sightPosition - heroPosition);
            Position = heroPosition;
            IsAlive = true;
            if (owner == level.Player)
            {
                NotifyEnemiesAboutShooting(Position);
            }
            Rotation = (float) (Math.Atan2(direction.Y, direction.X) * 180 / Math.PI);
        }

        private void NotifyEnemiesAboutShooting(Vector2f position)
        {
            var size = 1000;
            var hearZone = new RectangleShape(new Vector2f(size, size));
            hearZone.Origin = new Vector2f(size / 2, size / 2);
            hearZone.Position = position;
            var rect = hearZone.GetGlobalBounds();
            foreach (var enemy in level.Enemies)
            {
                if(rect.Intersects(enemy.GetGlobalBounds()))
                    enemy.NotifyAboutShooting(position);
            }
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
            var objects = level.Map.CollisionObjects;
            
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
            foreach (var entity in level.GetEntities())
            {
                if (entity != Owner  && entity.GetSpriteRectangleWithoutRotation().Intersects(collisionRect))
                {
                    entity.IsAlive = false;
                    level.Enemies = level.Enemies.Where(e => e.IsAlive).ToList();
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