using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public class GlobalDirectedLight : Light
    {
        private Vector3 direction;

        public Vector3 Direction 
        { 
            get { return direction; }
            set { direction = Vector3.Normalize(value); }
        }

        public GlobalDirectedLight(Vector3 direction, float power, Color color)
            : base(power, color)
        {
            Direction = Vector3.Normalize(direction);
        }

        public override Vector3 CalculateIntensivity(Triangle polygon, Vector3 normal)
        {
            float intensivity = Vector3.Dot(Direction, -normal);
            return color * intensivity * Power;
        }
    }
}
