using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GenericMathematics
{
	/// <summary>
	/// Represents general mathematical operations on standart numerical types and
	/// numerical types from this library.
	/// </summary>
	/// <typeparam name="T">The numerical type to use in calculations.</typeparam>
	public abstract class Math<T> : IComparer<T>, IEqualityComparer<T>
	{
        private static Lazy<Math<T>> instance = new Lazy<Math<T>>(() => MathManager.Instance.GetMath<T>());

		/// <summary>
		/// Gets the math associated with the type <typeparamref name="T"/>.
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public static Math<T> Default
		{
			get { return instance.Value; }
		}

		/// <summary>
		/// Initialize a new instance of <see cref="T:GenericMathematics.Math`1"/>.
		/// </summary>
		protected Math()
		{
		}

		/// <summary>
		/// Describes the general features of the number type.
		/// </summary>
		public virtual MathFeatures Features
		{
			get
			{
				return 
					(HasBounds ? MathFeatures.Bounded : 0) |
					(IsUnsigned ? MathFeatures.Unsigned : 0) |
					(IsTwosComplement ? MathFeatures.TwosComplement : 0) |
					(IsFractional ? MathFeatures.Fractional : 0) |
					(IsFloatingPoint ? MathFeatures.FloatingPoint : 0);
			}
		}

		/// <summary>
		/// Returns whether <typeparamref name="T"/> is unsigned
		/// (i.e. <see cref="UInt16"/>, <see cref="UInt32"/>, etc).
		/// </summary>
		public virtual bool IsUnsigned
		{
			get { return false; }
		}

		/// <summary>
		/// Returns whether <typeparamref name="T"/> is signed and has more
		/// negative values than positive values.
		/// </summary>
		public virtual bool IsTwosComplement
		{
			get { return false; }
		}

		/// <summary>
		/// Returns whether <typeparamref name="T"/> is fractional
		/// (<see cref="Single"/>, <see cref="Double"/>, <see cref="Decimal"/>, etc).
		/// </summary>
		public virtual bool IsFractional
		{
			get { return false; }
		}

		/// <summary>
		/// Return whether <typeparamref name="T"/> is floating point number
		/// (Single, Double, etc), supports <see cref="IsInfinite"/>, <see cref="IsNaN"/>, etc.
		/// </summary>
		public virtual bool IsFloatingPoint
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the zero value for <typeparamref name="T"/> (equals to <c>FromInt32(0)</c>).
		/// </summary>
		public virtual T Zero
		{
			get { return FromInt32(0); }
		}

		#region IComparer<T> and IEqualityComparer<T>

		/// <summary>
		/// Compares the first value with second and returns an integer that indicates
		/// whether the first value precedes, follows, or occurs in the same position
		/// in the sort order as the second.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>
		/// negative integer if <paramref name="x"/> &lt; <paramref name="y"/>,
		/// zero integer if <paramref name="x"/> == <paramref name="y"/>,
		/// positive integer if <paramref name="x"/> &gt; <paramref name="y"/>.
		/// </returns>
		public virtual int Compare(T x, T y)
		{
			return Comparer<T>.Default.Compare(x, y);
		}

		/// <summary>
		/// Indicates whether the first value is equal to the second.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>True if the first is equal to the second; otherwise, false.</returns>
		public virtual bool Equals(T x, T y)
		{
			return EqualityComparer<T>.Default.Equals(x, y);
		}

		/// <summary>
		/// Returns the hash code for the value, which is constant for equals values.
		/// </summary>
		/// <param name="value">The value to hash.</param>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public virtual int GetHashCode(T value)
		{
			return EqualityComparer<T>.Default.GetHashCode(value);
		}

		#endregion

		#region Comparing, Min and Max

		/// <summary>
		/// Compares the first value with the second.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns><c>true</c> if the first value less than the second; <c>false</c> otherwise.</returns>
		public virtual bool LessThan(T x, T y)
		{
			var c = Compare(x, y);
			if (c < 0)
				return true;
			return false;
		}

		/// <summary>
		/// Compares the first value with the second.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns><c>true</c> if the first value less than or equals the second; <c>false</c> otherwise.</returns>
		public virtual bool LessThanOrEqual(T x, T y)
		{
			var c = Compare(x, y);
			if (c <= 0)
				return true;
			return false;
		}

		/// <summary>
		/// Compares the first value with the second.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns><c>true</c> if the first value greater than the second; <c>false</c> otherwise.</returns>
		public virtual bool GreaterThan(T x, T y)
		{
			var c = Compare(x, y);
			if (c > 0)
				return true;
			return false;
		}

		/// <summary>
		/// Compares the first value with the second.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>
		/// <c>true</c> if the first value greater than or equals than the second;
		/// <c>false</c> otherwise.
		/// </returns>
		public virtual bool GreaterThanOrEqual(T x, T y)
		{
			var c = Compare(x, y);
			if (c >= 0)
				return true;
			return false;
		}

		/// <summary>
		/// Returns the maximum of two values.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The first value if <paramref name="x"/> &lt;= <paramref name="y"/>; otherwise the second.</returns>
		public virtual T Max(T x, T y)
		{
			var c = Compare(x, y);
			return c >= 0 ? x : y;
		}

		/// <summary>
		/// Returns the minimum of two values.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The first value if <paramref name="x"/> &gt;= <paramref name="y"/>; otherwise the second.</returns>
		public virtual T Min(T x, T y)
		{
			var c = Compare(x, y);
			return c <= 0 ? x : y;
		}

		#endregion

		#region Successor, Predecessor, Int32, Enumerate

		/// <summary>
		/// Returns the <c>value + 1</c> for the given value.
		/// </summary>
		/// <param name="value">Numerical value.</param>
		/// <returns>The <c>value + FromInt32(1)</c> for the value.</returns>
		public virtual T Successor(T value)
		{
			return Add(value, FromInt32(1));
		}

		/// <summary>
		/// Returns the <c>value - 1</c> for the given value.
		/// </summary>
		/// <param name="value">Numerical value.</param>
		/// <returns>The <c>value - FromInt32(1)</c> for the value.</returns>
		public virtual T Predecessor(T value)
		{
			return Subtract(value, FromInt32(1));
		}

		/// <summary>
		/// Gets the representation of <see cref="Int32"/> value in <typeparamref name="T"/>.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> value.</param>
		/// <returns>Value of <typeparamref name="T"/> that corresponds to given integer value.</returns>
		public virtual T FromInt32(int value)
		{
			return (T)ValueConverter.Convert(value, typeof(T));
		}

		/// <summary>
		/// Gets the representation of <typeparamref name="T"/> value in <see cref="Int32"/>.
		/// </summary>
		/// <param name="value"><typeparamref name="T"/> value.</param>
		/// <returns>Value of <see cref="Int32"/> that corresponds to given <typeparamref name="T"/> value.</returns>
		public virtual int ToInt32(T value)
		{
			return (int)ValueConverter.Convert(value, typeof(Int32));
		}

		/// <summary>
		/// Generates a sequence of values <c>value, Successor(value), Successor(Successor(value), ...</c>
		/// from the initial value.
		/// </summary>
		/// <param name="start">The initial value of the sequence.</param>
		/// <returns>The infinite sequence <c>value, value + 1, ...</c> that counts from the specified value.</returns>
		public virtual IEnumerable<T> EnumerateFrom(T start)
		{
			return Iterate(start, v => Successor(v));
		}

		/// <summary>
		/// Generates a sequence of values <c>value, Successor(value), Successor(Successor(value), ...</c>
		/// from the initial value, that contains values that less than end value.
		/// </summary>
		/// <param name="start">The initial value of the sequence.</param>
		/// <param name="end">The excluded end value of the sequence.</param>
		/// <returns>
		/// The sequence <c>value, value + 1, ..., last</c> that counts from the specified value,
		/// where last is the maximal value, that less than end.
		/// </returns>
		public virtual IEnumerable<T> EnumerateFromTo(T start, T end)
		{
			return EnumerateFrom(start).TakeWhile(v => LessThan(v, end));
		}

		#endregion

		#region Bounds

		/// <summary>
		/// Returns whether <typeparamref name="T"/> has <see cref="MinValue"/> and <see cref="MaxValue"/>.
		/// </summary>
		public virtual bool HasBounds
		{
			get { return false; }
		}

		/// <summary>
		/// Returns the minimal value of <typeparamref name="T"/>.
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T MinValue
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Returns the maximal value of <typeparamref name="T"/>.
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T MaxValue
		{
			get { throw new NotSupportedException(); }
		}

		#endregion

		#region Arithmetics

		/// <summary>
		/// Calculates the sum of two values.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The sum of first value and the second.</returns>
		public abstract T Add(T x, T y);

		/// <summary>
		/// Calculates the product of two values.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The product of first value and the second.</returns>
		public abstract T Multiply(T x, T y);

		/// <summary>
		/// Calculates the difference between two values.
		/// </summary>
		/// <param name="x">The first value.</param>
		/// <param name="y">The second value.</param>
		/// <returns>The difference between the first value and the second.</returns>
		public abstract T Subtract(T x, T y);

		/// <summary>
		/// Returns the negated value of the given value.
		/// </summary>
		/// <param name="value">The numerical value to negate.</param>
		/// <returns>The negated value <c>-value</c>.</returns>
		public virtual T Negate(T value)
		{
			return Subtract(FromInt32(0), value);
		}

		/// <summary>
		/// Returns the absolute value of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>
		/// The absolute value of the value
		/// (or length of vector with complex or vector types).
		/// </returns>
		public virtual T Abs(T value)
		{
			if (LessThan(value, FromInt32(0)))
				return Negate(value);
			return value;
		}

		/// <summary>
		/// Return the sign of the value. For scalar types it is unit value
		/// with sign or zero, for vector or complex types it is the value
		/// with the same direction and unit length of vector or zero vector.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>
		/// For scalar types 1, -1 if <paramref name="value"/> != 0, else 0;
		/// for complex or vector types it is <c>value / Abs(value)</c>.
		/// </returns>
		public virtual T Sign(T value)
		{
			var zero = FromInt32(0);
			if (Equals(zero, value))
				return zero;
			if (LessThan(value, zero))
				return FromInt32(-1);
			return FromInt32(1);
		}

		#endregion

		#region Division, ToDouble, FromDouble

		/// <summary>
		/// Calculates rounded to -inf quotient of division the dividend by the divisor.
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <returns>Rounded to -inf quotient of division the dividend by the divisor.</returns>
		/// <exception cref="System.DivideByZeroException"/>
		/// <exception cref="System.OverflowException"/>
		public virtual T Quotient(T x, T y)
		{
			if (Equals(y, Zero))
				throw new DivideByZeroException();

			if (!IsUnsigned && HasBounds &&
				Equals(y, FromInt32(-1)) && Equals(x, MinValue))
			{
				throw new OverflowException();
			}

			T modulus;
			T roundedTowardsZeroQuotient = DivideIntegralModulus(x, y, out modulus);
			bool dividedEvenly = Equals(modulus, Zero);
			if (dividedEvenly)
				return roundedTowardsZeroQuotient;

			// At this point we know that divisor was not zero 
			// (because we would have thrown) and we know that 
			// dividend was not zero (because there would have been no remainder)
			// Therefore both are non-zero.  Either they are of the same sign, 
			// or opposite signs. If they're of opposite sign then we rounded 
			// UP towards zero so we need to subtract one. If they're of the same sign 
			// then we rounded DOWN towards zero so we're done.

			bool wasRoundedDown = GreaterThan(x, Zero) == GreaterThan(y, Zero);
			if (wasRoundedDown)
				return roundedTowardsZeroQuotient;
			else
				return Predecessor(roundedTowardsZeroQuotient);
		}

		/// <summary>
		/// Calculates the remainder of division the dividend by the divisor.
		/// The remainder is always positive when y &gt; 0 ("mathematical remainder")
		/// otherwise when y &lt; 0 <c>Remainder(x, y) = -Remainder(x, Abs(y))</c>.
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <returns>
		/// The remainder of division the dividend by the divisor.
		/// The value is calculated as <c>x - Quotient(x, y) * y</c>.
		/// </returns>
		/// <exception cref="System.DivideByZeroException"/>
		/// <exception cref="System.OverflowException"/>
		public virtual T Remainder(T x, T y)
		{
			T remainder;
			QuotientRemainder(x, y, out remainder);
			return remainder;
		}

		/// <summary>
		/// Calculates rounded to zero quotient of division the dividend by the divisor.
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <returns>Rounded to zero quotient of division the dividend by the divisor.</returns>
		/// <exception cref="System.DivideByZeroException"/>
		/// <exception cref="System.OverflowException"/>
		public virtual T DivideIntegral(T x, T y)
		{
			T modulus;
			return DivideIntegralModulus(x, y, out modulus);
		}

		/// <summary>
		/// Calculates the remainder of division the dividend by the divisor.
		/// The remainder is negative if and only if dividend is negative ("computer remainder").
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <returns>
		/// The remainder of division the dividend by the divisor.
		/// The value is calculated as <c>x - Sign(x) * DivideIntegral(Abs(x), Abs(y)) * Abs(y)</c>.
		/// </returns>
		/// <exception cref="System.DivideByZeroException"/>
		/// <exception cref="System.OverflowException"/>
		public virtual T Modulus(T x, T y)
		{
			T modulus;
			DivideIntegralModulus(x, y, out modulus);
			return modulus;
		}

		/// <summary>
		/// Calculates pair of (<c>Quotient(x, y)</c>, <c>Remainder(x, y)</c>) of the division
		/// (if y &gt; 0 then "mathematical division").
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <param name="remainder">The resulting remainder.</param>
		/// <returns>The rounded to -inf quotient of division the dividend by the divisor</returns>
		/// <exception cref="System.DivideByZeroException"/>
		/// <exception cref="System.OverflowException"/>
		public virtual T QuotientRemainder(T x, T y, out T remainder)
		{
			T quotient = Quotient(x, y);
			remainder = Subtract(x, Multiply(quotient, y));
			return quotient;
		}

		/// <summary>
		/// Calculates pair of (<c>DivideIntegral(x, y)</c>, <c>Modulus(x, y)</c>) of the division
		/// ("computer division").
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <param name="modulus">The resulting modulus.</param>
		/// <returns>The rounded to zero quotient of division the dividend by the divisor</returns>
		/// <exception cref="System.DivideByZeroException"/>
		/// <exception cref="System.OverflowException"/>
		public abstract T DivideIntegralModulus(T x, T y, out T modulus);

		/// <summary>
		/// Calculate the division of two values.
		/// </summary>
		/// <param name="x">The dividend.</param>
		/// <param name="y">The divisor.</param>
		/// <returns>
		/// The <c>DivideIntegral(x, y)</c> if <typeparamref name="T"/> is integer, otherwise <c>x / y</c>.
		/// </returns>
		/// <exception cref="System.DivideByZeroException"/>
		/// <exception cref="System.OverflowException"/>
		public virtual T Divide(T x, T y)
		{
			return DivideIntegral(x, y);
		}

		/// <summary>
		/// Returns the reciprocal (<c>1 / x</c>) of value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The result that equals to <c>Divide(1, x)</c>.</returns>
		public virtual T Reciprocal(T value)
		{
			return Divide(FromInt32(1), value);
		}

		/// <summary>
		/// Gets the representation of <see cref="System.Double"/> value as <typeparamref name="T"/>.
		/// </summary>
		/// <param name="value"><see cref="System.Double"/> value.</param>
		/// <returns>
		/// <typeparamref name="T"/> value that corresponds to
		/// given <see cref="System.Double"/> value.
		/// </returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T FromDouble(double value)
		{
			return (T)ValueConverter.Convert(value, typeof(T));
		}

		/// <summary>
		/// Gets the representation of <typeparamref name="T"/> value as <see cref="System.Double"/>.
		/// </summary>
		/// <param name="value"><typeparamref name="T"/> value.</param>
		/// <returns>
		/// <see cref="System.Double"/> value that corresponds to
		/// given <typeparamref name="T"/> value.
		/// </returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual double ToDouble(T value)
		{
			return (double)ValueConverter.Convert(value, typeof(double));
		}

		#endregion

		#region Floating Functions and Values

		/// <summary>
		/// Returns the value of Pi.
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Pi
		{
			get { return FromDouble(Math.PI); }
		}

		/// <summary>
		/// Returns the value of E (natural logarithm base).
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T E
		{
			get { return FromDouble(Math.E); }
		}

		/// <summary>
		/// Calculate the exponent (<c>e^x</c>) of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns><c>e^x</c>.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Exp(T value)
		{
			return FromDouble(Math.Exp(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the square root of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The square root of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Sqrt(T value)
		{
			return FromDouble(Math.Sqrt(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the natural logarithm of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>Natural logarithm of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Log(T value)
		{
			return FromDouble(Math.Log(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the power of value.
		/// </summary>
		/// <param name="value">The base of exponentiation.</param>
		/// <param name="exp">The exponent power of exponentiation.</param>
		/// <returns>The value raised to the exp-th power.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Pow(T value, T exp)
		{
			return FromDouble(Math.Pow(ToDouble(value), ToDouble(exp)));
		}

		/// <summary>
		/// Calculate the logarithm of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <param name="newBase">The base of the logarithm.</param>
		/// <returns>The logarithm of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Log(T value, T newBase)
		{
			return FromDouble(Math.Log(ToDouble(value), ToDouble(newBase)));
		}

		/// <summary>
		/// Calculate the sine of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The sine of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Sin(T value)
		{
			return FromDouble(Math.Sin(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the tangent of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The tangent of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Tan(T value)
		{
			return FromDouble(Math.Tan(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the cosine of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The cosine of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Cos(T value)
		{
			return FromDouble(Math.Cos(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the arcsin of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The arcsin of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Asin(T value)
		{
			return FromDouble(Math.Asin(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the arctan of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The arctan of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Atan(T value)
		{
			return FromDouble(Math.Atan(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the arccos of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The arccos of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Acos(T value)
		{
			return FromDouble(Math.Acos(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the hyperbolic sine of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The hyperbolic sine of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Sinh(T value)
		{
			return FromDouble(Math.Sin(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the hyperbolic tangent of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The hyperbolic tangent of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Tanh(T value)
		{
			return FromDouble(Math.Tan(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the hyperbolic cosine of the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The hyperbolic cosine of the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Cosh(T value)
		{
			return FromDouble(Math.Cos(ToDouble(value)));
		}

		/// <summary>
		/// Calculate the arctan of the division.
		/// </summary>
		/// <param name="y">The first value.</param>
		/// <param name="x">The second value.</param>
		/// <returns>Arctan of <c>y / x</c>.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Atan2(T y, T x)
		{
			return FromDouble(Math.Atan2(ToDouble(y), ToDouble(x)));
		}

		#endregion

		#region NaN, Infinity

		/// <summary>
		/// Returns the value of NaN (Not a Number).
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T NaN
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Returns the value of positive infinity (+inf).
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T PositiveInfinity
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Returns the value of negative infinity (-inf).
		/// </summary>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T NegativeInfinity
		{
			get { throw new NotSupportedException(); }
		}

		/// <summary>
		/// Gets the value indicating that the value is NaN (Not a Number).
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns><c>true</c> if the value is NaN; <c>false</c> otherwise.</returns>
		public virtual bool IsNaN(T value)
		{
			return false;
		}

		/// <summary>
		/// Gets the value indicating that the value is infinity.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns><c>true</c> if the value is infinity; <c>false</c> otherwise.</returns>
		public virtual bool IsInfinite(T value)
		{
			return false;
		}

		#endregion

		#region Float => Ordinal

		/// <summary>
		/// Truncates the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The truncated value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Truncate(T value)
		{
			return FromDouble(Math.Truncate(ToDouble(value)));
		}

		/// <summary>
		/// Rounds the value to integer, round midpoint away from zero.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The rounded to integer value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public T Round(T value)
		{
			return Round(value, MidpointRounding.AwayFromZero);
		}

		/// <summary>
		/// Rounds the value to certain decimal places, round midpoint away from zero.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <param name="decimalPlaces">Decimal places round to.</param>
		/// <returns>The rounded to certain decimal places value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public T Round(T value, int decimalPlaces)
		{
			return Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
		}

		/// <summary>
		/// Rounds the value to integer with midpoint rounding mode.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <param name="mode">Midpoint rounding mode.</param>
		/// <returns>The rounded to integer value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Round(T value, MidpointRounding mode)
		{
			return FromDouble(Math.Round(ToDouble(value), mode));
		}

		/// <summary>
		/// Rounds the value to certain decimal places with midpoint rounding mode.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <param name="decimalPlaces">Decimal places round to.</param>
		/// <param name="mode">Midpoint rounding mode.</param>
		/// <returns>The rounded to certain decimal places value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Round(T value, int decimalPlaces, MidpointRounding mode)
		{
			return FromDouble(Math.Round(ToDouble(value), decimalPlaces, mode));
		}

		/// <summary>
		/// Returns the smallest integer greater than or equal to the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The smallest integer greater than or equal to the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Ceiling(T value)
		{
			return FromDouble(Math.Ceiling(ToDouble(value)));
		}

		/// <summary>
		/// Returns the greatest integer lesser than or equal to the value.
		/// </summary>
		/// <param name="value">The numerical value.</param>
		/// <returns>The greatest integer lesser than or equal to the value.</returns>
		/// <exception cref="System.NotSupportedException"/>
		public virtual T Floor(T value)
		{
			return FromDouble(Math.Floor(ToDouble(value)));
		}

		#endregion

		#region Other functions

		/// <summary>
		/// Converts the string representation of a value to its <typeparamref name="T"/> equivalent.
		/// </summary>
		/// <param name="s">A string containing a number to convert.</param>
		/// <returns>
		/// <typeparamref name="T"/> value equivalent to the value
		/// contained in <paramref name="s"/>.
		/// </returns>
		/// <exception cref="System.FormatException"/>
		public virtual T Parse(string s)
		{
			T result;
			if (!TryParse(s, out result))
				throw new FormatException();

			return result;
		}

		/// <summary>
		/// Try converts the string representation of a value to its <typeparamref name="T"/> equivalent.
		/// </summary>
		/// <param name="s">A string containing a number to convert.</param>
		/// <param name="result">
		/// The resulting <typeparamref name="T"/> value equivalent to
		/// the value contained in <paramref name="s"/>.
		/// </param>
		/// <returns><c>true</c> if s was converted successfully; <c>false</c> otherwise.</returns>
		public virtual bool TryParse(string s, out T result)
		{
			object value;
			if (ValueConverter.TryConvert(s, typeof(T), out value))
			{
				result = (T)value;
				return true;
			}

			result = default(T);
			return false;
		}

		private static IEnumerable<T> Iterate(T start, Func<T, T> nextValue)
		{
			T current = start;

			while (true)
			{
				yield return current;
				current = nextValue(current);
			}
		}

		/// <summary>
		/// Checks <paramref name="value"/> to be equals <c>null</c>.
		/// If it's true, throws an <see cref="ArgumentNullException"/>.
		/// </summary>
		/// <param name="value">Reference to object to check.</param>
		/// <exception cref="System.ArgumentNullException"/>
		protected static void CheckValue(object value)
		{
			if (value == null)
				throw new ArgumentNullException("value");
		}

		#endregion
	}
}
