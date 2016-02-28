using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GenericMathematics
{
	internal class DecimalMath : Math<decimal>
	{
		public override bool IsUnsigned { get { return false; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool IsFractional { get { return true; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(decimal x, decimal y) { return x < y; }
		public override bool LessThanOrEqual(decimal x, decimal y) { return x <= y; }
		public override bool GreaterThan(decimal x, decimal y) { return x > y; }
		public override bool GreaterThanOrEqual(decimal x, decimal y) { return x >= y; }
		public override int Compare(decimal x, decimal y) { return x.CompareTo(y); }
		public override bool Equals(decimal x, decimal y) { return x == y; }
		public override decimal Max(decimal x, decimal y) { return Math.Max(x, y); }
		public override decimal Min(decimal x, decimal y) { return Math.Min(x, y); }
		public override decimal Successor(decimal value) { return checked(value + 1); }
		public override decimal Predecessor(decimal value) { return checked(value - 1); }
		public override decimal FromInt32(int value) { return value; }
		public override int ToInt32(decimal value) { return (int)value; }
		public override bool HasBounds { get { return true; } }
		public override decimal MinValue { get { return decimal.MinValue; } }
		public override decimal MaxValue { get { return decimal.MaxValue; } }
		public override decimal Add(decimal x, decimal y) { return checked(x + y); }
		public override decimal Multiply(decimal x, decimal y) { return checked(x * y); }
		public override decimal Subtract(decimal x, decimal y) { return checked(x - y); }
		public override decimal Negate(decimal value) { return checked(-value); }
		public override decimal Abs(decimal value) { return Math.Abs(value); }
		public override decimal Sign(decimal value) { return Math.Sign(value); }
		public override decimal Quotient(decimal x, decimal y) { return Math.Floor(x / y); }  // truncates toward -inf
		public override decimal QuotientRemainder(decimal x, decimal y, out decimal remainder) { decimal q = Math.Floor(x / y); remainder = x - q * y; return q; }
		public override decimal DivideIntegral(decimal x, decimal y) { return (int)(x / y); }       // truncates toward 0
		public override decimal Modulus(decimal x, decimal y) { return x % y; }
		public override decimal DivideIntegralModulus(decimal x, decimal y, out decimal modulus) { modulus = x % y; return (int)(x / y); }
		public override decimal Divide(decimal x, decimal y) { return x / y; }
		public override decimal Reciprocal(decimal value) { return 1.0m / value; }
		public override decimal Pi { get { return new decimal(Math.PI); } }
		public override decimal E { get { return new decimal(Math.E); } }
		public override decimal Exp(decimal value) { return new decimal(Math.Exp(decimal.ToDouble(value))); }
		public override decimal Sqrt(decimal value) { return new decimal(Math.Sqrt(decimal.ToDouble(value))); }
		public override decimal Log(decimal value) { return new decimal(Math.Log(decimal.ToDouble(value))); }
		public override decimal Pow(decimal value, decimal exp) { return new decimal(Math.Pow(decimal.ToDouble(value), decimal.ToDouble(exp))); }
		public override decimal Log(decimal value, decimal newBase) { return new decimal(Math.Log(decimal.ToDouble(value), decimal.ToDouble(newBase))); }
		public override decimal Sin(decimal value) { return new decimal(Math.Sin(decimal.ToDouble(value))); }
		public override decimal Tan(decimal value) { return new decimal(Math.Tan(decimal.ToDouble(value))); }
		public override decimal Cos(decimal value) { return new decimal(Math.Cos(decimal.ToDouble(value))); }
		public override decimal Asin(decimal value) { return new decimal(Math.Asin(decimal.ToDouble(value))); }
		public override decimal Atan(decimal value) { return new decimal(Math.Atan(decimal.ToDouble(value))); }
		public override decimal Acos(decimal value) { return new decimal(Math.Acos(decimal.ToDouble(value))); }
		public override decimal Sinh(decimal value) { return new decimal(Math.Sinh(decimal.ToDouble(value))); }
		public override decimal Tanh(decimal value) { return new decimal(Math.Tanh(decimal.ToDouble(value))); }
		public override decimal Cosh(decimal value) { return new decimal(Math.Cosh(decimal.ToDouble(value))); }
		public override bool IsNaN(decimal value) { return false; }
		public override bool IsInfinite(decimal value) { return false; }
		public override decimal Atan2(decimal y, decimal x) { return (decimal)Math.Atan2(decimal.ToDouble(y), decimal.ToDouble(x)); }
		public override decimal Truncate(decimal value) { return Math.Truncate(value); }
		public override decimal Round(decimal value, MidpointRounding mode) { return Math.Round(value, mode); }
		public override decimal Round(decimal value, int decimalPlaces, MidpointRounding mode) { return Math.Round(value, decimalPlaces, mode); }
		public override decimal Ceiling(decimal value) { return Math.Ceiling(value); }
		public override decimal Floor(decimal value) { return Math.Floor(value); }
		public override decimal Zero { get { return 0; } }
		public override decimal FromDouble(double value) { return (decimal)value; }
		public override double ToDouble(decimal value) { return (double)value; }
		public override bool TryParse(string s, out decimal result) { return decimal.TryParse(s, out result); }
	}
    
	internal class DoubleMath : Math<double>
	{
		public override bool IsUnsigned { get { return false; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool IsFractional { get { return true; } }
		public override bool IsFloatingPoint { get { return true; } }
		public override bool LessThan(double x, double y) { return x < y; }
		public override bool LessThanOrEqual(double x, double y) { return x <= y; }
		public override bool GreaterThan(double x, double y) { return x > y; }
		public override bool GreaterThanOrEqual(double x, double y) { return x >= y; }
		public override int Compare(double x, double y) { return x.CompareTo(y); }
		public override bool Equals(double x, double y) { return x == y; }
		public override double Max(double x, double y) { return Math.Max(x, y); }
		public override double Min(double x, double y) { return Math.Min(x, y); }
		public override double Successor(double value) { return checked(value + 1); }
		public override double Predecessor(double value) { return checked(value - 1); }
		public override double FromInt32(int value) { return value; }
		public override int ToInt32(double value) { return (int)value; }
		public override bool HasBounds { get { return true; } }
		public override double MinValue { get { return double.MinValue; } }
		public override double MaxValue { get { return double.MaxValue; } }
		public override double Add(double x, double y) { return checked(x + y); }
		public override double Multiply(double x, double y) { return checked(x * y); }
		public override double Subtract(double x, double y) { return checked(x - y); }
		public override double Negate(double value) { return checked(-value); }
		public override double Abs(double value) { return Math.Abs(value); }
		public override double Sign(double value) { return Math.Sign(value); }
		public override double Quotient(double x, double y) { return Math.Floor(x / y); }   // truncates toward -inf
		public override double DivideIntegral(double x, double y) { double q = (x / y); return (IsNaN(q) || IsInfinite(q)) ? q : (double)(int)q; }  // truncates toward 0
		public override double Modulus(double x, double y) { return x % y; }
		public override double QuotientRemainder(double x, double y, out double remainder) { double q = Math.Floor(x / y); remainder = x - q * y; return q; }
		public override double DivideIntegralModulus(double x, double y, out double modulus) { modulus = x % y; return DivideIntegral(x, y); }
		public override double Divide(double x, double y) { return x / y; }
		public override double Reciprocal(double value) { return 1.0 / value; }
		public override double Pi { get { return Math.PI; } }
		public override double E { get { return Math.E; } }
		public override double Exp(double value) { return Math.Exp(value); }
		public override double Sqrt(double value) { return Math.Sqrt(value); }
		public override double Log(double value) { return Math.Log(value); }
		public override double Pow(double value, double exp) { return Math.Pow(value, exp); }
		public override double Log(double value, double newBase) { return Math.Log(value, newBase); }
		public override double Sin(double value) { return Math.Sin(value); }
		public override double Tan(double value) { return Math.Tan(value); }
		public override double Cos(double value) { return Math.Cos(value); }
		public override double Asin(double value) { return Math.Asin(value); }
		public override double Atan(double value) { return Math.Atan(value); }
		public override double Acos(double value) { return Math.Acos(value); }
		public override double Sinh(double value) { return Math.Sinh(value); }
		public override double Tanh(double value) { return Math.Tanh(value); }
		public override double Cosh(double value) { return Math.Cosh(value); }
		public override bool IsNaN(double value) { return double.IsNaN(value); }
		public override bool IsInfinite(double value) { return double.IsInfinity(value); }
		public override double NaN { get { return double.NaN; } }
		public override double PositiveInfinity { get { return double.PositiveInfinity; } }
		public override double NegativeInfinity { get { return double.NegativeInfinity; } }
		public override double Atan2(double y, double x) { return Math.Atan2(y, x); }
		public override double Truncate(double value) { return Math.Truncate(value); }
		public override double Round(double value, MidpointRounding mode) { return Math.Round(value, mode); }
		public override double Round(double value, int decimalPlaces, MidpointRounding mode) { return Math.Round(value, decimalPlaces, mode); }
		public override double Ceiling(double value) { return Math.Ceiling(value); }
		public override double Floor(double value) { return Math.Floor(value); }
		public override double Zero { get { return 0; } }
		public override double FromDouble(double value) { return value; }
		public override double ToDouble(double value) { return value; }
		public override bool TryParse(string s, out double result) { return double.TryParse(s, out result); }
	}
    
	internal class SingleMath : Math<float>
	{
		public override bool IsUnsigned { get { return false; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool IsFractional { get { return true; } }
		public override bool IsFloatingPoint { get { return true; } }
		public override bool LessThan(float x, float y) { return x < y; }
		public override bool LessThanOrEqual(float x, float y) { return x <= y; }
		public override bool GreaterThan(float x, float y) { return x > y; }
		public override bool GreaterThanOrEqual(float x, float y) { return x >= y; }
		public override int Compare(float x, float y) { return x.CompareTo(y); }
		public override bool Equals(float x, float y) { return x == y; }
		public override float Max(float x, float y) { return Math.Max(x, y); }
		public override float Min(float x, float y) { return Math.Min(x, y); }
		public override float Successor(float value) { return checked(value + 1); }
		public override float Predecessor(float value) { return checked(value - 1); }
		public override float FromInt32(int value) { return value; }
		public override int ToInt32(float value) { return (int)value; }
		public override bool HasBounds { get { return true; } }
		public override float MinValue { get { return float.MinValue; } }
		public override float MaxValue { get { return float.MaxValue; } }
		public override float Add(float x, float y) { return checked(x + y); }
		public override float Multiply(float x, float y) { return checked(x * y); }
		public override float Subtract(float x, float y) { return checked(x - y); }
		public override float Negate(float value) { return checked(-value); }
		public override float Abs(float value) { return Math.Abs(value); }
		public override float Sign(float value) { return Math.Sign(value); }
		public override float Quotient(float x, float y) { return (float)Math.Floor(x / y); }  // truncates toward -inf
		public override float DivideIntegral(float x, float y) { float q = (x / y); return IsNaN(q) || IsInfinite(q) ? q : (float)(int)q; }  // truncates toward 0
		public override float Modulus(float x, float y) { return x % y; }
		public override float QuotientRemainder(float x, float y, out float remainder) { float q = Quotient(x, y); remainder = x - q * y; return q; }
		public override float DivideIntegralModulus(float x, float y, out float modulus) { modulus = x % y; return DivideIntegral(x, y); }
		public override float Divide(float x, float y) { return x / y; }
		public override float Reciprocal(float value) { return 1.0f / value; }
		public override float Pi { get { return (float)Math.PI; } }
		public override float E { get { return (float)Math.E; } }
		public override float Exp(float value) { return (float)Math.Exp(value); }
		public override float Sqrt(float value) { return (float)Math.Sqrt(value); }
		public override float Log(float value) { return (float)Math.Log(value); }
		public override float Pow(float value, float exp) { return (float)Math.Pow(value, exp); }
		public override float Log(float value, float newBase) { return (float)Math.Log(value, newBase); }
		public override float Sin(float value) { return (float)Math.Sin(value); }
		public override float Tan(float value) { return (float)Math.Tan(value); }
		public override float Cos(float value) { return (float)Math.Cos(value); }
		public override float Asin(float value) { return (float)Math.Asin(value); }
		public override float Atan(float value) { return (float)Math.Atan(value); }
		public override float Acos(float value) { return (float)Math.Acos(value); }
		public override float Sinh(float value) { return (float)Math.Sinh(value); }
		public override float Tanh(float value) { return (float)Math.Tanh(value); }
		public override float Cosh(float value) { return (float)Math.Cosh(value); }
		public override bool IsNaN(float value) { return float.IsNaN(value); }
		public override bool IsInfinite(float value) { return float.IsInfinity(value); }
		public override float NaN { get { return float.NaN; } }
		public override float PositiveInfinity { get { return float.PositiveInfinity; } }
		public override float NegativeInfinity { get { return float.NegativeInfinity; } }
		public override float Atan2(float y, float x) { return (float)Math.Atan2(y, x); }
		public override float Truncate(float value) { return (float)Math.Truncate(value); }
		public override float Round(float value, MidpointRounding mode) { return (float)Math.Round(value, mode); }
		public override float Round(float value, int decimalPlaces, MidpointRounding mode) { return (float)Math.Round(value, decimalPlaces, mode); }
		public override float Ceiling(float value) { return (float)Math.Ceiling(value); }
		public override float Floor(float value) { return (float)Math.Floor(value); }
		public override float Zero { get { return 0; } }
		public override float FromDouble(double value) { return (float)value; }
		public override double ToDouble(float value) { return value; }
		public override bool TryParse(string s, out float result) { return float.TryParse(s, out result); }
	}
    
	internal class ByteMath : Math<byte>
	{
		public override bool IsUnsigned { get { return true; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(byte x, byte y) { return x < y; }
		public override bool LessThanOrEqual(byte x, byte y) { return x <= y; }
		public override bool GreaterThan(byte x, byte y) { return x > y; }
		public override bool GreaterThanOrEqual(byte x, byte y) { return x >= y; }
		public override int Compare(byte x, byte y) { return x.CompareTo(y); }
		public override bool Equals(byte x, byte y) { return x == y; }
		public override byte Max(byte x, byte y) { return Math.Max(x, y); }
		public override byte Min(byte x, byte y) { return Math.Min(x, y); }
		public override byte Successor(byte value) { return checked((byte)(value + 1)); }
		public override byte Predecessor(byte value) { return checked((byte)(value - 1)); }
		public override byte FromInt32(int value) { return checked((byte)value); }
		public override int ToInt32(byte value) { return value; }
		public override bool HasBounds { get { return true; } }
		public override byte MinValue { get { return byte.MinValue; } }
		public override byte MaxValue { get { return byte.MaxValue; } }
		public override byte Add(byte x, byte y) { return checked((byte)(x + y)); }
		public override byte Multiply(byte x, byte y) { return checked((byte)(x * y)); }
		public override byte Subtract(byte x, byte y) { return checked((byte)(x - y)); }
		public override byte Negate(byte value) { return checked((byte)(-value)); }
		public override byte Abs(byte value) { return (byte)Math.Abs(value); }
		public override byte Sign(byte value) { return (byte)Math.Sign(value); }
		public override byte DivideIntegral(byte x, byte y) { return checked((byte)(x / y)); } // truncates toward 0
		public override byte Modulus(byte x, byte y) { return checked((byte)(x % y)); }
		public override byte QuotientRemainder(byte x, byte y, out byte remainder) { byte q = Quotient(x, y); remainder = checked((byte)(x - q * y)); return q; }
		public override byte DivideIntegralModulus(byte x, byte y, out byte modulus) { modulus = (byte)(x % y); return checked((byte)(x / y)); }
		public override byte Divide(byte x, byte y) { return checked((byte)(x / y)); }
		public override byte Reciprocal(byte value) { return checked((byte)(1 / value)); }
		public override byte Zero { get { return 0; } }
		public override byte FromDouble(double value) { return (byte)value; }
		public override double ToDouble(byte value) { return value; }
		public override bool TryParse(string s, out byte result) { return byte.TryParse(s, out result); }
	}
    
	internal class Int16Math : Math<short>
	{
		public override bool IsUnsigned { get { return false; } }
		public override bool IsTwosComplement { get { return true; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(short x, short y) { return x < y; }
		public override bool LessThanOrEqual(short x, short y) { return x <= y; }
		public override bool GreaterThan(short x, short y) { return x > y; }
		public override bool GreaterThanOrEqual(short x, short y) { return x >= y; }
		public override int Compare(short x, short y) { return x.CompareTo(y); }
		public override bool Equals(short x, short y) { return x == y; }
		public override short Max(short x, short y) { return Math.Max(x, y); }
		public override short Min(short x, short y) { return Math.Min(x, y); }
		public override short Successor(short value) { return checked((short)(value + 1)); }
		public override short Predecessor(short value) { return checked((short)(value - 1)); }
		public override short FromInt32(int value) { return checked((short)value); }
		public override int ToInt32(short value) { return checked((int)value); }
		public override bool HasBounds { get { return true; } }
		public override short MinValue { get { return short.MinValue; } }
		public override short MaxValue { get { return short.MaxValue; } }
		public override short Add(short x, short y) { return checked((short)(x + y)); }
		public override short Multiply(short x, short y) { return checked((short)(x * y)); }
		public override short Subtract(short x, short y) { return checked((short)(x - y)); }
		public override short Negate(short value) { return checked((short)(-value)); }
		public override short Abs(short value) { return checked((short)Math.Abs(value)); }
		public override short Sign(short value) { return (short)Math.Sign(value); }
		public override short DivideIntegral(short x, short y) { return checked((short)(x / y)); } // truncates toward 0
		public override short Modulus(short x, short y) { return checked((short)(x % y)); }
		public override short QuotientRemainder(short x, short y, out short remainder) { short q = Quotient(x, y); remainder = checked((short)(x - q * y)); return q; }
		public override short DivideIntegralModulus(short x, short y, out short modulus) { modulus = checked((short)(x % y)); return checked((short)(x / y)); }
		public override short Divide(short x, short y) { return checked((short)(x / y)); }
		public override short Reciprocal(short value) { return checked((short)(1 / value)); }
		public override short Zero { get { return 0; } }
		public override short FromDouble(double value) { return (short)value; }
		public override double ToDouble(short value) { return value; }
		public override bool TryParse(string s, out short result) { return short.TryParse(s, out result); }
	}
    
	internal class Int32Math : Math<int>
	{
		public override bool IsUnsigned { get { return false; } }
		public override bool IsTwosComplement { get { return true; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(int x, int y) { return x < y; }
		public override bool LessThanOrEqual(int x, int y) { return x <= y; }
		public override bool GreaterThan(int x, int y) { return x > y; }
		public override bool GreaterThanOrEqual(int x, int y) { return x >= y; }
		public override int Compare(int x, int y) { return x.CompareTo(y); }
		public override bool Equals(int x, int y) { return x == y; }
		public override int Max(int x, int y) { return Math.Max(x, y); }
		public override int Min(int x, int y) { return Math.Min(x, y); }
		public override int Successor(int value) { return checked(value + 1); }
		public override int Predecessor(int value) { return checked(value - 1); }
		public override int FromInt32(int value) { return value; }
		public override int ToInt32(int value) { return value; }
		public override bool HasBounds { get { return true; } }
		public override int MinValue { get { return int.MinValue; } }
		public override int MaxValue { get { return int.MaxValue; } }
		public override int Add(int x, int y) { return checked(x + y); }
		public override int Multiply(int x, int y) { return checked(x * y); }
		public override int Subtract(int x, int y) { return checked(x - y); }
		public override int Negate(int value) { return checked(-value); }
		public override int Abs(int value) { return Math.Abs(value); }
		public override int Sign(int value) { return Math.Sign(value); }
		public override int DivideIntegral(int x, int y) { return x / y; } // truncates toward 0
		public override int Modulus(int x, int y) { return x % y; }
		public override int QuotientRemainder(int x, int y, out int remainder) { int q = Quotient(x, y); remainder = checked(x - q * y); return q; }
		public override int DivideIntegralModulus(int x, int y, out int modulus) { modulus = x % y; return x / y; }
		public override int Divide(int x, int y) { return x / y; }
		public override int Reciprocal(int value) { return checked(1 / value); }
		public override int Zero { get { return 0; } }
		public override int FromDouble(double value) { return (int)value; }
		public override double ToDouble(int value) { return value; }
		public override bool TryParse(string s, out int result) { return int.TryParse(s, out result); }
	}
    
	internal class Int64Math : Math<long>
	{
		public override bool IsUnsigned { get { return false; } }
		public override bool IsTwosComplement { get { return true; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(long x, long y) { return x < y; }
		public override bool LessThanOrEqual(long x, long y) { return x <= y; }
		public override bool GreaterThan(long x, long y) { return x > y; }
		public override bool GreaterThanOrEqual(long x, long y) { return x >= y; }
		public override int Compare(long x, long y) { return x.CompareTo(y); }
		public override bool Equals(long x, long y) { return x == y; }
		public override long Max(long x, long y) { return Math.Max(x, y); }
		public override long Min(long x, long y) { return Math.Min(x, y); }
		public override long Successor(long value) { return checked((long)(value + 1)); }
		public override long Predecessor(long value) { return checked((long)(value - 1)); }
		public override long FromInt32(int value) { return checked((long)value); }
		public override int ToInt32(long value) { return checked((int)value); }
		public override bool HasBounds { get { return true; } }
		public override long MinValue { get { return long.MinValue; } }
		public override long MaxValue { get { return long.MaxValue; } }
		public override long Add(long x, long y) { return checked((long)(x + y)); }
		public override long Multiply(long x, long y) { return checked((long)(x * y)); }
		public override long Subtract(long x, long y) { return checked((long)(x - y)); }
		public override long Negate(long value) { return checked((long)(-value)); }
		public override long Abs(long value) { return checked((long)Math.Abs(value)); }
		public override long Sign(long value) { return (long)Math.Sign(value); }
		public override long DivideIntegral(long x, long y) { return checked((long)(x / y)); } // truncates toward 0
		public override long Modulus(long x, long y) { return checked((long)(x % y)); }
		public override long QuotientRemainder(long x, long y, out long remainder) { long q = Quotient(x, y); remainder = checked(x - q * y); return q; }
		public override long DivideIntegralModulus(long x, long y, out long modulus) { modulus = checked((long)(x % y)); return checked((long)(x / y)); }
		public override long Divide(long x, long y) { return checked((long)(x / y)); }
		public override long Reciprocal(long value) { return checked((long)(1 / value)); }
		public override long Zero { get { return 0; } }
		public override long FromDouble(double value) { return (long)value; }
		public override double ToDouble(long value) { return value; }
		public override bool TryParse(string s, out long result) { return long.TryParse(s, out result); }
	}
    
	internal class SByteMath : Math<sbyte>
	{
		public override bool IsUnsigned { get { return false; } }
		public override bool IsTwosComplement { get { return true; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(sbyte x, sbyte y) { return x < y; }
		public override bool LessThanOrEqual(sbyte x, sbyte y) { return x <= y; }
		public override bool GreaterThan(sbyte x, sbyte y) { return x > y; }
		public override bool GreaterThanOrEqual(sbyte x, sbyte y) { return x >= y; }
		public override int Compare(sbyte x, sbyte y) { return x.CompareTo(y); }
		public override bool Equals(sbyte x, sbyte y) { return x == y; }
		public override sbyte Max(sbyte x, sbyte y) { return Math.Max(x, y); }
		public override sbyte Min(sbyte x, sbyte y) { return Math.Min(x, y); }
		public override sbyte Successor(sbyte value) { return checked((sbyte)(value + 1)); }
		public override sbyte Predecessor(sbyte value) { return checked((sbyte)(value - 1)); }
		public override sbyte FromInt32(int value) { return checked((sbyte)value); }
		public override int ToInt32(sbyte value) { return value; }
		public override bool HasBounds { get { return true; } }
		public override sbyte MinValue { get { return sbyte.MinValue; } }
		public override sbyte MaxValue { get { return sbyte.MaxValue; } }
		public override sbyte Add(sbyte x, sbyte y) { return checked((sbyte)(x + y)); }
		public override sbyte Multiply(sbyte x, sbyte y) { return checked((sbyte)(x * y)); }
		public override sbyte Subtract(sbyte x, sbyte y) { return checked((sbyte)(x - y)); }
		public override sbyte Negate(sbyte value) { return checked((sbyte)(-value)); }
		public override sbyte Abs(sbyte value) { return (sbyte)Math.Abs(value); }
		public override sbyte Sign(sbyte value) { return (sbyte)Math.Sign(value); }
		public override sbyte DivideIntegral(sbyte x, sbyte y) { return checked((sbyte)(x / y)); } // truncates toward 0
		public override sbyte Modulus(sbyte x, sbyte y) { return checked((sbyte)(x % y)); }
		public override sbyte QuotientRemainder(sbyte x, sbyte y, out sbyte remainder) { sbyte q = Quotient(x, y); remainder = checked((sbyte)(x - q * y)); return q; }
		public override sbyte DivideIntegralModulus(sbyte x, sbyte y, out sbyte modulus) { modulus = checked((sbyte)(x % y)); return checked((sbyte)(x / y)); }
		public override sbyte Divide(sbyte x, sbyte y) { return checked((sbyte)(x / y)); }
		public override sbyte Reciprocal(sbyte value) { return checked((sbyte)(1 / value)); }
		public override sbyte Zero { get { return 0; } }
		public override sbyte FromDouble(double value) { return (sbyte)value; }
		public override double ToDouble(sbyte value) { return value; }
		public override bool TryParse(string s, out sbyte result) { return sbyte.TryParse(s, out result); }
	}
    
	internal class UInt16Math : Math<ushort>
	{
		public override bool IsUnsigned { get { return true; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(ushort x, ushort y) { return x < y; }
		public override bool LessThanOrEqual(ushort x, ushort y) { return x <= y; }
		public override bool GreaterThan(ushort x, ushort y) { return x > y; }
		public override bool GreaterThanOrEqual(ushort x, ushort y) { return x >= y; }
		public override int Compare(ushort x, ushort y) { return x.CompareTo(y); }
		public override bool Equals(ushort x, ushort y) { return x == y; }
		public override ushort Max(ushort x, ushort y) { return Math.Max(x, y); }
		public override ushort Min(ushort x, ushort y) { return Math.Min(x, y); }
		public override ushort Successor(ushort value) { return checked((ushort)(value + 1)); }
		public override ushort Predecessor(ushort value) { return checked((ushort)(value - 1)); }
		public override ushort FromInt32(int value) { return checked((ushort)value); }
		public override int ToInt32(ushort value) { return checked((int)value); }
		public override bool HasBounds { get { return true; } }
		public override ushort MinValue { get { return ushort.MinValue; } }
		public override ushort MaxValue { get { return ushort.MaxValue; } }
		public override ushort Add(ushort x, ushort y) { return checked((ushort)(x + y)); }
		public override ushort Multiply(ushort x, ushort y) { return checked((ushort)(x * y)); }
		public override ushort Subtract(ushort x, ushort y) { return checked((ushort)(x - y)); }
		public override ushort Negate(ushort value) { return checked((ushort)(-value)); }
		public override ushort Abs(ushort value) { return checked((ushort)Math.Abs(new decimal(value))); }
		public override ushort Sign(ushort value) { return (ushort)Math.Sign(new decimal(value)); }
		public override ushort DivideIntegral(ushort x, ushort y) { return checked((ushort)(x / y)); }   // truncates toward 0
		public override ushort Modulus(ushort x, ushort y) { return checked((ushort)(x % y)); }
		public override ushort QuotientRemainder(ushort x, ushort y, out ushort remainder) { ushort q = Quotient(x, y); remainder = checked((ushort)(x - q * y)); return q; }
		public override ushort DivideIntegralModulus(ushort x, ushort y, out ushort modulus) { modulus = checked((ushort)(x % y)); return checked((ushort)(x / y)); }
		public override ushort Divide(ushort x, ushort y) { return checked((ushort)(x / y)); }
		public override ushort Reciprocal(ushort value) { return checked((ushort)(1 / value)); }
		public override ushort Zero { get { return 0; } }
		public override ushort FromDouble(double value) { return (ushort)value; }
		public override double ToDouble(ushort value) { return value; }
		public override bool TryParse(string s, out ushort result) { return ushort.TryParse(s, out result); }
	}
    
	internal class UInt32Math : Math<uint>
	{
		public override bool IsUnsigned { get { return true; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(uint x, uint y) { return x < y; }
		public override bool LessThanOrEqual(uint x, uint y) { return x <= y; }
		public override bool GreaterThan(uint x, uint y) { return x > y; }
		public override bool GreaterThanOrEqual(uint x, uint y) { return x >= y; }
		public override int Compare(uint x, uint y) { return x.CompareTo(y); }
		public override bool Equals(uint x, uint y) { return x == y; }
		public override uint Max(uint x, uint y) { return Math.Max(x, y); }
		public override uint Min(uint x, uint y) { return Math.Min(x, y); }
		public override uint Successor(uint value) { return checked((uint)(value + 1)); }
		public override uint Predecessor(uint value) { return checked((uint)(value - 1)); }
		public override uint FromInt32(int value) { return checked((uint)value); }
		public override int ToInt32(uint value) { return checked((int)value); }
		public override bool HasBounds { get { return true; } }
		public override uint MinValue { get { return uint.MinValue; } }
		public override uint MaxValue { get { return uint.MaxValue; } }
		public override uint Add(uint x, uint y) { return checked((uint)(x + y)); }
		public override uint Multiply(uint x, uint y) { return checked((uint)(x * y)); }
		public override uint Subtract(uint x, uint y) { return checked((uint)(x - y)); }
		public override uint Negate(uint value) { return checked((uint)(-value)); }
		public override uint Abs(uint value) { return checked((uint)Math.Abs(value)); }
		public override uint Sign(uint value) { return (uint)Math.Sign(value); }
		public override uint DivideIntegral(uint x, uint y) { return checked((uint)(x / y)); }   // truncates toward 0
		public override uint Modulus(uint x, uint y) { return checked((uint)(x % y)); }
		public override uint QuotientRemainder(uint x, uint y, out uint remainder) { uint q = Quotient(x, y); remainder = checked((uint)(x - q * y)); return q; }
		public override uint DivideIntegralModulus(uint x, uint y, out uint modulus) { modulus = checked((uint)(x % y)); return checked((uint)(x / y)); }
		public override uint Divide(uint x, uint y) { return checked((uint)(x / y)); }
		public override uint Reciprocal(uint value) { return checked((uint)(1 / value)); }
		public override uint Zero { get { return 0; } }
		public override uint FromDouble(double value) { return (uint)value; }
		public override double ToDouble(uint value) { return value; }
		public override bool TryParse(string s, out uint result) { return uint.TryParse(s, out result); }
	}
    
	internal class UInt64Math : Math<ulong>
	{
		public override bool IsUnsigned { get { return true; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(ulong x, ulong y) { return x < y; }
		public override bool LessThanOrEqual(ulong x, ulong y) { return x <= y; }
		public override bool GreaterThan(ulong x, ulong y) { return x > y; }
		public override bool GreaterThanOrEqual(ulong x, ulong y) { return x >= y; }
		public override int Compare(ulong x, ulong y) { return x.CompareTo(y); }
		public override bool Equals(ulong x, ulong y) { return x == y; }
		public override ulong Max(ulong x, ulong y) { return Math.Max(x, y); }
		public override ulong Min(ulong x, ulong y) { return Math.Min(x, y); }
		public override ulong Successor(ulong value) { return checked((ulong)(value + 1)); }
		public override ulong Predecessor(ulong value) { return checked((ulong)(value - 1)); }
		public override ulong FromInt32(int value) { return checked((ulong)value); }
		public override int ToInt32(ulong value) { return checked((int)value); }
		public override bool HasBounds { get { return true; } }
		public override ulong MinValue { get { return ulong.MinValue; } }
		public override ulong MaxValue { get { return ulong.MaxValue; } }
		public override ulong Add(ulong x, ulong y) { return checked((ulong)(x + y)); }
		public override ulong Multiply(ulong x, ulong y) { return checked((ulong)(x * y)); }
		public override ulong Subtract(ulong x, ulong y) { return checked((ulong)(x - y)); }
		public override ulong Negate(ulong value) { if (value == 0UL) return value; throw new OverflowException(); }
		public override ulong Abs(ulong value) { return checked((ulong)Math.Abs(new decimal(value))); }
		public override ulong Sign(ulong value) { return (ulong)Math.Sign(new decimal(value)); }
		public override ulong DivideIntegral(ulong x, ulong y) { return checked((ulong)(x / y)); }   // truncates toward 0
		public override ulong Modulus(ulong x, ulong y) { return checked((ulong)(x % y)); }
		public override ulong QuotientRemainder(ulong x, ulong y, out ulong remainder) { ulong q = Quotient(x, y); remainder = checked((ulong)(x - q * y)); return q; }
		public override ulong DivideIntegralModulus(ulong x, ulong y, out ulong modulus) { modulus = checked((ulong)(x % y)); return checked((ulong)(x / y)); }
		public override ulong Divide(ulong x, ulong y) { return checked((ulong)(x / y)); }
		public override ulong Reciprocal(ulong value) { return checked((ulong)(1 / value)); }
		public override ulong Zero { get { return 0; } }
		public override ulong FromDouble(double value) { return (ulong)value; }
		public override double ToDouble(ulong value) { return value; }
		public override bool TryParse(string s, out ulong result) { return ulong.TryParse(s, out result); }
	}
    
	internal class BigIntegerMath : Math<BigInteger>
	{
		public override BigInteger Zero { get { return BigInteger.Zero; } }
		public override bool IsUnsigned { get { return false; } }
		public override bool IsTwosComplement { get { return false; } }
		public override bool IsFractional { get { return false; } }
		public override bool IsFloatingPoint { get { return false; } }
		public override bool LessThan(BigInteger x, BigInteger y) { return x < y; }
		public override bool LessThanOrEqual(BigInteger x, BigInteger y) { return x <= y; }
		public override bool GreaterThan(BigInteger x, BigInteger y) { return x > y; }
		public override bool GreaterThanOrEqual(BigInteger x, BigInteger y) { return x >= y; }
		public override int Compare(BigInteger x, BigInteger y) { return x.CompareTo(y); }
		public override bool Equals(BigInteger x, BigInteger y) { return x == y; }
		public override BigInteger Max(BigInteger x, BigInteger y) { return BigInteger.Max(x, y); }
		public override BigInteger Min(BigInteger x, BigInteger y) { return BigInteger.Min(x, y); }
		public override BigInteger Successor(BigInteger value) { return value + 1; }
		public override BigInteger Predecessor(BigInteger value) { return value - 1; }
		public override BigInteger FromInt32(int value) { return new BigInteger(value); }
		public override int ToInt32(BigInteger value) { return (int)value; }
		public override bool HasBounds { get { return false; } }
		public override BigInteger Add(BigInteger x, BigInteger y) { return x + y; }
		public override BigInteger Multiply(BigInteger x, BigInteger y) { return x * y; }
		public override BigInteger Subtract(BigInteger x, BigInteger y) { return x - y; }
		public override BigInteger Negate(BigInteger value) { return -value; }
		public override BigInteger Abs(BigInteger value) { return BigInteger.Abs(value); }
		public override BigInteger Sign(BigInteger value) { return value.Sign; }
		public override BigInteger DivideIntegral(BigInteger x, BigInteger y) { return x / y; }
		public override BigInteger Modulus(BigInteger x, BigInteger y) { return x % y; }
		public override BigInteger QuotientRemainder(BigInteger x, BigInteger y, out BigInteger remainder)
		{ var q = Quotient(x, y); remainder = x - q * y; return q; }
		public override BigInteger DivideIntegralModulus(BigInteger x, BigInteger y, out BigInteger modulus)
		{ return BigInteger.DivRem(x, y, out modulus); }
		public override BigInteger Divide(BigInteger x, BigInteger y) { return x / y; }
		public override BigInteger Reciprocal(BigInteger value) { return 1 / value; }
		public override BigInteger Log(BigInteger value) { return FromDouble(BigInteger.Log(value)); }
		public override BigInteger Log(BigInteger value, BigInteger newBase)
		{ return FromDouble(BigInteger.Log(value, ToDouble(newBase))); }
		public override BigInteger Pow(BigInteger value, BigInteger exp) { return BigInteger.Pow(value, ToInt32(exp)); }
		public override BigInteger FromDouble(double value) { return new BigInteger(value); }
		public override double ToDouble(BigInteger value) { return (double)value; }
		public override bool TryParse(string s, out BigInteger result) { return BigInteger.TryParse(s, out result); }
	}
}
