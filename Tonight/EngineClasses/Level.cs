using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace Tonight
{
    public class Level: GameProcess
    {
        public Map Map;
        private Hero Player;
        public List<SecurityGuy> Enemies;
        private Camera camera;
        private string mapPath;

        public Level(string mapPath)
        {
            this.mapPath = mapPath;
        }

        public IEnumerable<IEntity> GetEntities()
        {
            yield return Player;
            foreach (var enemy in Enemies)
            {
                yield return enemy;
            }
        }
        protected override void Initialize()
        {
            camera = new Camera(800, 600);
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
                    
                    
                    
            var path = Map.FindPathInTiles(new Vector2i(1, 2), new Vector2i(16, 30));
        }
    }
}