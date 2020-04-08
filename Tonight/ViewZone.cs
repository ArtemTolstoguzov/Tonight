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
        private Vector2f direction;
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
            direction = GetPoint(sectorPointCount / 2);
        }

        public void Rotate(float angleRad)
        {
            var angleGrad = angleRad * 180 / (float)Math.PI;
            var x1 = direction.X * (float)Math.Cos(angleRad) - direction.Y * (float)Math.Sin(angleRad);
            var y1 = direction.X * (float)Math.Sin(angleRad) + direction.Y * (float)Math.Cos(angleRad);
            direction = new Vector2f(x1, y1);
            Rotation += angleGrad;
        }

        public bool Intersects(Vector2f point)
        {
            var vec = direction;
            point = point - Position;
            var firstCond = point.X * point.X + point.Y * point.Y <= vec.X * vec.X + vec.Y * vec.Y;
            var secondCond = (vec.X * point.X + vec.Y * point.Y) / (Math.Sqrt(vec.X * vec.X + vec.Y * vec.Y) * Math.Sqrt(point.X * point.X + point.Y * point.Y)) >= Math.Cos(Angle / 2);

            return firstCond && secondCond;
        }
    }
}
