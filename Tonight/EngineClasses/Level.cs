using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace Tonight
{
    public class Level: GameProcess
    {
        public Map Map;
        public Hero Player;
        public List<SecurityGuy> Enemies;
        private Camera camera;
        private string mapPath;

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

            foreach (var bullet in Map.Bullets)
            {
                window2D.Draw(bullet);
            }
            foreach (var enemy in Enemies)
            {
                window2D.Draw(enemy);
            }
        }

        protected override bool IsExit()
        {
            return !Enemies.Any() || !Player.IsAlive;
        }
    }
}