using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SoftwareGraphics
{
    public struct Triangle
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        public Color Color;

        public Vector3 Normal
        {
            get
            {
                return Vector3.Normalize(Vector3.Cross(C - A, B - A));
            }
        }

        public Vector3 Center
        {
            get
            {
                return (A + B + C) / 3f;
            }
        }

        public Triangle(Vector3 a, Vector3 b, Vector3 c, Color color)
        {
            A = a;
            B = b;
            C = c;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            Triangle? triangle = obj as Triangle?;

            if (!triangle.HasValue)
                return false;

            return this.Equals(triangle.Value);
        }

        public bool Equals(Triangle other)
        {
            return A.Equals(other.A) && B.Equals(other.B) && C.Equals(other.C)
                && this.Color == other.Color;
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode() ^ C.GetHashCode();
        }
    }
}
