﻿using System;
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

        private const float TIME_BEFORE_UPDATE = 1f / 60;


        public void Run()
        {
            var hero = new Hero(window2D);
            var sector = new ViewZone(150, (float)Math.PI / 2);     //!
            var enemy = new SecurityGuy(window2D, new Vector2f(300, 150));

            var totalTimeBeforeUpdate = 0f;
            var previousTimeElapsed = 0f;
            var deltaTime = 0f;
            var totalTimeElapsed = 0f;

            var clock = new Clock();

            while (window2D.IsOpen == true)
            {
                window2D.DispatchEvents();

                totalTimeElapsed = clock.ElapsedTime.AsSeconds();
                deltaTime = totalTimeElapsed - previousTimeElapsed;
                previousTimeElapsed = totalTimeElapsed;

                totalTimeBeforeUpdate += deltaTime;

                if (totalTimeBeforeUpdate >= TIME_BEFORE_UPDATE)
                {
                    totalTimeBeforeUpdate = 0f;
                    window2D.DrawBG();

                    sector.Position = hero.Position;    //
                    window2D.Draw(sector);              //It should be better
                    window2D.Draw(hero);                //

                    if (!sector.Intersects(enemy.Position))
                        window2D.Draw(enemy);

                    window2D.Display();
                }
            }
        }
    }
}
