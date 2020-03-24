using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Tonight
{
    class Hero: Sprite
    {
        public Image HeroImage;

        public Hero()
        {
            HeroImage = new Image("images/hero.png");
            Texture = new Texture(HeroImage);
            TextureRect = new IntRect(0, 192, 96, 96);
            Scale = new Vector2f(1f, 1f); // Масштаб размеров игрока относительно картинки
            Origin = new Vector2f(GetLocalBounds().Width, GetLocalBounds().Height / 2); // Центр героя, активная точка
            Position = new Vector2f(150, 50);
        }
    }
}
