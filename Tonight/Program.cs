using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonight
{
    public class Hero
    {

    }

    class Program
    {
        public void Move(Sprite sprite)
        {
            
        }

        static RenderWindow win;
        static void Main(string[] args)
        {
            win = new RenderWindow(new SFML.Window.VideoMode(800, 600), "Tonight");
            win.SetVerticalSyncEnabled(true);

            var heroImage = new Image("images/hero.png");
            var heroTexture = new Texture(heroImage);
            var heroSprite = new Sprite(heroTexture);
            heroSprite.TextureRect = new IntRect(0, 192, 96, 96);
            heroSprite.Position = new Vector2f(50, 25);

            
            while (win.IsOpen)
            {
                win.DispatchEvents();

                win.Clear(Color.Black);
                win.Draw(heroSprite);

                

                win.Display();
            }
        }

        private static void WinClosed(object sender, EventArgs e)
        {
            win.Close();
        }
    }
}
