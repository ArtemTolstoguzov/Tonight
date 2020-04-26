using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Keyboard;

namespace Tonight
{
    class Hero : Sprite
    {
        public Image HeroImage;
        public Map map;
        public Sight sight;
        private Vector2f? deltaMoveVector;
        private const float delta = 500;
        private Vector2f deltaToUp = new Vector2f(0, -delta);
        private Vector2f deltaToDown = new Vector2f(0, delta);
        private Vector2f deltaToLeft = new Vector2f(-delta, 0);
        private Vector2f deltaToRight = new Vector2f(delta, 0);
        
        public Hero(Window2D window2D, Map map)
        {
            this.map = map;
            sight = new Sight(window2D,20, (float)Math.PI / 6);
            HeroImage = new Image("images/hero1.png");
            Texture = new Texture(HeroImage);
            TextureRect = new IntRect(6, 232, 84, 53);
            Scale = new Vector2f(1f, 1f); // Масштаб размеров игрока относительно картинки
            Origin = new Vector2f(GetLocalBounds().Width / 2, GetLocalBounds().Height / 2); // Центр героя, активная точка
            Position = new Vector2f(250, 150);
            window2D.KeyPressed += OnKeyPressed;
            window2D.KeyReleased += OnKeyReleased;
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
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
            if (deltaMoveVector != null)
            {
                var moveVector = gameTime.DeltaTime * deltaMoveVector.Value;
                if (!CheckCollisions(moveVector))
                {
                    Position += moveVector;
                }
            }
        }
        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Key.W:
                    deltaMoveVector = deltaToUp;
                    break;
                case Key.A:
                    deltaMoveVector = deltaToLeft;
                    break;
                case Key.S:
                    deltaMoveVector = deltaToDown;
                    break;
                case Key.D:
                    deltaMoveVector = deltaToRight;
                    break;
            }
        }

        private void OnKeyReleased(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Key.W:
                case Key.A:
                case Key.S:
                case Key.D:
                    deltaMoveVector = null;
                    break;
            }
        }
    }
}
