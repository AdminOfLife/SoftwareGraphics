using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace GenericMathematics
{
	[TypeConverter(typeof(FractionConverter))]
	public struct Fraction : IEquatable<Fraction>, IFormattable, 
		IComparable, IComparable<Fraction>
	{
		static readonly Regex ParseRegex = new Regex(@"^([^ /]*)( ?/ ?([^ /]+))?$");

		public static readonly Fraction Zero = new Fraction(0, 0);
		
		private BigInteger numerator;
		private BigInteger denominator;

		public BigInteger Numerator
		{
			get { return numerator; }
			set
			{
				numerator = value;
			}
		}

		public BigInteger Denominator
		{
			get { return denominator; }
			set
			{
				if (value.Sign < 0)
					numerator = -numerator;

				denominator = BigInteger.Abs(value);
			}
		}

		public bool IsZero
		{
			get { return numerator.IsZero || denominator.IsZero; }
		}

		public Fraction(BigInteger numerator, BigInteger denominator)
		{
			this.numerator = 0;
			this.denominator = 0;

			Numerator = numerator;
			Denominator = denominator;
			Reduce();
		}

		#region Methods Overriding

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Fraction))
				return false;

			Fraction other = (Fraction)obj;
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return numerator.GetHashCode() ^ denominator.GetHashCode();
		}

		public override string ToString()
		{
			return ToString(null, null);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (IsZero)
			{
				return "0";
			}
			else if (denominator.IsOne)
			{
				return numerator.ToString();
			}
			else if (format == null)
			{
				return string.Format("{0}/{1}", numerator, denominator);
			}
			else
			{
				return string.Format("{0}/{1}",
					numerator.ToString(format), denominator.ToString(format));
			}
		}

		#endregion

		#region Operations

		public bool Equals(Fraction other)
		{
			Fraction frac1 = this;
			Fraction frac2 = other;

			frac1.Reduce();
			frac2.Reduce();

			return
				frac1.numerator == frac2.numerator &&
				frac1.denominator == frac2.denominator;
		}

		public void Reduce()
		{
			if (numerator.IsZero)
			{
				denominator = 0;
			}
			else if (denominator.IsZero)
			{
				numerator = 0;
			}
			else
			{
				BigInteger gcd = BigInteger.GreatestCommonDivisor(
					numerator, denominator);

				numerator /= gcd;
				denominator /= gcd;
			}
		}

		public Fraction Negate()
		{
			return new Fraction(-numerator, denominator);
		}

		public Fraction Add(Fraction frac)
		{
			Fraction frac1 = this;
			Fraction frac2 = frac;
			Fraction result = Fraction.Zero;

			frac1.Reduce();
			frac2.Reduce();

			if (frac1.IsZero)
			{
				result = frac2;
			}
			else if (frac2.IsZero)
			{
				result = frac1;
			}
			else
			{
				var denomGcd = BigInteger.GreatestCommonDivisor(
					frac1.denominator, frac2.denominator);

				var m1 = frac1.denominator / denomGcd;
				var m2 = frac2.denominator / denomGcd;

				var newNumerator = frac1.numerator * m2 + frac2.numerator * m1;
				var newDenom = denomGcd * m1 * m2;
				result = new Fraction(newNumerator, newDenom); // auto-reduced
			}

			return result;
		}

		public Fraction Subtract(Fraction frac)
		{
			return Add(-frac);
		}

		public Fraction Multiply(Fraction frac)
		{
			Fraction frac1 = this;
			Fraction frac2 = frac;
			Fraction result = Fraction.Zero;

			frac1.Reduce();
			frac2.Reduce();

			if (frac1.IsZero || frac2.IsZero)
			{
				result = Fraction.Zero;
			}
			else
			{
				var newNumerator = frac1.numerator * frac2.numerator;
				var newDenom = frac1.denominator * frac2.denominator;
				result = new Fraction(newNumerator, newDenom); // auto-reduced
			}

			return result;
		}

		public Fraction Divide(Fraction frac)
		{
			return Multiply(new Fraction(
				frac.numerator.Sign * frac.denominator,
				BigInteger.Abs(frac.numerator)));
		}

		public double ToDouble()
		{
			return IsZero ? 0 : ((double)numerator / (double)denominator);
		}

		#endregion

		#region Operators Overloading

		public static Fraction operator +(Fraction frac)
		{
			return frac;
		}

		public static Fraction operator -(Fraction frac)
		{
			return frac.Negate();
		}

		public static Fraction operator +(Fraction frac1, Fraction frac2)
		{
			return frac1.Add(frac2);
		}

		public static Fraction operator -(Fraction frac1, Fraction frac2)
		{
			return frac1.Subtract(frac2);
		}

		public static Fraction operator *(Fraction frac1, Fraction frac2)
		{
			return frac1.Multiply(frac2);
		}

		public static Fraction operator /(Fraction frac1, Fraction frac2)
		{
			return frac1.Divide(frac2);
		}

		public static bool operator ==(Fraction frac1, Fraction frac2)
		{
			return frac1.Equals(frac2);
		}

		public static bool operator !=(Fraction frac1, Fraction frac2)
		{
			return !frac1.Equals(frac2);
		}

		public static bool operator <(Fraction frac1, Fraction frac2)
		{
			return frac1.CompareTo(frac2) == -1;
		}

		public static bool operator >(Fraction frac1, Fraction frac2)
		{
			return frac1.CompareTo(frac2) == 1;
		}

		public static bool operator <=(Fraction frac1, Fraction frac2)
		{
			return frac1.CompareTo(frac2) != 1;
		}

		public static bool operator >=(Fraction frac1, Fraction frac2)
		{
			return frac1.CompareTo(frac2) != -1;
		}

		public static implicit operator Fraction(int value)
		{
			return new Fraction(value, 1);
		}

		public static implicit operator Fraction(long value)
		{
			return new Fraction(value, 1);
		}

		public static explicit operator long(Fraction frac)
		{
			return frac.IsZero ? 0 : (long)(frac.numerator / frac.denominator);
		}

		public static explicit operator double(Fraction frac)
		{
			return frac.ToDouble();
		}

		public static explicit operator float(Fraction frac)
		{
			return (float)frac.ToDouble();
		}

		public static explicit operator decimal(Fraction frac)
		{
			return frac.IsZero ? 0 : ((decimal)frac.numerator / (decimal)frac.denominator);
		}

		#endregion

		#region IComparable and IComparable<Fraction> Members

		int IComparable.CompareTo(object obj)
		{
			if (obj.GetType() != typeof(Fraction))
				return 0;

			Fraction other = (Fraction)obj;
			return CompareTo(other);
		}

		public int CompareTo(Fraction other)
		{
			Fraction frac1 = this;
			Fraction frac2 = other;
			
			frac1.Reduce();
			frac2.Reduce();

			if (frac1.IsZero)
			{
				return -frac2.numerator.Sign;
			}
			else if (frac2.IsZero)
			{
				return frac1.numerator.Sign;
			}
			else
			{
				var denomGcd = BigInteger.GreatestCommonDivisor(
					frac1.denominator, frac2.denominator);

				var m1 = frac1.denominator / denomGcd;
				var m2 = frac2.denominator / denomGcd;

				checked
				{
					var left = frac1.numerator * m2;
					var right = frac2.numerator * m1;

					return left.CompareTo(right);
				}
			}
		}

		#endregion

		#region Static Methods

		public static Fraction Parse(string s)
		{
			Fraction frac;
			if (TryParse(s, out frac))
			{
				return frac;
			}
			else
			{
				throw new FormatException();
			}
		}

		public static bool TryParse(string s, out Fraction result)
		{
			var match = ParseRegex.Match(s);
			if (match.Success)
			{
				BigInteger numerator;
				if (BigInteger.TryParse(match.Groups[1].Value, out numerator))
				{
					Fraction frac = new Fraction();
					frac.Numerator = numerator;
					
					// a (/ b); if '/b' exist parse 'b'
					if (match.Groups[2].Success)
					{
						BigInteger denominator;
						if (BigInteger.TryParse(match.Groups[3].Value, out denominator))
						{
							frac.Denominator = denominator;
							result = frac;
							return true;
						}
					}
					else
					{
						frac.Denominator = 1;
						result = frac;
						return true;
					}
				}
			}

			result = Fraction.Zero;
			return false;
		}

		public static Fraction FromDouble(double value, int significantDigits)
		{
			if (significantDigits < 0)
				throw new ArgumentOutOfRangeException("significantDigits");

			double multipler = Math.Pow(10.0, significantDigits);
			double numerator = Math.Round(value, significantDigits,
				MidpointRounding.AwayFromZero) * multipler;

			return new Fraction((long)numerator, (long)multipler);
		}

		public static void SetCommonDenominator(Fraction[] values)
		{
			if (values == null)
				throw new ArgumentNullException("values");
			if (values.Length == 0)
				return;

			var fracs = values.Select(f => f.Denominator).Where(d => d != 0).ToArray();
			if (fracs.Length < 2)
				return;

			BigInteger gcd = BigInteger.GreatestCommonDivisor(fracs[0], fracs[1]);
			for (int i = 2; i < fracs.Length; i++)
			{
				gcd = BigInteger.GreatestCommonDivisor(gcd, fracs[i]);
			}

			BigInteger lcm = 1;
			for (int i = 0; i < fracs.Length; i++)
			{
				lcm *= fracs[i];
			}
			lcm /= gcd;

			for (int i = 0; i < values.Length; i++)
			{
				Fraction frac = values[i];
				if (!frac.IsZero)
				{
					frac.Numerator *= lcm / frac.Denominator;
					frac.Denominator = lcm;
					values[i] = frac;
				}
			}
		}

		#endregion
	}
    
	internal class FractionMath : Math<Fraction>
	{
		internal const int DefaultSignificantDigits = 10;
		static readonly Fraction epsilon = new Fraction(1, long.MaxValue);

		public override Fraction Zero { get { return Fraction.Zero; } }
		public override bool IsFractional { get { return true; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool IsTwosComplement { get { return true; } }
		public override bool HasBounds { get { return false; } }
		public override bool IsUnsigned { get { return false; } }
		public override Fraction FromInt32(int value) { return new Fraction(value, 1); }
		public override int ToInt32(Fraction value) { return (int)value; }
		public override int Compare(Fraction x, Fraction y) { return x.CompareTo(y); }
		public override bool Equals(Fraction x, Fraction y) { return x.Equals(y); }
		public override Fraction Add(Fraction x, Fraction y) { return x.Add(y); }
		public override Fraction Subtract(Fraction x, Fraction y) { return x.Subtract(y); }
		public override Fraction Negate(Fraction value) { return value.Negate(); }
		public override Fraction Multiply(Fraction x, Fraction y) { return x.Multiply(y); }
		public override Fraction QuotientRemainder(Fraction x, Fraction y, out Fraction remainder) { remainder = Fraction.Zero; return x.Divide(y); }
		public override Fraction Divide(Fraction x, Fraction y) { return x.Divide(y); }
		public override Fraction DivideIntegral(Fraction x, Fraction y) { return x.Divide(y); }
		public override Fraction DivideIntegralModulus(Fraction x, Fraction y, out Fraction modulus) { return QuotientRemainder(x, y, out modulus); }
		public override Fraction Predecessor(Fraction value) { return new Fraction(value.Numerator - value.Denominator, value.Denominator); }
		public override Fraction Successor(Fraction value) { return new Fraction(value.Numerator + value.Denominator, value.Denominator); }
		public override Fraction Sign(Fraction value) { return value.Numerator.Sign; }
		public override Fraction E { get { return new Fraction(666, 245); } }
		public override Fraction Pi { get { return new Fraction(355, 113); } }
		public override Fraction Abs(Fraction value) { return new Fraction(BigInteger.Abs(value.Numerator), value.Denominator); }
		public override Fraction Truncate(Fraction value) { return (int)value; }
		public override Fraction Reciprocal(Fraction value) { return new Fraction(value.Denominator, value.Numerator); }
		public override double ToDouble(Fraction value) { return (double)value; }
		public override Fraction FromDouble(double value) { return Fraction.FromDouble(value, DefaultSignificantDigits); }
		public override bool TryParse(string s, out Fraction result) { return Fraction.TryParse(s, out result); }
	}

	public sealed class FractionConverter : TypeConverter
	{
		static readonly Dictionary<Type, Func<object, Fraction>> mapFrom =
			new Dictionary<Type, Func<object, Fraction>>();
		static readonly Dictionary<Type, Func<Fraction, object>> mapTo =
			new Dictionary<Type, Func<Fraction, object>>();

		static void From<T>(Func<object, Fraction> converter)
		{
			mapFrom.Add(typeof(T), converter);
		}

		static void To<T>(Func<Fraction, object> converter)
		{
			mapTo.Add(typeof(T), converter);
		}

		static FractionConverter()
		{
			From<byte>(x => new Fraction((byte)x, 1));
			From<sbyte>(x => new Fraction((sbyte)x, 1));
			From<short>(x => new Fraction((short)x, 1));
			From<ushort>(x => new Fraction((ushort)x, 1));
			From<int>(x => new Fraction((int)x, 1));
			From<uint>(x => new Fraction((long)(uint)x, 1));
			From<long>(x => new Fraction((long)x, 1));
			From<ulong>(x => new Fraction((long)(ulong)x, 1));
			From<float>(x => Fraction.FromDouble((float)x, FractionMath.DefaultSignificantDigits));
			From<double>(x => Fraction.FromDouble((double)x, FractionMath.DefaultSignificantDigits));
			From<decimal>(x => Fraction.FromDouble((double)(decimal)x, FractionMath.DefaultSignificantDigits));
			From<string>(x => Fraction.Parse((string)x));
			From<object>(x => (Fraction)x);

			To<byte>(f => (byte)(long)f);
			To<sbyte>(f => (sbyte)(long)f);
			To<short>(f => (short)(long)f);
			To<ushort>(f => (ushort)(long)f);
			To<int>(f => (int)f);
			To<uint>(f => (uint)(long)f);
			To<long>(f => (long)f);
			To<ulong>(f => (ulong)(long)f);
			To<float>(f => (float)(double)f);
			To<double>(f => (double)f);
			To<decimal>(f => (decimal)f);
			To<string>(f => f.ToString());
			To<object>(f => f);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return mapFrom.ContainsKey(sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return mapTo.ContainsKey(destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			Func<object, Fraction> converter;
			if (mapFrom.TryGetValue(value.GetType(), out converter))
			{
				return converter(value);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (value.GetType() != typeof(Fraction))
			{
				throw new ArgumentException(
					string.Format(
						TextResources.TypeConverter_ValueTypeMismatch, "GenericMathematics.Fraction."),
					"value");
			}

			Func<Fraction, object> converter;
			if (mapTo.TryGetValue(destinationType, out converter))
			{
				return converter((Fraction)value);
			}
			else
			{
				throw new NotSupportedException();
			}
		}
	}
}
