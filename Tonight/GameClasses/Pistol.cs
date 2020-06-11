using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Audio;

namespace Tonight
{
    public static class Pistol
    {
        private static float accuracy = 0.1f;
        private static Random random = new Random();
        private static SoundBuffer soundBuffer = new SoundBuffer("sounds/shoot_pistol.ogg");
        private static Sound shootingSound = new Sound(soundBuffer);

        public static void Shoot(Level level, Vector2f source, Vector2f destination, IEntity owner)
        {
            var length = Directions.Length(destination - source);
            var epsilon = (int)(accuracy * length);
            var xEps = random.Next(-epsilon, epsilon);
            var yEps = random.Next(-epsilon, epsilon);
            destination.X += xEps;
            destination.Y += yEps;
            level.Map.Bullets.Add(new Bullet(source, destination, level, owner));

            shootingSound.Volume = 80;
            shootingSound.Play();
        }
    }
}
