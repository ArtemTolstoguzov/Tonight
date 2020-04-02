using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Tonight
{
    class ViewZone : ConvexShape
    {
        public float Radius;
        public float Angle;
        private uint sectorPointCount;
        public ViewZone(float radius, float angle, uint pointCount = 30)
        {
            Radius = radius;
            Angle = angle;
            sectorPointCount = pointCount;
            SetPointCount(sectorPointCount);
            Origin = new Vector2f(0, 0);
            SetPoint(0, Origin);
            var rotateAngle = angle / 2;
            for (uint i = 1; i < sectorPointCount; i++)
            {
                var currentAngle = i * angle / pointCount - rotateAngle;
                var x = (float)Math.Cos(currentAngle) * radius;
                var y = (float)Math.Sin(currentAngle) * radius;

                SetPoint(i, new Vector2f(x, y));
            }
        }

        public bool Intersects(Vector2f point)
        {
            var vec = GetPoint(sectorPointCount / 2);
            point = point - Position;
            var firstCond = point.X * point.X + point.Y * point.Y <= vec.X * vec.X + vec.Y * vec.Y;
            var secondCond = (vec.X * point.X + vec.Y * point.Y) / (Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y) * Math.Sqrt(point.X * point.X + point.Y * point.Y)) >= Math.Cos(Angle / 2);
            Console.Write(firstCond);
            Console.Write(" ");
            Console.Write(secondCond);
            Console.Write("\n");

            return firstCond && secondCond;
        }
    }
}
