using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Tonight
{
    class Hero: Sprite
    {
        public Image HeroImage;
        private const float delta = 10;
        private Vector2f deltaToUp = new Vector2f(0, -delta);
        private Vector2f deltaToDown = new Vector2f(0, delta);
        private Vector2f deltaToLeft = new Vector2f(-delta, 0);
        private Vector2f deltaToRight = new Vector2f(delta, 0);

        public Hero(Window2D window2D)
        {
            HeroImage = new Image("images/hero.png");
            Texture = new Texture(HeroImage);
            TextureRect = new IntRect(0, 192, 96, 96);
            Scale = new Vector2f(1f, 1f); // Масштаб размеров игрока относительно картинки
            Origin = new Vector2f(GetLocalBounds().Width / 2, GetLocalBounds().Height / 2); // Центр героя, активная точка
            Position = new Vector2f(150, 50);
            window2D.KeyPressed += Move;
        }

        private void Move(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.W)
            {
                Position += deltaToUp;
                TextureRect = new IntRect(0, 288, 96, 96);
            }
            if (e.Code == Keyboard.Key.S)
            {
                Position += deltaToDown;
                TextureRect = new IntRect(0, 0, 96, 96);
            }
            if (e.Code == Keyboard.Key.A)
            {
                Position += deltaToLeft;
                TextureRect = new IntRect(0, 96, 96, 96);
            }
            if (e.Code == Keyboard.Key.D)
            {
                Position += deltaToRight;
                TextureRect = new IntRect(0, 192, 96, 96);
            }
        }
    }
}
