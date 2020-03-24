using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Tonight
{
    class Window2D : RenderWindow
    {
        private readonly Texture texture;
        private readonly Sprite background;

        public Window2D(): base(new VideoMode(800, 600, 24), "Tonight")
        {
            
        }
    }
}
