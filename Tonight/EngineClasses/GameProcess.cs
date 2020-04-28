using System;
using System.Collections.Generic;
using System.Data;
using SFML.System;
using SFML.Window;
using SFML.Audio;
using SFML.Graphics;
using TiledSharp;

namespace Tonight
{
    public class GameProcess
    {
        private readonly Window2D window2D = new Window2D();
        private GameTime gameTime = new GameTime();
        private const float TIME_BEFORE_UPDATE = 1f / 60;
        private void ShowTime(GameTime gameTime)
        {
            var fps = (1f / gameTime.DeltaTime);
            var str = gameTime.TotalTimeElapsed + " " + gameTime.DeltaTime + " " + fps;
                      Console.WriteLine(str);
        }
        public void Run()
        {
            var camera = new Camera(800, 600);
            var map = new Map("maps/RectMapOnlyWithWallsAndGround.tmx", camera);
            var hero = new Hero(window2D, map);
            window2D.SetMouseCursorVisible(false);

            var sector = new ViewZone(150, (float)Math.PI / 2);     //!
            sector.Rotate(-(float)Math.PI / 2);
            var enemy = new SecurityGuy(window2D, new Vector2f(300, 150));

            var totalTimeBeforeUpdate = 0f;
            var previousTimeElapsed = 0f;

            var clock = new Clock();
            
            while (window2D.IsOpen)
            {
                window2D.DispatchEvents();
                var totalTimeElapsed = clock.ElapsedTime.AsSeconds();
                var deltaTime = totalTimeElapsed - previousTimeElapsed;
                previousTimeElapsed = totalTimeElapsed;

                totalTimeBeforeUpdate += deltaTime;

                
                if (totalTimeBeforeUpdate >= TIME_BEFORE_UPDATE)
                {
                    gameTime.Update(totalTimeBeforeUpdate, totalTimeElapsed);
                    hero.Update(gameTime);
                    camera.Move(hero.Position, map);
                    //ShowTime(gameTime);
                    totalTimeBeforeUpdate = 0f;
                    window2D.SetView(camera);
                    window2D.Clear();
                    window2D.DrawBG();
                    window2D.Draw(map);

                    sector.Position = hero.Position;    //
                    window2D.Draw(sector);              //It should be better
                    window2D.Draw(hero);                //
                    window2D.Draw(hero.sight);          //
                    
                    if (!sector.Intersects(enemy.Position))
                        window2D.Draw(enemy);

                    window2D.Display();
                    
                }
            }
        }
    }
}
