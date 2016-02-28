using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericMathematics
{
	public struct Vector3<T> : IFormattable, IEquatable<Vector3<T>>, IComparable<Vector3<T>>
	{
		static readonly Math<T> math = Math<T>.Default;
		static readonly T Two = Math<T>.Default.FromInt32(2);

		public static readonly Vector3<T> Zero;
		public static readonly Vector3<T> UnitX;
		public static readonly Vector3<T> UnitY;
		public static readonly Vector3<T> UnitZ;

		public T X;
		public T Y;
		public T Z;

		static Vector3()
		{
			T zero = math.FromInt32(0);
			T one = math.FromInt32(1);

			Zero = new Vector3<T>(zero, zero, zero);
			UnitX = new Vector3<T>(one, zero, zero);
			UnitY = new Vector3<T>(zero, one, zero);
			UnitZ = new Vector3<T>(zero, zero, one);
		}

		public Vector3(T value)
			: this(value, value, value)
		{
		}

		public Vector3(Vector2<T> v, T z)
			: this(v.X, v.Y, z)
		{
		}

		public Vector3(T x, T y, T z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		#region Methods Overriding

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Vector3<T>))
				return false;

			Vector3<T> other = (Vector3<T>)obj;
			return this.Equals(other);
		}

		public bool Equals(Vector3<T> other)
		{
			return math.Equals(this.X, other.X)
				&& math.Equals(this.Y, other.Y)
				&& math.Equals(this.Z, other.Z);
		}

		public int CompareTo(Vector3<T> other)
		{
			int xCompare = math.Compare(X, other.X);
			if (xCompare != 0)
				return xCompare > 0 ? 1 : -1;

			int yCompare = math.Compare(Y, other.Y);
			if (yCompare != 0)
				return yCompare > 0 ? 1 : -1;

			int zCompare = math.Compare(Z, other.Z);
			if (zCompare != 0)
				return zCompare > 0 ? 1 : -1;

			return 0;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
		}

		public override string ToString()
		{
			return ToString(null, null);
		}

		public string ToString(string format)
		{
			return ToString(format, null);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			string formattedX;
			string formattedY;
			string formattedZ;

			if (typeof(IFormattable).IsAssignableFrom(typeof(T)))
			{
				formattedX = ((IFormattable)X).ToString(format, formatProvider);
				formattedY = ((IFormattable)Y).ToString(format, formatProvider);
				formattedZ = ((IFormattable)Z).ToString(format, formatProvider);
			}
			else
			{
				formattedX = X.ToString();
				formattedY = Y.ToString();
				formattedZ = Z.ToString();
			}

			return string.Format("{{{0}; {1}; {2}}}", formattedX, formattedY, formattedZ);
		}

		#endregion

		#region Operations

		public T Length()
		{
			return math.Sqrt(math.Add(math.Add(
				math.Pow(X, Two),
				math.Pow(Y, Two)),
				math.Pow(Z, Two)));
		}

		public T LengthSquared()
		{
			return math.Add(math.Add(
				math.Pow(X, Two),
				math.Pow(Y, Two)),
				math.Pow(Z, Two));
		}

		public Vector3<T> Normalize()
		{
			T length = Length();
			if (math.Equals(length, math.Zero))
				return Vector3<T>.Zero;

			return this.Divide(length);
		}

		public Vector3<T> Negate()
		{
			return new Vector3<T>(
				math.Negate(this.X), math.Negate(this.Y), math.Negate(this.Z));
		}

		public Vector3<T> Add(Vector3<T> v)
		{
			return new Vector3<T>(
				math.Add(this.X, v.X), math.Add(this.Y, v.Y), math.Add(this.Z, v.Z));
		}

		public Vector3<T> Subtract(Vector3<T> v)
		{
			return new Vector3<T>(
				math.Subtract(this.X, v.X), math.Subtract(this.Y, v.Y), math.Subtract(this.Z, v.Z));
		}

		public Vector3<T> Multiply(T scalar)
		{
			return new Vector3<T>(
				math.Multiply(this.X, scalar), math.Multiply(this.Y, scalar), math.Multiply(this.Z, scalar));
		}

		public Vector3<T> Divide(T scalar)
		{
			return new Vector3<T>(
				math.Divide(this.X, scalar), math.Divide(this.Y, scalar), math.Divide(this.Z, scalar));
		}

		public T Dot(Vector3<T> v)
		{
			return math.Add(math.Add(
				math.Multiply(X, v.X),
				math.Multiply(Y, v.Y)),
				math.Multiply(Z, v.Z));
		}

		public Vector3<T> Cross(Vector3<T> v)
		{
			return new Vector3<T>(
				math.Subtract(math.Multiply(Y, v.Z), math.Multiply(Z, v.Y)),
				math.Subtract(math.Multiply(Z, v.X), math.Multiply(X, v.Z)),
				math.Subtract(math.Multiply(X, v.Y), math.Multiply(Y, v.X)));
		}

		public Vector3<T> Clamp(Vector3<T> min, Vector3<T> max)
		{
			T x = X;
			x = math.GreaterThan(x, max.X) ? max.X : x;
			x = math.LessThan(x, min.X) ? min.X : x;

			T y = Y;
			y = math.GreaterThan(y, max.Y) ? max.Y : y;
			y = math.LessThan(y, min.Y) ? min.Y : y;

			T z = Z;
			z = math.GreaterThan(z, max.Z) ? max.Z : z;
			z = math.LessThan(z, min.Z) ? min.Z : z;

			return new Vector3<T>(x, y, z);
		}

		public Vector3<T> Transform3D(Matrix<T> transform)
		{
			if (transform.Rows != 4 ||
				transform.Columns != 4)
			{
				throw new ArgumentException(TextResources.Vector_Transform3DMatrixSize);
			}

			return new Vector3<T>(
				math.Add(
					math.Add(
						math.Multiply(X, transform[0, 0]),
						math.Multiply(Y, transform[1, 0])),
					math.Add(
						math.Multiply(Z, transform[2, 0]),
						transform[3, 0])),
				math.Add(
					math.Add(
						math.Multiply(X, transform[0, 1]),
						math.Multiply(Y, transform[1, 1])),
					math.Add(
						math.Multiply(Z, transform[2, 1]),
						transform[3, 1])),
				math.Add(
					math.Add(
						math.Multiply(X, transform[0, 2]),
						math.Multiply(Y, transform[1, 2])),
					math.Add(
						math.Multiply(Z, transform[2, 2]),
						transform[3, 2])));
		}

		public Vector3<T> TransformNormal(Matrix<T> transform)
		{
			if (transform.Rows < 3 ||
				transform.Columns < 3)
			{
				throw new ArgumentException(TextResources.Vector3_TransformNormalMatrixSize);
			}

			return new Vector3<T>(
				math.Add(math.Add(
						math.Multiply(X, transform[0, 0]),
						math.Multiply(Y, transform[1, 0])),
						math.Multiply(Z, transform[2, 0])),
				math.Add(math.Add(
						math.Multiply(X, transform[0, 1]),
						math.Multiply(Y, transform[1, 1])),
						math.Multiply(Z, transform[2, 1])),
				math.Add(math.Add(
						math.Multiply(X, transform[0, 2]),
						math.Multiply(Y, transform[1, 2])),
						math.Multiply(Z, transform[2, 2])));
		}

		#endregion

		#region Operators Overloading

		public static bool operator ==(Vector3<T> v1, Vector3<T> v2)
		{
			return v1.Equals(v2);
		}

		public static bool operator !=(Vector3<T> v1, Vector3<T> v2)
		{
			return !v1.Equals(v2);
		}

		public static Vector3<T> operator +(Vector3<T> v)
		{
			return v;
		}

		public static Vector3<T> operator -(Vector3<T> v)
		{
			return v.Negate();
		}

		public static Vector3<T> operator +(Vector3<T> v1, Vector3<T> v2)
		{
			return v1.Add(v2);
		}

		public static Vector3<T> operator -(Vector3<T> v1, Vector3<T> v2)
		{
			return v1.Subtract(v2);
		}

		public static Vector3<T> operator *(Vector3<T> v, T scalar)
		{
			return v.Multiply(scalar);
		}

		public static Vector3<T> operator *(T scalar, Vector3<T> v)
		{
			return v.Multiply(scalar);
		}

		public static Vector3<T> operator /(Vector3<T> v, T scalar)
		{
			return v.Divide(scalar);
		}

		#endregion
	}
    
	internal class Vector3Math<T> : Math<Vector3<T>>
	{
		Math<T> math = Math<T>.Default;

		public override bool IsFloatingPoint { get { return math.IsFloatingPoint; } }
		public override bool IsFractional { get { return math.IsFractional; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool HasBounds { get { return math.HasBounds; } }
		public override bool IsUnsigned { get { return math.IsUnsigned; } }
		public override Vector3<T> MinValue { get { return new Vector3<T>(math.MinValue); } }
		public override Vector3<T> MaxValue { get { return new Vector3<T>(math.MaxValue); } }
		public override Vector3<T> Zero { get { return Vector3<T>.Zero; } }
		public override Vector3<T> NaN { get { return new Vector3<T>(math.NaN); } }
		public override Vector3<T> PositiveInfinity { get { return new Vector3<T>(math.PositiveInfinity); } }
		public override Vector3<T> NegativeInfinity { get { return new Vector3<T>(math.NegativeInfinity); } }
		public override bool IsInfinite(Vector3<T> value) { return math.IsInfinite(value.X) || math.IsInfinite(value.Y) || math.IsInfinite(value.Z); }
		public override bool IsNaN(Vector3<T> value) { return math.IsNaN(value.X) || math.IsNaN(value.Y) || math.IsNaN(value.Z); }
		public override Vector3<T> FromInt32(int value) { return new Vector3<T>(math.FromInt32(value), math.Zero, math.Zero); }
		public override int ToInt32(Vector3<T> value) { return math.ToInt32(value.X); }
		public override int Compare(Vector3<T> x, Vector3<T> y) { return x.CompareTo(y); }
		public override bool Equals(Vector3<T> x, Vector3<T> y) { return x.Equals(y); }
		public override Vector3<T> Add(Vector3<T> x, Vector3<T> y) { return x.Add(y); }
		public override Vector3<T> Subtract(Vector3<T> x, Vector3<T> y) { return x.Subtract(y); }
		public override Vector3<T> Negate(Vector3<T> value) { return value.Negate(); }
		public override Vector3<T> Multiply(Vector3<T> x, Vector3<T> y) { return new Vector3<T>(x.Dot(y), math.Zero, math.Zero); }
		public override Vector3<T> DivideIntegralModulus(Vector3<T> x, Vector3<T> y, out Vector3<T> modulus) { throw new NotSupportedException(); }
		public override Vector3<T> Divide(Vector3<T> x, Vector3<T> y) { throw new NotSupportedException(); }
		public override Vector3<T> Reciprocal(Vector3<T> value) { throw new NotSupportedException(); }
		public override Vector3<T> Predecessor(Vector3<T> value) { return new Vector3<T>(math.Predecessor(value.X), value.Y, value.Z); }
		public override Vector3<T> Successor(Vector3<T> value) { return new Vector3<T>(math.Successor(value.X), value.Y, value.Z); }
		public override Vector3<T> Sign(Vector3<T> value) { return value.Normalize(); }
		public override Vector3<T> E { get { return new Vector3<T>(math.E, math.Zero, math.Zero); } }
		public override Vector3<T> Pi { get { return new Vector3<T>(math.Pi, math.Zero, math.Zero); } }
		public override Vector3<T> Abs(Vector3<T> value) { return new Vector3<T>(value.Length(), math.Zero, math.Zero); }
		public override Vector3<T> Truncate(Vector3<T> value)
			{ return new Vector3<T>(math.Truncate(value.X), math.Truncate(value.Y), math.Truncate(value.Z)); }
		public override Vector3<T> Round(Vector3<T> value, int decimalPlaces, MidpointRounding mode)
			{ return new Vector3<T>(math.Round(value.X, decimalPlaces, mode), math.Round(value.Y, decimalPlaces, mode), math.Round(value.Z, decimalPlaces, mode)); }
		public override Vector3<T> Round(Vector3<T> value, MidpointRounding mode)
			{ return new Vector3<T>(math.Round(value.X, mode), math.Round(value.Y, mode), math.Round(value.Z, mode)); }
		public override Vector3<T> Ceiling(Vector3<T> value)
			{ return new Vector3<T>(math.Ceiling(value.X), math.Ceiling(value.Y), math.Ceiling(value.Z)); }
		public override Vector3<T> Floor(Vector3<T> value)
			{ return new Vector3<T>(math.Floor(value.X), math.Floor(value.Y), math.Floor(value.Z)); }
	}
}
