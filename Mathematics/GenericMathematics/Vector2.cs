using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GenericMathematics
{
	public struct Vector2<T> : IFormattable, IEquatable<Vector2<T>>, IComparable<Vector2<T>>
	{
		static readonly Math<T> math = Math<T>.Default;
		static readonly T Two = Math<T>.Default.FromInt32(2);

		public static readonly Vector2<T> Zero;
		public static readonly Vector2<T> UnitX;
		public static readonly Vector2<T> UnitY;

		public T X;
		public T Y;

		static Vector2()
		{
			T zero = math.FromInt32(0);
			T one = math.FromInt32(1);

			Zero = new Vector2<T>(zero, zero);
			UnitX = new Vector2<T>(one, zero);
			UnitY = new Vector2<T>(zero, one);
		}

		public Vector2(T value)
			: this(value, value)
		{
		}

		public Vector2(T x, T y)
		{
			X = x;
			Y = y;
		}

		#region Methods Overriding

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Vector2<T>))
				return false;

			Vector2<T> other = (Vector2<T>)obj;
			return this.Equals(other);
		}

		public bool Equals(Vector2<T> other)
		{
			return math.Equals(this.X, other.X) && math.Equals(this.Y, other.Y);
		}

		public int CompareTo(Vector2<T> other)
		{
			int xCompare = math.Compare(X, other.X);
			if (xCompare != 0)
				return xCompare > 0 ? 1 : -1;

			int yCompare = math.Compare(Y, other.Y);
			if (yCompare != 0)
				return yCompare > 0 ? 1 : -1;

			return 0;
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() ^ Y.GetHashCode();
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

			if (typeof(IFormattable).IsAssignableFrom(typeof(T)))
			{
				formattedX = ((IFormattable)X).ToString(format, formatProvider);
				formattedY = ((IFormattable)Y).ToString(format, formatProvider);
			}
			else
			{
				formattedX = X.ToString();
				formattedY = Y.ToString();
			}

			return string.Format("{{{0}; {1}}}", formattedX, formattedY);
		}

		#endregion

		#region Operations

		public T Length()
		{
			return math.Sqrt(math.Add(math.Pow(X, Two), math.Pow(Y, Two)));
		}

		public T LengthSquared()
		{
			return math.Add(math.Pow(X, Two), math.Pow(Y, Two));
		}

		public Vector2<T> Normalize()
		{
			T length = Length();
			if (math.Equals(length, math.Zero))
				return Vector2<T>.Zero;

			return this.Divide(length);
		}

		public Vector2<T> Negate()
		{
			return new Vector2<T>(math.Negate(this.X), math.Negate(this.Y));
		}

		public Vector2<T> Add(Vector2<T> v)
		{
			return new Vector2<T>(math.Add(this.X, v.X), math.Add(this.Y, v.Y));
		}

		public Vector2<T> Subtract(Vector2<T> v)
		{
			return new Vector2<T>(math.Subtract(this.X, v.X), math.Subtract(this.Y, v.Y));
		}

		public Vector2<T> Multiply(T scalar)
		{
			return new Vector2<T>(math.Multiply(this.X, scalar), math.Multiply(this.Y, scalar));
		}

		public Vector2<T> Divide(T scalar)
		{
			return new Vector2<T>(math.Divide(this.X, scalar), math.Divide(this.Y, scalar));
		}

		public T Dot(Vector2<T> v)
		{
			return math.Add(math.Multiply(X, v.X), math.Multiply(Y, v.Y));
		}

		public Vector2<T> Clamp(Vector2<T> min, Vector2<T> max)
		{
			T x = X;
			x = math.GreaterThan(x, max.X) ? max.X : x;
			x = math.LessThan(x, min.X) ? min.X : x;

			T y = Y;
			y = math.GreaterThan(y, max.Y) ? max.Y : y;
			y = math.LessThan(y, min.Y) ? min.Y : y;

			return new Vector2<T>(x, y);
		}

		public Vector2<T> Transform3D(Matrix<T> transform)
		{
			if (transform.Rows != 4 ||
				transform.Columns != 4)
			{
				throw new ArgumentException(TextResources.Vector_Transform3DMatrixSize);
			}

			return new Vector2<T>(
				math.Add(math.Add(
					math.Multiply(X, transform[0, 0]),
					math.Multiply(Y, transform[1, 0])),
					transform[3, 0]),
				math.Add(math.Add(
					math.Multiply(X, transform[0, 1]),
					math.Multiply(Y, transform[1, 1])),
					transform[3, 1]));
		}

		public Vector2<T> TransformNormal(Matrix<T> transform)
		{
			if (transform.Rows < 2 ||
				transform.Columns < 2)
			{
				throw new ArgumentException(TextResources.Vector2_TransformNormalMatrixSize);
			}

			return new Vector2<T>(
				math.Add(
					math.Multiply(X, transform[0, 0]),
					math.Multiply(Y, transform[0, 1])),
				math.Add(
					math.Multiply(X, transform[1, 0]),
					math.Multiply(Y, transform[1, 1])));
		}

		#endregion

		#region Operators Overloading

		public static bool operator ==(Vector2<T> v1, Vector2<T> v2)
		{
			return v1.Equals(v2);
		}

		public static bool operator !=(Vector2<T> v1, Vector2<T> v2)
		{
			return !v1.Equals(v2);
		}

		public static Vector2<T> operator +(Vector2<T> v)
		{
			return v;
		}

		public static Vector2<T> operator -(Vector2<T> v)
		{
			return v.Negate();
		}

		public static Vector2<T> operator +(Vector2<T> v1, Vector2<T> v2)
		{
			return v1.Add(v2);
		}

		public static Vector2<T> operator -(Vector2<T> v1, Vector2<T> v2)
		{
			return v1.Subtract(v2);
		}

		public static Vector2<T> operator *(Vector2<T> v, T scalar)
		{
			return v.Multiply(scalar);
		}

		public static Vector2<T> operator *(T scalar, Vector2<T> v)
		{
			return v.Multiply(scalar);
		}

		public static Vector2<T> operator /(Vector2<T> v, T scalar)
		{
			return v.Divide(scalar);
		}

		#endregion
	}
    
	internal class Vector2Math<T> : Math<Vector2<T>>
	{
		Math<T> math = Math<T>.Default;

		public override bool IsFloatingPoint { get { return math.IsFloatingPoint; } }
		public override bool IsFractional { get { return math.IsFractional; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool HasBounds { get { return math.HasBounds; } }
		public override bool IsUnsigned { get { return math.IsUnsigned; } }
		public override Vector2<T> MinValue { get { return new Vector2<T>(math.MinValue); } }
		public override Vector2<T> MaxValue { get { return new Vector2<T>(math.MaxValue); } }
		public override Vector2<T> Zero { get { return Vector2<T>.Zero; } }
		public override Vector2<T> NaN { get { return new Vector2<T>(math.NaN); } }
		public override Vector2<T> PositiveInfinity { get { return new Vector2<T>(math.PositiveInfinity); } }
		public override Vector2<T> NegativeInfinity { get { return new Vector2<T>(math.NegativeInfinity); } }
		public override bool IsInfinite(Vector2<T> value) { return math.IsInfinite(value.X) || math.IsInfinite(value.Y); }
		public override bool IsNaN(Vector2<T> value) { return math.IsNaN(value.X) || math.IsNaN(value.Y); }
		public override Vector2<T> FromInt32(int value) { return new Vector2<T>(math.FromInt32(value), math.Zero); }
		public override int ToInt32(Vector2<T> value) { return math.ToInt32(value.X); }
		public override int Compare(Vector2<T> x, Vector2<T> y) { return x.CompareTo(y); }
		public override bool Equals(Vector2<T> x, Vector2<T> y) { return x.Equals(y); }
		public override Vector2<T> Add(Vector2<T> x, Vector2<T> y) { return x.Add(y); }
		public override Vector2<T> Subtract(Vector2<T> x, Vector2<T> y) { return x.Subtract(y); }
		public override Vector2<T> Negate(Vector2<T> value) { return value.Negate(); }
		public override Vector2<T> Multiply(Vector2<T> x, Vector2<T> y) { return new Vector2<T>(x.Dot(y), math.Zero); }
		public override Vector2<T> DivideIntegralModulus(Vector2<T> x, Vector2<T> y, out Vector2<T> modulus) { throw new NotSupportedException(); }
		public override Vector2<T> Divide(Vector2<T> x, Vector2<T> y) { throw new NotSupportedException(); }
		public override Vector2<T> Reciprocal(Vector2<T> value) { throw new NotSupportedException(); }
		public override Vector2<T> Predecessor(Vector2<T> value) { return new Vector2<T>(math.Predecessor(value.X), value.Y); }
		public override Vector2<T> Successor(Vector2<T> value) { return new Vector2<T>(math.Successor(value.X), value.Y); }
		public override Vector2<T> Sign(Vector2<T> value) { return value.Normalize(); }
		public override Vector2<T> E { get { return new Vector2<T>(math.E, math.Zero); } }
		public override Vector2<T> Pi { get { return new Vector2<T>(math.Pi, math.Zero); } }
		public override Vector2<T> Abs(Vector2<T> value) { return new Vector2<T>(value.Length(), math.Zero); }
		public override Vector2<T> Truncate(Vector2<T> value)
			{ return new Vector2<T>(math.Truncate(value.X), math.Truncate(value.Y)); }
		public override Vector2<T> Round(Vector2<T> value, int decimalPlaces, MidpointRounding mode)
			{ return new Vector2<T>(math.Round(value.X, decimalPlaces, mode), math.Round(value.Y, decimalPlaces, mode)); }
		public override Vector2<T> Round(Vector2<T> value, MidpointRounding mode)
			{ return new Vector2<T>(math.Round(value.X, mode), math.Round(value.Y, mode)); }
		public override Vector2<T> Ceiling(Vector2<T> value)
			{ return new Vector2<T>(math.Ceiling(value.X), math.Ceiling(value.Y)); }
		public override Vector2<T> Floor(Vector2<T> value)
			{ return new Vector2<T>(math.Floor(value.X), math.Floor(value.Y)); }
	}
}
