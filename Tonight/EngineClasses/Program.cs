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
            //var GameProcess = new GameProcess();
            //GameProcess.Run();
            var testLevel = new Level("maps/NiceTestMapV2.tmx");
            testLevel.Run();
        }
    }
}
