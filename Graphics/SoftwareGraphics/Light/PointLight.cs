using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public class PointLight : Light
    {
        public Vector3 Position { get; set; }

        public PointLight(Vector3 position, float power, Color color)
            : base(power, color)
        {
            Position = position;
        }

        public override Vector3 CalculateIntensivity(Triangle polygon, Vector3 normal)
        {
            Vector3 direction = Vector3.Normalize(polygon.Center - Position);
            float intesivity = Vector3.Dot(direction, -normal);
            return color * intesivity * Power;
        }
    }
}
