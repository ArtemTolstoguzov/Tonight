using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace Tonight
{
    public enum Weapons
    {
        Pistol,
        Shotgun
    }
    public class Hero : Sprite, IUpdatable, IEntity, IShootable
    {
        public const float bonusTime = 5;
        public float elapsedBonusTime;
        public Weapons weapon;
        public Image HeroImage;
        public bool IsAlive { get; set; }
        private Level level;
        public Sight sight;
        private float speed = 500;
        private Window2D window;
        private const float timeForSingleShot = 0.3f;
        private float timeSinceLastShot;
        public Hero(Vector2f position, Window2D window2D, Level level)
        {
            elapsedBonusTime = 0;
            weapon = Weapons.Pistol;
            this.level = level;
            HeroImage = new Image("Sprites/Player/player_gun.png");
            Texture = new Texture(HeroImage);
            TextureRect = new IntRect(0, 0, 49, 43);
            Scale = new Vector2f(1f, 1f);
            Origin = new Vector2f(GetLocalBounds().Width / 2, GetLocalBounds().Height / 2);
            Position = position;
            sight = new Sight(window2D);
            window = window2D;
            IsAlive = true;
            timeSinceLastShot = 0f;
        }

        public void Shoot()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && timeSinceLastShot >= timeForSingleShot)
            {
                if (weapon == Weapons.Pistol)
                    Pistol.Shoot(level, Position, sight.Position, this);
                if (weapon == Weapons.Shotgun)
                    Shotgun.Shoot(level, Position, sight.Position, this);
                timeSinceLastShot = 0f;
            }
        }
        
        public void Update(GameTime gameTime)
        {
            sight.Update(gameTime);

            var heroRect = GetGlobalBounds();
            Sprite bonus = null;
            foreach (var s in level.Map.Shotguns)
            {
                if (heroRect.Intersects(s.GetGlobalBounds()))
                {
                    bonus = s;
                    weapon = Weapons.Shotgun;
                    elapsedBonusTime = 0f;
                    break;
                }
            }
            if (bonus != null)
            {
                level.Map.Shotguns.Remove(bonus);
                bonus = null;
            }
            
            Move(gameTime);
            RotateToCursor();

            timeSinceLastShot += gameTime.DeltaTime;
            Shoot();

            if (weapon == Weapons.Shotgun)
            {
                elapsedBonusTime += gameTime.DeltaTime;
            }
            if (elapsedBonusTime >= bonusTime)
            {
                elapsedBonusTime = 0f;
                weapon = Weapons.Pistol;
            }
        }

        
        public void RotateToCursor()
        {
            var mouseVec = (Vector2f) Mouse.GetPosition(window);
            var sightLine = sight.Position - Position;
            var rotation = ((float) Math.Atan2(sightLine.Y, sightLine.X)) * 180 / (float) Math.PI;
            Rotation = rotation;
        }

        public bool CheckCollisions(Vector2f delta)
        {
            var objects = level.Map.CollisionObjects;

            var collisionRect = GetSpriteRectangleWithoutRotation();
            collisionRect.Left += delta.X;
            collisionRect.Top += delta.Y;
            foreach (var obj in objects)
            {
                if (obj.Rect.Intersects(collisionRect))
                    return true;
            }

            // foreach (var enemy in level.Enemies)
            // {
            //     if (enemy.GetGlobalBounds().Intersects(collisionRect))
            //         return true;
            // }

            return false;
        }

        public FloatRect GetSpriteRectangleWithoutRotation()
        {
            var tempRotation = Rotation;
            Rotation = 0;
            var rect = GetGlobalBounds();
            Rotation = tempRotation;
            return rect;
        }

    private void Move(GameTime gameTime)
        {
            if (IsKeyPressed(Key.W))
            {
                if (IsKeyPressed(Key.A))
                {
                    if (TryMove(Directions.UpLeft, gameTime.DeltaTime))
                        return;
                    if (TryMove(Directions.Up, gameTime.DeltaTime))
                        return;
                    if (TryMove(Directions.Left, gameTime.DeltaTime))
                        return;
                }

                if (IsKeyPressed(Key.D))
                {
                    if(TryMove(Directions.UpRight, gameTime.DeltaTime))
                        return;
                    if(TryMove(Directions.Up, gameTime.DeltaTime))
                        return;
                    if (TryMove(Directions.Right, gameTime.DeltaTime))
                        return;
                }

                TryMove(Directions.Up, gameTime.DeltaTime);
                return;
            }
            
            if (IsKeyPressed(Key.S))
            {
                if (IsKeyPressed(Key.A))
                {
                    if (TryMove(Directions.DownLeft, gameTime.DeltaTime))
                        return;
                    if (TryMove(Directions.Down, gameTime.DeltaTime))
                        return;
                    if (TryMove(Directions.Left, gameTime.DeltaTime))
                        return;
                }
                if (IsKeyPressed(Key.D))
                {
                    if(TryMove(Directions.DownRight, gameTime.DeltaTime))
                        return;
                    if(TryMove(Directions.Down, gameTime.DeltaTime))
                        return;
                    if (TryMove(Directions.Right, gameTime.DeltaTime))
                        return;
                }

                TryMove(Directions.Down, gameTime.DeltaTime);
                return;
            }

            if (IsKeyPressed(Key.A) && TryMove(Directions.Left, gameTime.DeltaTime))
                return;
            if (IsKeyPressed(Key.D))
                TryMove(Directions.Right, gameTime.DeltaTime);
        }
        
        private bool TryMove(Vector2f direction, float deltaTime)
        {
            var moveVector = speed * direction * deltaTime;
            if (!CheckCollisions(moveVector))
            {
                Position += moveVector;
                return true;
            }

            return false;
        }
    }
}
