using SFML.System;
using SFML.Window;

namespace Tonight
{
    public abstract class GameProcess
    {
        protected readonly Window2D window2D = new Window2D();
        private const float TIME_BEFORE_UPDATE = 1f / 60;
        private GameTime gameTime = new GameTime();
        private bool isPause = false;
        public void Run()
        {
            var totalTimeBeforeUpdate = 0f;
            var previousTimeElapsed = 0f;
            var clock = new Clock();
            Initialize();
            window2D.SetKeyRepeatEnabled(false);
            window2D.SetMouseCursorVisible(false);
            while (window2D.IsOpen)
            {
                clock = new Clock();
                previousTimeElapsed = 0f;
                if(!isPause)
                    while (window2D.IsOpen)
                    {
                        if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                        {
                            isPause = true;
                            break;
                        }

                        window2D.DispatchEvents();
                
                        var totalTimeElapsed = clock.ElapsedTime.AsSeconds();
                        var deltaTime = totalTimeElapsed - previousTimeElapsed;
                        previousTimeElapsed = totalTimeElapsed;
                        totalTimeBeforeUpdate += deltaTime;
                
                        if (totalTimeBeforeUpdate >= TIME_BEFORE_UPDATE)
                        {
                            gameTime.Update(totalTimeBeforeUpdate, totalTimeElapsed);
                            Update(gameTime);
                            totalTimeBeforeUpdate = 0f;
                            window2D.Clear();
                            window2D.DrawBG();
                            Draw();
                            window2D.Display();
                        }
                    }
            }
        }

        protected abstract void Initialize();
        protected abstract void Update(GameTime gameTime);
        protected abstract void Draw();
    }
}