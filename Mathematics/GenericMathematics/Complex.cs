using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GenericMathematics
{
	[TypeConverter(typeof(ComplexConverter))]
	public struct Complex : IEquatable<Complex>, IComparable, 
		IComparable<Complex>, IFormattable
	{
		static readonly Regex ParseRegex = new Regex(
			@"^([+-])?([^+ i-]*)([iI])?( ?([+-]) ?([^+ i-]*)([iI])?)?$");

		public static readonly Complex Zero = new Complex(0.0, 0.0);
		public static readonly Complex MinValue = new Complex(double.MinValue, double.MinValue);
		public static readonly Complex MaxValue = new Complex(double.MaxValue, double.MaxValue);

		public static readonly Complex NaN = new Complex(double.NaN, double.NaN);
		public static readonly Complex PositiveInfinity = 
			new Complex(double.PositiveInfinity, double.PositiveInfinity);
		public static readonly Complex NegativeInfinity =
			new Complex(double.NegativeInfinity, double.NegativeInfinity);

		public static readonly Complex ReOne = new Complex(1.0, 0);
		public static readonly Complex ImOne = new Complex(0, 1.0);

		public double Re;
		public double Im;

		public double Modulus
		{
			get
			{
				return Math.Sqrt(Math.Pow(Re, 2) + Math.Pow(Im, 2));
			}
		}

		public double Argument
		{
			get
			{
				return Math.Atan2(Im, Re);
			}
		}

		public Complex(double real, double imaginary)
		{
			Re = real;
			Im = imaginary;
		}

		#region Methods Overriding

		public override bool Equals(object obj)
		{
			Complex? other = obj as Complex?;
			if (other == null)
				return false;

			return this.Equals((Complex)other);
		}

		public override int GetHashCode()
		{
			return Re.GetHashCode() ^ Im.GetHashCode();
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
			string numberFormat = format;
			var spec = FormatParser.FirstSpecifierOrDefault(ref format, 'S', -1);

			if (char.ToUpperInvariant(spec.Item1) == 'S') // or format == null
			{
				StringBuilder result = new StringBuilder();

				bool outputReal = Re != 0;
				bool outputImaginary = Im != 0;

				if (outputReal)
				{
					result.Append(
						format != null
						? Re.ToString(format)
						: Re.ToString());
				}

				if (outputImaginary)
				{
					if (Im < 0)
						result.Append("-");
					else if (outputReal)
						result.Append("+");

					var imaginaryModulus = Math.Abs(Im);
					if (imaginaryModulus != 1)
					{
						result.Append(
							format != null
							? imaginaryModulus.ToString(format)
							: imaginaryModulus.ToString());
					}

					result.Append("i");
				}

				if (!outputReal && !outputImaginary)
					result.Append("0");

				return result.ToString();
			}
			else
			{
				string real = Re.ToString(numberFormat, formatProvider);
				string imaginary = Math.Abs(Im).ToString(numberFormat, formatProvider);

				return string.Format("{0}{1}{2}i",
					real,
					Im >= 0 ? "+" : "-",
					imaginary);
			}
		}

		#endregion

		#region Operations

		public bool Equals(Complex other)
		{
			return 
				this.Re == other.Re &&
				this.Im == other.Im;
		}

		public Complex Negate()
		{
			return new Complex(-Re, -Im);
		}

		public Complex Add(Complex c)
		{
			return new Complex(
				this.Re + c.Re,
				this.Im + c.Im);
		}

		public Complex Add(double value)
		{
			return new Complex(Re + value, Im);
		}

		public Complex Subtract(Complex c)
		{
			return new Complex(
				this.Re - c.Re,
				this.Im - c.Im);
		}

		public Complex Subtract(double value)
		{
			return new Complex(Re - value, Im);
		}

		public Complex Multiply(Complex c)
		{
			return new Complex(
				this.Re * c.Re - this.Im * c.Im,
				this.Im * c.Re + this.Re * c.Im);
		}

		public Complex Multiply(double value)
		{
			return new Complex(Re * value, Im * value);
		}

		public Complex Divide(Complex c)
		{
			double denom = Math.Pow(c.Re, 2) + Math.Pow(c.Im, 2);
			return new Complex(
				(this.Re * c.Re + this.Im * c.Im) / denom,
				(this.Im * c.Re - this.Re * c.Im) / denom);
		}

		public Complex Divide(double value)
		{
			return new Complex(Re / value, Im / value);
		}

		public Complex Conjugation()
		{
			return new Complex(Re, -Im);
		}

		public Complex Pow(int n)
		{
			if (n < 0)
				throw new ArgumentException(TextResources.Complex_PowerDegree, "n");

			double r1 = Math.Pow(this.Modulus, n);
			double f1 = this.Argument * n;

			while (f1 > 2 * Math.PI)
			{
				f1 -= 2 * Math.PI;
			}

			while (f1 < -2 * Math.PI)
			{
				f1 += 2 * Math.PI;
			}

			return Complex.FromTrigonometricForm(r1, f1);
		}

		public Complex[] Roots(int n)
		{
			if (n < 2)
				throw new ArgumentException(TextResources.Complex_RootDegree, "n");

			double r1 = Math.Pow(this.Modulus, 1.0 / (double)n);
			double f1 = this.Argument / n;
			double step = 2 * Math.PI / n;

			Complex[] roots = new Complex[n];
			for (int i = 0; i < n; i++)
			{
				double f = f1 + step * i;
				roots[i] = Complex.FromTrigonometricForm(r1, f);
			}

			return roots;
		}

		public string ToTrigonometricString()
		{
			return string.Format("{0}*(cos{1} + i*sin{1})",
				Modulus, Argument * 180.0 / Math.PI);
		}

		#endregion

		#region Operators Overloading

		public static Complex operator +(Complex c)
		{
			return c;
		}

		public static Complex operator -(Complex c)
		{
			return c.Negate();
		}

		public static Complex operator +(Complex c1, Complex c2)
		{
			return c1.Add(c2);
		}

		public static Complex operator -(Complex c1, Complex c2)
		{
			return c1.Subtract(c2);
		}

		public static Complex operator *(Complex c1, Complex c2)
		{
			return c1.Multiply(c2);
		}

		public static Complex operator /(Complex c1, Complex c2)
		{
			return c1.Divide(c2);
		}

		public static bool operator ==(Complex c1, Complex c2)
		{
			return c1.Equals(c2);
		}

		public static bool operator !=(Complex c1, Complex c2)
		{
			return !c1.Equals(c2);
		}

		public static bool operator <(Complex c1, Complex c2)
		{
			return c1.CompareTo(c2) == -1;
		}

		public static bool operator >(Complex c1, Complex c2)
		{
			return c1.CompareTo(c2) == 1;
		}

		public static bool operator <=(Complex c1, Complex c2)
		{
			return c1.CompareTo(c2) != 1;
		}

		public static bool operator >=(Complex c1, Complex c2)
		{
			return c1.CompareTo(c2) != -1;
		}

		public static implicit operator Complex(double value)
		{
			return new Complex(value, 0);
		}

		public static explicit operator double(Complex c)
		{
			return c.Modulus;
		}

		#endregion

		#region IComparable and IComparable<Complex> Members

		int IComparable.CompareTo(object obj)
		{
			Complex? other = obj as Complex?;
			if (other == null)
				return 0;

			return CompareTo((Complex)other);
		}

		public int CompareTo(Complex other)
		{
			return this.Modulus.CompareTo(other.Modulus);
		}

		#endregion

		#region Static Methods

		public static Complex FromTrigonometricForm(double modulus, double argument)
		{
			return new Complex(
				modulus * Math.Cos(argument),
				modulus * Math.Sin(argument));
		}

		public static Complex Parse(string s)
		{
			Complex complex;
			if (TryParse(s, out complex))
			{
				return complex;
			}
			else
			{
				throw new FormatException();
			}
		}

		public static bool TryParse(string s, out Complex result)
		{
			//     1        2       3    4    5         6       7
			// ^([+-])?([^+ i-]*)([iI])?( ?([+-]) ?([^+ i-]*)([iI])?)?$

			var match = ParseRegex.Match(s);
			
			result = Complex.Zero;

			if (match.Success)
			{
				bool firstImaginary = match.Groups[3].Success;

				double first;
				if (firstImaginary && match.Groups[2].Length == 0)
				{
					first = 1;
				}
				else if (!double.TryParse(match.Groups[2].Value, out first))
				{
					return false;
				}
				
				if (match.Groups[1].Success &&
					match.Groups[1].Value.Equals("-", StringComparison.Ordinal))
				{
					first *= -1;
				}

				if (match.Groups[4].Success)
				{
					bool secondImaginary = match.Groups[7].Success;

					double second;
					if (secondImaginary && match.Groups[6].Length == 0)
					{
						second = 1;
					}
					else if (!double.TryParse(match.Groups[6].Value, out second))
					{
						return false;
					}

					if (match.Groups[5].Value.Equals("-", StringComparison.Ordinal))
					{
						second *= -1;
					}

					if (firstImaginary && !secondImaginary ||
						!firstImaginary && secondImaginary)
					{
						if (firstImaginary)
							result = new Complex(second, first);
						else
							result = new Complex(first, second);

						return true;
					}
				}
				else
				{
					if (firstImaginary)
						result = new Complex(0, first);
					else
						result = new Complex(first, 0);

					return true;
				}
			}
			
			return false;
		}

		#endregion
	}
    
	internal class ComplexMath : Math<Complex>
	{
		static readonly Complex epsilon = new Complex(double.Epsilon, 0);

		public override bool IsFractional { get { return true; } }
		public override bool IsFloatingPoint { get { return true; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool HasBounds { get { return true; } }
		public override bool IsUnsigned { get { return false; } }
		public override Complex MinValue { get { return Complex.MinValue; } }
		public override Complex MaxValue { get { return Complex.MaxValue; } }
		public override Complex Zero { get { return Complex.Zero; } }
		public override Complex NaN { get { return Complex.NaN; } }
		public override Complex PositiveInfinity { get { return Complex.PositiveInfinity; } }
		public override Complex NegativeInfinity { get { return Complex.NegativeInfinity; } }
		public override bool IsInfinite(Complex value) { return double.IsInfinity(value.Re) || double.IsInfinity(value.Im); }
		public override bool IsNaN(Complex value) { return double.IsNaN(value.Re) || double.IsNaN(value.Im); }
		public override MathFeatures Features { get { return MathFeatures.Bounded | MathFeatures.Fractional; } }
		public override Complex FromInt32(int value) { return new Complex(value, 0); }
		public override int ToInt32(Complex value) { return (int)value.Modulus; }
		public override int Compare(Complex x, Complex y) { return x.CompareTo(y); }
		public override bool Equals(Complex x, Complex y) { return x.Equals(y); }
		public override int GetHashCode(Complex obj) { return obj.GetHashCode(); }
		public override Complex Add(Complex x, Complex y) { return x.Add(y); }
		public override Complex Subtract(Complex x, Complex y) { return x.Subtract(y); }
		public override Complex Negate(Complex value) { return value.Negate(); }
		public override Complex Multiply(Complex x, Complex y) { return x.Multiply(y); }
		public override Complex QuotientRemainder(Complex x, Complex y, out Complex remainder) { remainder = Complex.Zero; return x.Divide(y); }
		public override Complex DivideIntegralModulus(Complex x, Complex y, out Complex modulus) { modulus = Complex.Zero; return x.Divide(y); }
		public override Complex Divide(Complex x, Complex y) { return x.Divide(y); }
		public override Complex Reciprocal(Complex value) { return Complex.ReOne / value; }
		public override Complex Predecessor(Complex value) { return new Complex(value.Re - 1.0, value.Im); }
		public override Complex Successor(Complex value) { return new Complex(value.Re + 1.0, value.Im); }
		public override Complex Sign(Complex value) { return value.Re == 0 && value.Im == 0 ? 0 : value.Divide(value.Modulus); }
		public override Complex E { get { return new Complex(Math.E, 0); } }
		public override Complex Pi { get { return new Complex(Math.PI, 0); } }
		public override Complex Abs(Complex value) { return value.Modulus; }
		public override Complex Truncate(Complex value)
			{ return new Complex(Math.Truncate(value.Re), Math.Truncate(value.Im)); }
		public override Complex Round(Complex value, int decimalPlaces, MidpointRounding mode) 
			{ return new Complex(Math.Round(value.Re, decimalPlaces, mode), Math.Round(value.Im, decimalPlaces, mode)); }
		public override Complex Round(Complex value, MidpointRounding mode)
			{ return new Complex(Math.Round(value.Re, mode), Math.Round(value.Im, mode)); }
		public override Complex Ceiling(Complex value)
			{ return new Complex(Math.Ceiling(value.Re), Math.Ceiling(value.Im)); }
		public override Complex Floor(Complex value)
			{ return new Complex(Math.Floor(value.Re), Math.Floor(value.Im)); }
		public override bool TryParse(string s, out Complex result) { return Complex.TryParse(s, out result); }
	}

	public class ComplexConverter : TypeConverter
	{
		static readonly Dictionary<Type, Func<object, Complex>> mapFrom =
			new Dictionary<Type, Func<object, Complex>>();
		static readonly Dictionary<Type, Func<Complex, object>> mapTo =
			new Dictionary<Type, Func<Complex, object>>();

		static void From<T>(Func<object, Complex> converter)
		{
			mapFrom.Add(typeof(T), converter);
		}

		static void To<T>(Func<Complex, object> converter)
		{
			mapTo.Add(typeof(T), converter);
		}

		static ComplexConverter()
		{
			From<byte>(x => new Complex((byte)x, 0));
			From<sbyte>(x => new Complex((sbyte)x, 0));
			From<short>(x => new Complex((short)x, 0));
			From<ushort>(x => new Complex((ushort)x, 0));
			From<int>(x => new Complex((int)x, 0));
			From<uint>(x => new Complex((uint)x, 0));
			From<long>(x => new Complex((long)x, 0));
			From<ulong>(x => new Complex((ulong)x, 0));
			From<float>(x => new Complex((float)x, 0));
			From<double>(x => new Complex((double)x, 0));
			From<decimal>(x => new Complex((double)(decimal)x, 0));
			From<string>(x => Complex.Parse((string)x));
			From<object>(x => (Complex)x);

			To<byte>(c => (byte)(double)c);
			To<sbyte>(c => (sbyte)(double)c);
			To<short>(c => (short)(double)c);
			To<ushort>(c => (ushort)(double)c);
			To<int>(c => (int)(double)c);
			To<uint>(c => (uint)(double)c);
			To<long>(c => (long)(double)c);
			To<ulong>(c => (ulong)(double)c);
			To<float>(c => (float)(double)c);
			To<double>(c => (double)c);
			To<decimal>(c => (decimal)(double)c);
			To<string>(c => c.ToString());
			To<object>(c => c);
		}

		public override bool CanConvertFrom(
			ITypeDescriptorContext context, Type sourceType)
		{
			return mapFrom.ContainsKey(sourceType);
		}

		public override bool CanConvertTo(
			ITypeDescriptorContext context, Type destinationType)
		{
			return mapTo.ContainsKey(destinationType);
		}

		public override object ConvertFrom(
			ITypeDescriptorContext context, System.Globalization.CultureInfo culture, 
			object value)
		{
			Func<object, Complex> converter;
			if (mapFrom.TryGetValue(value.GetType(), out converter))
			{
				return converter(value);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public override object ConvertTo(
			ITypeDescriptorContext context, System.Globalization.CultureInfo culture, 
			object value, Type destinationType)
		{
			if (value.GetType() != typeof(Complex))
			{
				throw new ArgumentException(
					string.Format(
						TextResources.TypeConverter_ValueTypeMismatch, typeof(Complex).FullName),
					"value");
			}

			Func<Complex, object> converter;
			if (mapTo.TryGetValue(destinationType, out converter))
			{
				return converter((Complex)value);
			}
			else
			{
				throw new NotSupportedException();
			}
		}
	}
}
