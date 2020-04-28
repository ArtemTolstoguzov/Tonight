using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace Tonight
{
    public class SecurityGuy : Sprite, IUpdatable
    {
        private Image SecurityGuyImage;
        private ViewZone viewZone;
        private bool isAlive = true;

        public SecurityGuy(Window2D window2D, Vector2f position)
        {
            SecurityGuyImage = new Image("images/enemy.png");
            SecurityGuyImage.CreateMaskFromColor(Color.White);
            Texture = new Texture(SecurityGuyImage);
            TextureRect = new IntRect(0, 0, 30, 30);
            Scale = new Vector2f(1f, 1f);
            Origin = new Vector2f(GetLocalBounds().Width / 2, GetLocalBounds().Height / 2);
            Position = position;
        }

        public bool IsKilled(Hero hero)
        {
            var heroRect = hero.GetGlobalBounds();
            var securityGuyRect = GetGlobalBounds();

            if (heroRect.Intersects(securityGuyRect))
                isAlive = false;

            return !isAlive;
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
