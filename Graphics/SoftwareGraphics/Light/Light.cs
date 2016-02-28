using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public abstract class Light
    {
        protected float power;
        protected Vector3 color;

        public float Power
        {
            get { return power; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "value must be >= 0.");

                power = value;
            }
        }

        public Color Color
        {
            get
            {
                return Color.FromArgb(
                    (int)(color.X * 255),
                    (int)(color.Y * 255),
                    (int)(color.Z * 255));
            }
            set
            {
                color = new Vector3(value.R, value.G, value.B) / 255;
            }
        }

        public Light()
            : this(1f, Color.White)
        {
        }

        public Light(float power, Color color)
        {
            Power = power;
            Color = color;
        }

        public abstract Vector3 CalculateIntensivity(Triangle polygon, Vector3 normal);

        public static Color CalculateColor(Color color, Vector3 intensivity)
        {
            if (intensivity.X < 0)
                intensivity.X = 0;
            if (intensivity.Y < 0)
                intensivity.Y = 0;
            if (intensivity.Z < 0)
                intensivity.Z = 0;

            float r = (float)color.R / 255;
            float g = (float)color.G / 255;
            float b = (float)color.B / 255;

            return Color.FromArgb(
                color.A,
                Math.Min((int)(r * intensivity.X * 255), 255),
                Math.Min((int)(g * intensivity.Y * 255), 255),
                Math.Min((int)(b * intensivity.Z * 255), 255));
        }
    }
}
