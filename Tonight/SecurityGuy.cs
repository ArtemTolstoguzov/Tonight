using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace Tonight
{
    class SecurityGuy : Sprite
    {
        private Image SecurityGuyImage;
        public bool isAlive = true;

        public SecurityGuy(Window2D window2D, Vector2f position)
        {
            SecurityGuyImage = new Image("images/hero.png");
            Texture = new Texture(SecurityGuyImage);
            TextureRect = new IntRect(0, 192, 96, 96);
            Scale = new Vector2f(1f, 1f); // Масштаб размеров игрока относительно картинки
            Origin = new Vector2f(GetLocalBounds().Width / 2, GetLocalBounds().Height / 2); // Центр героя, активная точка
            Position = position;
        }

        public bool IsKilled(Hero hero)
        {
            var heroRect = hero.GetGlobalBounds();
            var secguyRect = GetGlobalBounds();

            if (heroRect.Intersects(secguyRect))
                isAlive = false;

            return !isAlive;
        }

    }
}
