using System;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tonight
{
    class Program
    {
        static void Main()
        {
            //var testLevel = new Level("maps/NiceTestMapV2.tmx", new Window2D());
            //testLevel.Run();
            var menu = new Menu(new Window2D());
            menu.Run();
        }
    }
}
