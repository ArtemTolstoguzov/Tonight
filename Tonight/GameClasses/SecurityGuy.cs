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
        public List<Bullet> Bullets { get; set; }
        public bool IsAlive { get; set; }
        private Image SecurityGuyImage;
        private ViewZone viewZone;

        public SecurityGuy(Vector2f position)
        {
            SecurityGuyImage = new Image("images/hero1.png");
            SecurityGuyImage.CreateMaskFromColor(Color.White);
            Texture = new Texture(SecurityGuyImage);
            TextureRect = new IntRect(6, 232, 84, 53);;
            Scale = new Vector2f(1f, 1f);
            Origin = new Vector2f(GetLocalBounds().Width / 2, GetLocalBounds().Height / 2);
            Position = position;
            IsAlive = true;
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
