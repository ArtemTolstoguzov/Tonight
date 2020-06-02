using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Tonight
{
    public class Window2D : RenderWindow
    {
        private readonly Sprite background;
        private readonly Texture bgTexture;

        public Window2D() : base(new VideoMode(1920, 1080, 24), "MarcelKey", Styles.Fullscreen)
        {
            bgTexture = new Texture("images/bg.png");
            background = new Sprite(bgTexture);

            base.SetFramerateLimit(60);
            Closed += Window2DClosed;
        }

        private void Window2DClosed(object sender, EventArgs e)
        {
            base.Close();
        }

        public void DrawBG()
        {
            base.Clear(Color.Blue);
            base.Draw(background);
        }
    }
}
