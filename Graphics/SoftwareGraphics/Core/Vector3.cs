using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public struct Vector3
    {
        const string ElementFormat = "F3";

        static Vector3 zero     = new Vector3( 0,  0,  0 );
        static Vector3 unitX    = new Vector3( 1,  0,  0 );
        static Vector3 unitY    = new Vector3( 0,  1,  0 );
        static Vector3 unitZ    = new Vector3( 0,  0,  1 );

        static Vector3 left     = new Vector3(-1,  0,  0 );
        static Vector3 right    = new Vector3( 1,  0,  0 );
        static Vector3 up       = new Vector3( 0,  1,  0 );
        static Vector3 down     = new Vector3( 0, -1,  0 );
        static Vector3 forward  = new Vector3( 0,  0, -1 );
        static Vector3 backward = new Vector3( 0,  0,  1 );

        public static Vector3 Zero  { get { return zero;  } }
        public static Vector3 UnitX { get { return unitX; } }
        public static Vector3 UnitY { get { return unitY; } }
        public static Vector3 UnitZ { get { return unitZ; } }

        public static Vector3 Left     { get { return left;     } }
        public static Vector3 Right    { get { return right;    } }
        public static Vector3 Up       { get { return up;       } }
        public static Vector3 Down     { get { return down;     } }
        public static Vector3 Forward  { get { return forward;  } }
        public static Vector3 Backward { get { return backward; } }

        public float X;
        public float Y;
        public float Z;

        public float Length
        {
            get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public float LengthSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(float xyzValue)
            : this(xyzValue, xyzValue, xyzValue)
        {
        }

        #region Static Methods

        public static bool Equals(Vector3 v1, Vector3 v2)
        {
            return v1 == v2;
        }

        public static Vector3 Negate(Vector3 v)
        {
            return -v;
        }

        public static Vector3 Add(Vector3 v1, Vector3 v2)
        {
            return v1 + v2;
        }

        public static Vector3 Subtract(Vector3 v1, Vector3 v2)
        {
            return v1 - v2;
        }

        public static Vector3 Multiply(Vector3 v, float scalar)
        {
            return v * scalar;
        }

        public static float Distance(Vector3 v1, Vector3 v2)
        {
            return (v2 - v1).Length;
        }

        public static float DistanceSquared(Vector3 v1, Vector3 v2)
        {
            return (v2 - v1).LengthSquared;
        }

        public static Vector3 Normalize(Vector3 v)
        {
            return v / v.Length;
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.Y * v2.Z - v1.Z * v2.Y,
                v1.Z * v2.X - v1.X * v2.Z,
                v1.X * v2.Y - v1.Y * v2.X);
        }

        public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
        {
            float x = v.X;
            x = x < min.X ? min.X : x;
            x = x > max.X ? max.X : x;

            float y = v.Y;
            y = y < min.Y ? min.Y : y;
            y = y > max.Y ? max.Y : y;

            float z = v.Z;
            z = z < min.Z ? min.Z : z;
            z = z > max.Z ? max.Z : z;

            return new Vector3(x, y, z);
        }

        public static Vector3 Transform(Vector3 v, ref Matrix m)
        {
            return new Vector3(
                v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31 + m.M41,
                v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32 + m.M42,
                v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33 + m.M43);
        }

        public static Vector3 Transform(Vector3 v, Matrix m)
        {
            return Transform(v, ref m);
        }

        public static Vector3 TransformNormal(Vector3 v, ref Matrix m)
        {
            return new Vector3(
                v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31,
                v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32,
                v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33);
        }

        public static Vector3 TransformNormal(Vector3 v, Matrix m)
        {
            return TransformNormal(v, ref m);
        }

        #endregion

        #region Methods Overriding

        public override bool Equals(object obj)
        {
            Vector3? other = obj as Vector3?;
            if (other == null)
                return false;

            return Equals(other.Value);
        }

        public bool Equals(Vector3 v)
        {
            return this == v;
        }

        public override int GetHashCode()
        {
            return (X + Y + Z).GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder()
                .Append("{").Append(X.ToString(ElementFormat, null))
                .Append("; ").Append(Y.ToString(ElementFormat, null))
                .Append("; ").Append(Z.ToString(ElementFormat, null))
                .Append("}");

            return result.ToString();
        }

        #endregion

        #region Operators Overloading

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public static Vector3 operator +(Vector3 v)
        {
            return v;
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator *(Vector3 v, float scalar)
        {
            return new Vector3(scalar * v.X, scalar * v.Y, scalar * v.Z);
        }

        public static Vector3 operator *(float scalar, Vector3 v)
        {
            return new Vector3(scalar * v.X, scalar * v.Y, scalar * v.Z);
        }

        public static Vector3 operator /(Vector3 v, float scalar)
        {
            return new Vector3(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }

        #endregion
    }
}
