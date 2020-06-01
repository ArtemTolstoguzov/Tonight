using SFML.System;
using SFML.Window;

namespace Tonight
{
    public enum GameResult
    {
        Win,
        Lose,
        Escape,
        InProcess
    }
    public abstract class GameProcess
    {
        protected Window2D window2D;
        private const float TIME_BEFORE_UPDATE = 1f / 60;
        private GameTime gameTime = new GameTime();
        private GameResult exitCode;
        public GameResult Run()
        {
            exitCode = GameResult.InProcess;
            var totalTimeBeforeUpdate = 0f;
            var previousTimeElapsed = 0f;
            var clock = new Clock();
            Initialize();
            window2D.SetKeyRepeatEnabled(false);
            window2D.SetMouseCursorVisible(false);
            while (window2D.IsOpen && exitCode == GameResult.InProcess)
            {
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

                exitCode = GetExitCode();
            }

            return exitCode;
        }

        protected abstract void Initialize();
        protected abstract void Update(GameTime gameTime);
        protected abstract void Draw();
        protected abstract GameResult GetExitCode();
    }
}