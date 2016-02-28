using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public class ConeLight : PointLight
    {
        Vector3 direction;
        float cos;
        
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = Vector3.Normalize(value); }
        }

        public float Angle
        {
            get { return (float)Math.Acos(cos); }
            set
            {
                if (value < 0 || value > Math.PI / 2)
                    throw new ArgumentOutOfRangeException("value", "value must be in [0..PI/2].");

                cos = (float)Math.Cos(value);
            }
        }

        public ConeLight(Vector3 position, Vector3 direction, float angle, float power, Color color)
            : base(position, power, color)
        {
            Direction = direction;
            Angle = angle;
        }

        public override Vector3 CalculateIntensivity(Triangle polygon, Vector3 normal)
        {
            Vector3 polygonDirection = Vector3.Normalize(polygon.Center - Position);
            float cosP = Vector3.Dot(polygonDirection, Direction);
            float intensivity = 0;

            if (cosP >= cos)
                intensivity = Vector3.Dot(polygonDirection, -normal);

            return color * intensivity * Power;
        }
    }
}
