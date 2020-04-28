using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace Tonight
{
    public class Hero : Sprite, IUpdatable
    {
        public Image HeroImage;
        public Map map;
        public Sight sight;
        private float speed = 500;
        private Window2D window;
        public Hero(Window2D window2D, Map map)
        {
            this.map = map;
            sight = new Sight(window2D);
            HeroImage = new Image("images/hero.png");
            Texture = new Texture(HeroImage);
            TextureRect = new IntRect(6, 232, 84, 53);
            Scale = new Vector2f(1f, 1f);
            Origin = new Vector2f(GetLocalBounds().Width / 2, GetLocalBounds().Height / 2);
            Position = new Vector2f(250, 150);
            window = window2D;
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
            RotateToCursor();
        }

        public void RotateToCursor()
        {
            var mousePoint = (Vector2f) Mouse.GetPosition(window);
            var sightLine = mousePoint-Position;
            var rotation = ((float) Math.Atan2(sightLine.Y, sightLine.X)) * 180 / (float) Math.PI;
            Rotation = rotation;
        }
        public bool CheckCollisions(Vector2f delta)
        {
            var objects = map.mapObjects["collision"];
            var heroRect = GetGlobalBounds();
            heroRect.Left += delta.X;
            heroRect.Top += delta.Y;
            foreach (var obj in objects)
            {
                if (obj.Rect.Intersects(heroRect))
                    return true;
            }

            return false;
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
