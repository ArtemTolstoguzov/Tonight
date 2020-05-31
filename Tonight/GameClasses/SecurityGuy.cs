using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace Tonight
{
    enum EnemyState
    {
        Patrol,
        MovingToPoint,
        Shoot
    }
    public class SecurityGuy : Sprite, IUpdatable, IEntity
    {
        public List<Bullet> Bullets { get; set; }

        public bool IsAlive { get; set; }
        public Segment SegmentToPlayer;
        private EnemyState state;
        private Image SecurityGuyImage;
        private ViewZone viewZone;
        private Level level;
        private static Random random = new Random();
        private Vector2i? tileDestination;
        private Vector2f pointDestination;
        private Vector2i patrolDirection;
        private const float speed = 100;
        private Hero hero;

        public SecurityGuy(Vector2f position, Level level)
        {
            this.level = level;
            SecurityGuyImage = new Image("Sprites/SecurityGuy/security_gun.png");
            SecurityGuyImage.CreateMaskFromColor(Color.White);
            Texture = new Texture(SecurityGuyImage);
            TextureRect = new IntRect(0, 0, 51, 43);;
            Scale = new Vector2f(1f, 1f);
            Origin = new Vector2f(GetLocalBounds().Width / 2, GetLocalBounds().Height / 2);
            Position = position;
            IsAlive = true;
            state = EnemyState.Patrol;
        }

        public bool CheckCollisions(Vector2f delta)
        {
            var countColiisions = 0;
            var entities = level.GetEntities();

            var collisionRect = GetSpriteRectangleWithoutRotation();
            collisionRect.Left += delta.X;
            collisionRect.Top += delta.Y;
            foreach (var entity in entities)
            {
                if (entity.GetSpriteRectangleWithoutRotation().Intersects(collisionRect))
                    countColiisions++;
            }

            return countColiisions > 1;
        }
        
        public FloatRect GetSpriteRectangleWithoutRotation()
        {
            var tempRotation = Rotation;
            Rotation = 0;
            var rect = GetGlobalBounds();
            Rotation = tempRotation;
            return rect;
        }
        private void Move(GameTime gameTime)
        {
            var direction = Directions.Normalize(level.Map.ConvertToWindowCoordinates((Vector2i)tileDestination) - Position);
            var moveVector = speed * direction * gameTime.DeltaTime;
            if (tileDestination != null 
                && level.Map.GetTileGidInLayer((Vector2i)tileDestination, level.Map.collisionTiles) != Map.Wall
                && !CheckCollisions(moveVector))
            {
                Position += moveVector;
                Rotation = GetRotationDependingOnDirection(moveVector);
                if (Directions.IsVectorsEqual(Position, pointDestination))
                    tileDestination = null;
            }
        }

        private float GetRotationDependingOnDirection(Vector2f direction)
        {
            var dx = Math.Sign((int)direction.X);
            var dy = Math.Sign((int)direction.Y);
            if (dx == -1 && dy == -1)
                return 225;
            if (dx == -1 && dy == 0)
                return 180;
            if (dx == -1 && dy == 1)
                return 135;
            if (dx == 0 && dy == -1)
                return 270;
            if (dx == 0 && dy == 0)
                return Rotation;
            if (dx == 0 && dy == 1)
                return 90;
            if (dx == 1 && dy == -1)
                return -45;
            if (dx == 1 && dy == 0)
                return 0;
            if (dx == 1 && dy == 1)
                return 45;
            return Rotation;
        }

        private void Patrol(GameTime gameTime)
        {
            var rnd = random.Next(0, 100);
            if (rnd < 2)
            {
                patrolDirection = Directions.GetIntDirection((DirectionsEnum) random.Next(0, 8));
            }

            var enemyTileCoordinates = level.Map.ConvertToTileCoordinates(Position);
            DiagonallyMove(enemyTileCoordinates + patrolDirection);
            Move(gameTime);

        }
        
        private void DiagonallyMove(Vector2i tilePoint)
        {
            tileDestination = tilePoint;
            pointDestination = level.Map.ConvertToWindowCoordinates(tilePoint);
        }
        public void Update(GameTime gameTime)
        {
            //SegmentToPlayer = new Segment();
            Patrol(gameTime);
        }
    }
}
