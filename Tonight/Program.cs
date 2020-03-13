using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonight
{
    class Program
    {
        static RenderWindow win;

        static void Main(string[] args)
        {
            win = new RenderWindow(new SFML.Window.VideoMode(800, 600), "Tonight");
            win.SetVerticalSyncEnabled(true);

            win.Closed += WinClosed;
            while (win.IsOpen)
            {
                win.DispatchEvents();

                win.Clear(Color.Black);

                win.Display();
            }
        }

        private static void WinClosed(object sender, EventArgs e)
        {
            win.Close();
        }
    }
}
