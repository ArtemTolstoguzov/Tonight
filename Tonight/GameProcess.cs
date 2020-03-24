using System;
using System.Collections.Generic;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using SFML.Graphics;

namespace Tonight
{
    class GameProcess
    {
        private readonly Window2D window2D = new Window2D();
        private readonly Hero Hero = new Hero();

        public GameProcess()
        {
            window2D.KeyPressed += Window2DKeyPressed;
        }

        private void Window2DKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.W)
            {
                Hero.Position += new Vector2f(0, -10);
                Hero.TextureRect = new IntRect(0, 288, 96, 96);
            }
            if (e.Code == Keyboard.Key.S)
            {
                Hero.Position += new Vector2f(0, 10);
                Hero.TextureRect = new IntRect(0, 0, 96, 96);
            }
            if (e.Code == Keyboard.Key.A)
            {
                Hero.Position += new Vector2f(-10, 0);
                Hero.TextureRect = new IntRect(0, 96, 96, 96);
            }
            if (e.Code == Keyboard.Key.D)
            {
                Hero.Position += new Vector2f(10, 0);
                Hero.TextureRect = new IntRect(0, 192, 96, 96);
            }
        }

        public void Run()
        {

            while (window2D.IsOpen == true)
            {
                window2D.DispatchEvents();

                window2D.DrawBG();

                window2D.Draw(Hero);

                window2D.Display();
            }
        }
    }
}
