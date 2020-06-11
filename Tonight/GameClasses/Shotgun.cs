using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Audio;

namespace Tonight
{
    public static class Shotgun
    {
        private static float accuracy = 0.4f;
        private static int bulletCount = 5;
        private static Random random = new Random();
        private static SoundBuffer soundBuffer = new SoundBuffer("sounds/shoot_shotgun.ogg");
        private static Sound shootingSound = new Sound(soundBuffer);

        public static void Shoot(Level level, Vector2f source, Vector2f destination, IEntity owner)
        {
            var length = Directions.Length(destination - source);
            var epsilon = (int)(accuracy * length);

            for (int i = 0; i < bulletCount; i++)
            {
                var xEps = random.Next(-epsilon, epsilon);
                var yEps = random.Next(-epsilon, epsilon);
                var deltaVec = new Vector2f(destination.X + xEps, destination.Y + yEps);
                level.Map.Bullets.Add(new Bullet(source, deltaVec, level, owner));
            }

            shootingSound.Volume = 80;
            shootingSound.Play();
        }
    }
}
