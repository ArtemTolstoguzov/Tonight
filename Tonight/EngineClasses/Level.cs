using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Tonight
{
    public class Level: GameProcess
    {
        public Map Map;
        public Hero Player;
        public List<SecurityGuy> Enemies;
        private Camera camera;
        private Font bonusFont;
        private string mapPath;
        private static Color bonusColor = new Color(130, 57, 203);

        public Level(string mapPath, Window2D window)
        {
            this.mapPath = mapPath;
            window2D = window;
        }
        
        public IEnumerable<IEntity> GetEntities()
        {
            yield return Player;
            foreach (var enemy in Enemies)
            {
                yield return enemy;
            }
        }

        public Vector2f GetPlayerCoordinates()
        {
            return Player.Position;
        }
        protected override void Initialize()
        {
            camera = new Camera(1920, 1080);
            Map = new Map(mapPath, camera);
            Player = new Hero(Map.GetStartPlayerCoordinates(), window2D, this);
            Enemies = Map.GetEnemies().Select(o => new SecurityGuy(o.Position, this)).ToList();
            window2D.SetMouseCursorVisible(true);
            bonusFont = new Font("FrizQuadrataBoldItalic.ttf");
        }

        protected override void Update(GameTime gameTime)
        {
            camera.Move(Player, Map);
            Player.Update(gameTime);
            Map.Update(gameTime);
            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime);
            }
            window2D.SetView(camera);
        }

        protected override void Draw()
        {
            window2D.Draw(Map);
            window2D.Draw(Player);
            window2D.Draw(Player.sight);

            foreach (var s in Map.Shotguns)
            {
                window2D.Draw(s);
            }
            foreach (var bullet in Map.Bullets)
            {
                window2D.Draw(bullet);
            }
            foreach (var enemy in Enemies)
            {
                window2D.Draw(enemy);
            }


            if (Player.weapon == Weapons.Shotgun)
            {
                var leftTime = Hero.bonusTime - Player.elapsedBonusTime;
                var text = new Text(leftTime.ToString(), bonusFont, 57);
                text.Color = bonusColor;
                text.Position = new Vector2f(camera.Center.X + camera.Size.X / 2 - 400, camera.Center.Y - camera.Size.Y / 2 + 70);
                text.Style = Text.Styles.Bold;
                window2D.Draw(text);
            }
        }

        protected override GameResult GetExitCode()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                return GameResult.Escape;
            if (!Enemies.Any())
                return GameResult.Win;
            if (!Player.IsAlive)
                return GameResult.Lose;
            return GameResult.InProcess;
        }
    }
}