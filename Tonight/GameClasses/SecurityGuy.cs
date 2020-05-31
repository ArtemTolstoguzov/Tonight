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
    }
    public class SecurityGuy : Sprite, IUpdatable, IEntity, IShootable
    {
        public List<Bullet> Bullets { get; set; }

        public bool IsAlive { get; set; }
        public Segment SegmentToPlayer;
        private EnemyState state;
        private Image SecurityGuyImage;
        public ViewZone viewZone;
        private Level level;
        private static Random random = new Random();
        private Vector2i? tileDestination;
        private Vector2f pointDestination;
        private IEnumerator<Vector2i> shortestPathToPoint;
        private Vector2i patrolDirection;
        private float speed = 100;
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
            viewZone = new ViewZone(600, (float) (Math.PI / 1.4));
            IsAlive = true;
            state = EnemyState.Patrol;
        }

        public bool IsPlayerSeen()
        {
            return viewZone.Intersects(level.GetPlayerCoordinates()) && IsPlayerInLineOfSight();
        }

        private bool IsPlayerInLineOfSight()
        {
            var segmentRect = SegmentToPlayer.GetGlobalBounds();
            foreach (var obj in level.Map.CollisionObjects)
            {
                if (obj.Rect.Intersects(segmentRect))
                    return false;
            }

            return true;
        }
        private bool CheckCollisions(Vector2f delta)
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
            speed = 100;
            var rnd = random.Next(0, 100);
            if (rnd < 2)
            {
                patrolDirection = Directions.GetIntDirection((DirectionsEnum) random.Next(0, 8));
            }

            var enemyTileCoordinates = level.Map.ConvertToTileCoordinates(Position);
            DiagonallyMove(enemyTileCoordinates + patrolDirection);
            PatrolMove(gameTime);

        }
        
        private void DiagonallyMove(Vector2i tilePoint)
        {
            tileDestination = tilePoint;
            pointDestination = level.Map.ConvertToWindowCoordinates(tilePoint);
        }
        
        private void PatrolMove(GameTime gameTime)
        {
            if (tileDestination == null)
                return;
            var direction = Directions.Normalize(level.Map.ConvertToWindowCoordinates((Vector2i)tileDestination) - Position);
            var moveVector = speed * direction * gameTime.DeltaTime;
            if (level.Map.GetTileGidInLayer((Vector2i)tileDestination, level.Map.collisionTiles) != Map.Wall
                && !CheckCollisions(moveVector))
            {
                Position += moveVector;
                var oldRotation = Rotation;
                Rotation = GetRotationDependingOnDirection(moveVector);
                if (Directions.IsVectorsEqual(Position, pointDestination))
                {
                    tileDestination = null;
                    state = EnemyState.Patrol;
                }
                viewZone.Position = Position;
                viewZone.Rotate((Rotation - oldRotation) * (float) Math.PI / 180);
            }
        }
        
        private void Move(GameTime gameTime)
        {
            if (tileDestination == null)
                return;

            if (Directions.IsVectorsEqual(Position, pointDestination))
            {
                if (shortestPathToPoint == null || !shortestPathToPoint.MoveNext())
                {
                    state = EnemyState.Patrol;
                    return;
                }
                tileDestination = shortestPathToPoint.Current;
                pointDestination = level.Map.ConvertToWindowCoordinates((Vector2i)tileDestination);
            }
            
            var direction = Directions.Normalize(level.Map.ConvertToWindowCoordinates((Vector2i)tileDestination) - Position);
            var moveVector = speed * direction * gameTime.DeltaTime;
            if (level.Map.GetTileGidInLayer((Vector2i)tileDestination, level.Map.collisionTiles) != Map.Wall
                && !CheckCollisions(moveVector))
            {
                Position += moveVector;
                var oldRotation = Rotation;
                Rotation = GetRotationDependingOnDirection(moveVector);
                viewZone.Position = Position;
                viewZone.Rotate((Rotation - oldRotation) * (float) Math.PI / 180);
            }
        }
        public void Shoot()
        { 
            level.Map.Bullets.Add(new Bullet(Position, level.GetPlayerCoordinates(),  level, this));
        }

        public void NotifyAboutShooting(Vector2f position)
        {
            var tiledPosition = level.Map.ConvertToTileCoordinates(Position);
            var shootingPositionInTiles = level.Map.ConvertToTileCoordinates(position);
            var path = level.Map.FindPathInTiles(tiledPosition, shootingPositionInTiles);
            if (path != null)
            {
                shortestPathToPoint = path.Reverse().GetEnumerator();
            }
            else
            {
                shortestPathToPoint = null;
            }
            tileDestination = level.Map.ConvertToTileCoordinates(Position);
            pointDestination = level.Map.ConvertToWindowCoordinates(tiledPosition);
            speed = 300;
            state = EnemyState.MovingToPoint;
        }
        public void Update(GameTime gameTime)
        {
            SegmentToPlayer = new Segment(Position, level.GetPlayerCoordinates());
            if(state == EnemyState.Patrol)
                Patrol(gameTime);
            if (state == EnemyState.MovingToPoint)
                Move(gameTime);
            if(IsPlayerSeen())
                Shoot();
        }
    }
}
