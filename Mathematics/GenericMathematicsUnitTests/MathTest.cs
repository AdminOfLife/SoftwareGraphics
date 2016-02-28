using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GenericMathematics;
using NUnit.Framework;

namespace MathematicsUnitTests
{
    [TestFixture]
    public sealed class MathTest
    {
        [Test]
        public void BasicByteAddition()
        {
            // assert that we are running tests in our universe
            Assert.That(Math<byte>.Default.Add(2, 2), Is.EqualTo(4));
        }

        [Test]
        public void AcquiringMathForTypes()
        {
            // system type maths
            Math<byte> byteMath = Math<byte>.Default;
            Math<sbyte> sbyteMath = Math<sbyte>.Default;
            Math<short> shortMath = Math<short>.Default;
            Math<ushort> ushortMath = Math<ushort>.Default;
            Math<int> intMath = Math<int>.Default;
            Math<uint> uintMath = Math<uint>.Default;
            Math<long> longMath = Math<long>.Default;
            Math<ulong> ulongMath = Math<ulong>.Default;
            Math<float> floatMath = Math<float>.Default;
            Math<double> doubleMath = Math<double>.Default;
            Math<decimal> decimalMath = Math<decimal>.Default;
            
            // user type maths
            Math<Fraction> fractionMath = Math<Fraction>.Default;
            Math<Complex> complexMath = Math<Complex>.Default;

            Assert.That(() => Math<Guid>.Default, Throws.TypeOf<NotSupportedException>());
            Assert.That(() => Math<IntPtr>.Default, Throws.TypeOf<NotSupportedException>());

            Assert.That(byteMath.Features, Is.EqualTo(
                MathFeatures.Bounded |
                MathFeatures.Unsigned));

            Assert.That(intMath.Features, Is.EqualTo(
                MathFeatures.Bounded |
                MathFeatures.TwosComplement));

            Assert.That(ushortMath.Features, Is.EqualTo(
                MathFeatures.Bounded |
                MathFeatures.Unsigned));

            Assert.IsTrue(longMath.IsTwosComplement);
            Assert.IsFalse(sbyteMath.IsUnsigned);
            Assert.IsFalse(uintMath.IsFractional);

            Assert.That(floatMath.Features, Is.EqualTo(
                MathFeatures.Bounded |
                MathFeatures.FloatingPoint |
                MathFeatures.Fractional));

            Assert.That(doubleMath.Features, Is.EqualTo(
                MathFeatures.Bounded |
                MathFeatures.FloatingPoint |
                MathFeatures.Fractional));

            Assert.That(decimalMath.Features, Is.EqualTo(
                MathFeatures.Bounded |
                MathFeatures.Fractional));
        }

        [Test]
        public void Comparing(
            [Values(
                typeof(byte), 
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal))]
            Type mathType)
        {
            var methodInfo = this.GetType().GetMethod("ComparingImplementation")
                .MakeGenericMethod(mathType);

            methodInfo.Invoke(this, null);
        }

        public void ComparingImplementation<T>()
        {
            var math = Math<T>.Default;

            Assert.That(math.FromInt32(0), Is.EqualTo(math.Zero));

            // Compare
            Assert.That(math.Compare(math.FromInt32(0), math.FromInt32(1)) < 0);
            Assert.That(math.Compare(math.FromInt32(20), math.FromInt32(10)) > 0);
            Assert.That(math.Compare(math.FromInt32(42), math.FromInt32(42)) == 0);

            // Equals
            Assert.IsTrue(math.Equals(math.FromInt32(111), math.FromInt32(111)));
            Assert.IsFalse(math.Equals(math.FromInt32(110), math.FromInt32(120)));

            // GetHashCode
            T numberForHashing = math.FromInt32(89);
            Assert.That(math.GetHashCode(numberForHashing), Is.EqualTo(numberForHashing.GetHashCode()));
            Assert.That(math.GetHashCode(numberForHashing), Is.EqualTo(math.GetHashCode(numberForHashing)));

            // LessThan, LessThanOrEqual
            Assert.IsTrue(math.LessThan(math.FromInt32(36), math.FromInt32(38)));
            Assert.IsFalse(math.LessThan(math.FromInt32(77), math.FromInt32(70)));
            Assert.IsTrue(math.LessThanOrEqual(math.FromInt32(37), math.FromInt32(39)));
            Assert.IsTrue(math.LessThanOrEqual(math.FromInt32(40), math.FromInt32(40)));
            Assert.IsFalse(math.LessThanOrEqual(math.FromInt32(76), math.FromInt32(69)));

            // GreaterThan, GreaterThanOrEqual
            Assert.IsTrue(math.GreaterThan(math.FromInt32(41), math.FromInt32(40)));
            Assert.IsFalse(math.GreaterThan(math.FromInt32(68), math.FromInt32(76)));
            Assert.IsTrue(math.GreaterThanOrEqual(math.FromInt32(44), math.FromInt32(43)));
            Assert.IsTrue(math.GreaterThanOrEqual(math.FromInt32(45), math.FromInt32(45)));
            Assert.IsFalse(math.GreaterThanOrEqual(math.FromInt32(66), math.FromInt32(75)));

            // Min, Max
            Assert.That(math.Min(math.FromInt32(5), math.FromInt32(6)), Is.EqualTo(math.FromInt32(5)));
            Assert.That(math.Max(math.FromInt32(8), math.FromInt32(7)), Is.EqualTo(math.FromInt32(8)));

            if (!math.IsUnsigned)
            {
                // Compare
                Assert.That(math.Compare(math.FromInt32(-13), math.FromInt32(32)) < 0);
                Assert.That(math.Compare(math.FromInt32(100), math.FromInt32(-100)) > 0);
                Assert.That(math.Compare(math.FromInt32(-33), math.FromInt32(-22)) < 0);
                Assert.That(math.Compare(math.FromInt32(-7), math.FromInt32(-10)) > 0);
                Assert.That(math.Compare(math.FromInt32(-43), math.FromInt32(-43)) == 0);

                // Equals
                Assert.IsTrue(math.Equals(math.FromInt32(-67), math.FromInt32(-67)));
                Assert.IsFalse(math.Equals(math.FromInt32(-67), math.FromInt32(-34)));

                // GetHashCode
                numberForHashing = math.FromInt32(-128);
                Assert.That(math.GetHashCode(numberForHashing), Is.EqualTo(numberForHashing.GetHashCode()));
                Assert.That(math.GetHashCode(numberForHashing), Is.EqualTo(math.GetHashCode(numberForHashing)));

                // LessThan, LessThanOrEqual
                Assert.IsTrue(math.LessThan(math.FromInt32(-36), math.FromInt32(38)));
                Assert.IsTrue(math.LessThan(math.FromInt32(-38), math.FromInt32(-36)));
                Assert.IsFalse(math.LessThan(math.FromInt32(77), math.FromInt32(-70)));
                Assert.IsFalse(math.LessThan(math.FromInt32(-70), math.FromInt32(-77)));
                Assert.IsTrue(math.LessThanOrEqual(math.FromInt32(-37), math.FromInt32(39)));
                Assert.IsTrue(math.LessThanOrEqual(math.FromInt32(-39), math.FromInt32(-37)));
                Assert.IsTrue(math.LessThanOrEqual(math.FromInt32(-40), math.FromInt32(-40)));
                Assert.IsFalse(math.LessThanOrEqual(math.FromInt32(76), math.FromInt32(-69)));
                Assert.IsFalse(math.LessThanOrEqual(math.FromInt32(76), math.FromInt32(-69)));

                // GreaterThan, GreaterThanOrEqual
                Assert.IsTrue(math.GreaterThan(math.FromInt32(41), math.FromInt32(-40)));
                Assert.IsTrue(math.GreaterThan(math.FromInt32(-40), math.FromInt32(-41)));
                Assert.IsFalse(math.GreaterThan(math.FromInt32(-68), math.FromInt32(76)));
                Assert.IsFalse(math.GreaterThan(math.FromInt32(-76), math.FromInt32(-68)));
                Assert.IsTrue(math.GreaterThanOrEqual(math.FromInt32(44), math.FromInt32(-43)));
                Assert.IsTrue(math.GreaterThanOrEqual(math.FromInt32(-43), math.FromInt32(-44)));
                Assert.IsTrue(math.GreaterThanOrEqual(math.FromInt32(-45), math.FromInt32(-45)));
                Assert.IsFalse(math.GreaterThanOrEqual(math.FromInt32(-66), math.FromInt32(75)));
                Assert.IsFalse(math.GreaterThanOrEqual(math.FromInt32(-75), math.FromInt32(-66)));

                // Min, Max
                Assert.That(math.Min(math.FromInt32(-5), math.FromInt32(6)), Is.EqualTo(math.FromInt32(-5)));
                Assert.That(math.Min(math.FromInt32(-5), math.FromInt32(-6)), Is.EqualTo(math.FromInt32(-6)));
                Assert.That(math.Max(math.FromInt32(8), math.FromInt32(-7)), Is.EqualTo(math.FromInt32(8)));
                Assert.That(math.Max(math.FromInt32(-8), math.FromInt32(-7)), Is.EqualTo(math.FromInt32(-7)));
            }
        }

        [Test]
        public void OrdinalAndBounds(
            [Values(
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal))]
            Type mathType)
        {
            var methodInfo = this.GetType().GetMethod("OrdinalAndBoundsImplementation")
                .MakeGenericMethod(mathType);

            methodInfo.Invoke(this, null);
        }

        public void OrdinalAndBoundsImplementation<T>()
        {
            var math = Math<T>.Default;

            Assert.That(math.ToInt32(math.FromInt32(1)), Is.EqualTo(1));

            if (!math.IsFloatingPoint)
            {
                Assert.That(math.Successor(math.FromInt32(10)), Is.EqualTo(math.FromInt32(11)));
                Assert.That(math.Predecessor(math.FromInt32(13)), Is.EqualTo(math.FromInt32(12)));

                Assert.That(math.EnumerateFrom(math.FromInt32(7)).Take(3).ToArray(),
                    Is.EqualTo(new T[]
                        {
                            math.FromInt32(7),
                            math.FromInt32(8),
                            math.FromInt32(9),
                        }).AsCollection);
            }

            Assert.That(math.EnumerateFrom(math.FromInt32(5)).Take(10),
                Is.EqualTo(Enumerable.Range(5, 10)).AsCollection);
            Assert.That(math.EnumerateFromTo(math.FromInt32(100), math.FromInt32(0)), Is.Empty);

            if (!math.IsUnsigned)
            {
                Assert.That(math.ToInt32(math.FromInt32(-1)), Is.EqualTo(-1));
            }

            // all system types have bounds
            Assert.That(math.HasBounds);
            Assert.That(math.LessThan(math.MinValue, math.MaxValue));
        }

        [Test]
        public void Arithmetics(
            [Values(
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal))]
            Type mathType)
        {
            var methodInfo = this.GetType().GetMethod("ArithmeticsImplementation")
                .MakeGenericMethod(mathType);

            methodInfo.Invoke(this, null);
        }

        public void ArithmeticsImplementation<T>()
        {
            var math = Math<T>.Default;

            // Addition, Subtraction, Multiplication
            Assert.That(math.Add(math.FromInt32(2), math.FromInt32(3)), 
                Is.EqualTo(math.FromInt32(5)));
            Assert.That(math.Subtract(math.FromInt32(42), math.FromInt32(12)),
                Is.EqualTo(math.FromInt32(30)));
            Assert.That(math.Multiply(math.FromInt32(25), math.FromInt32(3)),
                Is.EqualTo(math.FromInt32(75)));
            Assert.That(math.Multiply(math.FromInt32(43), math.Zero),
                Is.EqualTo(math.Zero));

            if (math.IsFractional)
            {
                Assert.That(math.FromDouble(0.5),
                    Is.EqualTo(math.Divide(math.FromInt32(1), math.FromInt32(2))));

                if (math.IsFloatingPoint)
                {
                    T add = math.Add(math.MaxValue, math.FromInt32(1));
                    Assert.That(math.Equals(add, math.MaxValue));
                    T sub = math.Subtract(math.MinValue, math.FromInt32(1));
                    Assert.That(math.Equals(sub, math.MinValue));

                    Assert.That(math.DivideIntegral(math.FromInt32(7), math.FromInt32(2)),
                    Is.EqualTo(math.FromInt32(3)).Within(1).Ulps);
                }

                // Division
                Assert.That(math.Divide(math.FromInt32(15), math.FromInt32(2)),
                    Is.GreaterThan(math.FromInt32(7)).And.LessThan(math.FromInt32(8)));
                Assert.That(math.Modulus(math.FromInt32(11), math.FromInt32(5)),
                    Is.EqualTo(math.FromInt32(1)));
                Assert.That(math.Remainder(math.FromInt32(-17), math.FromInt32(3)),
                    Is.EqualTo(math.FromInt32(1)));

                // "mathematical" quotitent rounds to -inf
                Assert.That(math.Quotient(math.FromInt32(-9), math.FromInt32(2)),
                    Is.EqualTo(math.FromInt32(-5)));

                // 2^(-1) == 1 / 2
                Assert.That(math.Reciprocal(math.FromInt32(2)),
                    Is.EqualTo(math.Divide(math.FromInt32(1), math.FromInt32(2))));
            }
            else
            {
                // double truncs when converted to integer
                Assert.That(math.FromDouble(1.5),
                    Is.EqualTo(math.FromInt32(1)));
                Assert.That(math.FromDouble(2.5),
                    Is.EqualTo(math.FromInt32(2)));

                Assert.That(() => math.Add(math.MaxValue, math.FromInt32(1)),
                    Throws.TypeOf<OverflowException>());
                Assert.That(() => math.Subtract(math.MinValue, math.FromInt32(1)),
                    Throws.TypeOf<OverflowException>());

                Assert.That(math.Divide(math.FromInt32(11), math.FromInt32(2)),
                    Is.EqualTo(math.FromInt32(5)));
                Assert.That(math.Modulus(math.FromInt32(10), math.FromInt32(4)),
                    Is.EqualTo(math.FromInt32(2)));
                // 1 / x for integer is always 0
                Assert.That(math.Reciprocal(math.FromInt32(27)),
                    Is.EqualTo(math.Zero));

                // Can't divide integer by ZERO
                Assert.That(() => math.Divide(math.FromInt32(1), math.Zero),
                    Throws.TypeOf<DivideByZeroException>());

                if (!math.IsUnsigned)
                {
                    // Addition
                    Assert.That(math.Add(math.FromInt32(-50), math.FromInt32(60)),
                        Is.EqualTo(math.FromInt32(10)));
                    Assert.That(math.Add(math.FromInt32(-100), math.FromInt32(-12)),
                        Is.EqualTo(math.FromInt32(-112)));

                    // Subtraction
                    Assert.That(math.Subtract(math.FromInt32(80), math.FromInt32(90)),
                        Is.EqualTo(math.FromInt32(-10)));
                    Assert.That(math.Subtract(math.FromInt32(40), math.FromInt32(-30)),
                        Is.EqualTo(math.FromInt32(70)));
                    Assert.That(math.Subtract(math.FromInt32(-33), math.FromInt32(22)),
                        Is.EqualTo(math.FromInt32(-55)));

                    // Multiplication
                    Assert.That(math.Multiply(math.FromInt32(6), math.FromInt32(-7)),
                        Is.EqualTo(-42));
                    Assert.That(math.Multiply(math.FromInt32(-20), math.FromInt32(-1)),
                        Is.EqualTo(20));

                    // Division
                    Assert.That(math.Quotient(math.FromInt32(-13), math.FromInt32(3)),
                        Is.EqualTo(math.FromInt32(-5)));
                    Assert.That(math.Remainder(math.FromInt32(-10), math.FromInt32(3)),
                        Is.EqualTo(2));

                    // normal integer division rounds toward 0
                    Assert.That(math.Divide(math.FromInt32(-9), math.FromInt32(2)),
                        Is.EqualTo(math.FromInt32(-4)));
                    // "mathematical" quotitent rounds toward -inf
                    Assert.That(math.Quotient(math.FromInt32(-9), math.FromInt32(2)),
                        Is.EqualTo(math.FromInt32(-5)));
                }
            }

            if (!math.IsUnsigned)
            {
                // Negate
                Assert.That(math.Negate(math.FromInt32(104)),
                    Is.EqualTo(math.FromInt32(-104)));
                Assert.That(math.Negate(math.FromInt32(-104)),
                    Is.EqualTo(math.FromInt32(104)));
                // Abs
                Assert.That(math.Abs(math.FromInt32(77)),
                    Is.EqualTo(77));
                Assert.That(math.Abs(math.FromInt32(-77)),
                    Is.EqualTo(77));
                // Sign
                Assert.That(math.Sign(math.FromInt32(88)),
                    Is.EqualTo(math.FromInt32(1)));
                Assert.That(math.Sign(math.FromInt32(-88)),
                    Is.EqualTo(math.FromInt32(-1)));
                Assert.That(math.Sign(math.Zero),
                    Is.EqualTo(math.Zero));
            }
        }

        [Test]
        public void FloatFunctions(
            [Values(
                typeof(float),
                typeof(double),
                typeof(decimal))]
            Type mathType)
        {
            var methodInfo = this.GetType().GetMethod("FloatFunctionsImplementation")
                .MakeGenericMethod(mathType);

            methodInfo.Invoke(this, null);
        }

        public void FloatFunctionsImplementation<T>()
        {
            var math = Math<T>.Default;

            if (math.IsFloatingPoint)
            {
                // NaN, +inf, -inf
                T nan = math.Divide(math.Zero, math.Zero);
                T pinf = math.Divide(math.FromInt32(1), math.Zero);
                T ninf = math.Divide(math.FromInt32(-1), math.Zero);
                Assert.That(math.IsNaN(nan));
                Assert.That(math.IsInfinite(pinf));
                Assert.That(math.IsInfinite(ninf));
            }
            else
            {
                Assert.That(() => math.Divide(math.FromInt32(1), math.Zero),
                    Throws.TypeOf<DivideByZeroException>());
            }

            // Truncate
            Assert.That(math.Truncate(math.FromDouble(3.3)),
                Is.EqualTo(math.FromInt32(3)));
            Assert.That(math.Truncate(math.FromDouble(-3.3)),
                Is.EqualTo(math.FromInt32(-3)));

            // Rounding (MidpointRounding.AwayFromZero by default)
            Assert.That(math.Round(math.FromDouble(1.5)),
                Is.EqualTo(math.FromInt32(2)));
            Assert.That(math.Round(math.FromDouble(2.5)),
                Is.EqualTo(math.FromInt32(3)));
            Assert.That(math.Round(math.FromDouble(-1.5)),
                Is.EqualTo(math.FromInt32(-2)));

            // Ceiling
            Assert.That(math.Ceiling(math.FromDouble(7.2)),
                Is.EqualTo(math.FromInt32(8)));
            Assert.That(math.Ceiling(math.FromDouble(-7.2)),
                Is.EqualTo(math.FromInt32(-7)));

            // Floor
            Assert.That(math.Floor(math.FromDouble(8.9)),
                Is.EqualTo(math.FromInt32(8)));
            Assert.That(math.Floor(math.FromDouble(-8.9)),
                Is.EqualTo(math.FromInt32(-9)));
        }

        [Test]
        public void GenericTypeValue(
            [Values(
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal))]
            Type mathType)
        {
            var methodInfo = this.GetType().GetMethod("GenericTypeValueImplementation")
                .MakeGenericMethod(mathType);

            methodInfo.Invoke(this, null);
        }

        public void GenericTypeValueImplementation<T>()
        {
            var vMath = Math<Vector2<T>>.Default;
            var tMath = Math<T>.Default;

            var v2 = vMath.Add(
                new Vector2<T>(tMath.FromInt32(1), tMath.FromInt32(2)),
                new Vector2<T>(tMath.FromInt32(3), tMath.FromInt32(4)));

            Assert.That(v2, Is.EqualTo(new Vector2<T>(tMath.FromInt32(4), tMath.FromInt32(6))));
        }
        
        [Test]
        public void DivideRoundAndModulus(
            [Values(
                typeof(byte),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(decimal))]
            Type mathType)
        {
            var methodInfo = this.GetType().GetMethod("DivideRoundAndModulusImplementation")
                .MakeGenericMethod(mathType);

            methodInfo.Invoke(this, null);
        }

        public void DivideRoundAndModulusImplementation<T>()
        {
        	var math = Math<T>.Default;
        	
        	if (!math.IsFractional)
        	{
        		Assert.That(math.Quotient(math.FromInt32(7), math.FromInt32(2)),
        			Is.EqualTo(math.FromInt32(3)));
        		
        		Assert.That(math.DivideIntegral(math.FromInt32(7), math.FromInt32(2)),
        			Is.EqualTo(math.FromInt32(3)));

                Assert.That(math.Remainder(math.FromInt32(8), math.FromInt32(3)),
                    Is.EqualTo(math.FromInt32(2)));

                Assert.That(math.Modulus(math.FromInt32(9), math.FromInt32(2)),
                    Is.EqualTo(math.FromInt32(1)));

                if (!math.IsUnsigned)
                {
                    Assert.That(math.Modulus(math.FromInt32(-7), math.FromInt32(2)),
                        Is.EqualTo(math.FromInt32(-1)));

                    Assert.That(math.Quotient(math.FromInt32(-7), math.FromInt32(2)),
                        Is.EqualTo(math.FromInt32(-4)));

                    // it seems that modulus(x, y) = x - sign(x) * (abs(x) div abs(y)) * abs(y)

                    Assert.That(math.Modulus(math.FromInt32(-8), math.FromInt32(3)),
                        Is.EqualTo(math.FromInt32(-2)));

                    Assert.That(math.Modulus(math.FromInt32(8), math.FromInt32(-3)),
                        Is.EqualTo(math.FromInt32(2)));

                    Assert.That(math.Modulus(math.FromInt32(-9), math.FromInt32(2)),
                        Is.EqualTo(math.FromInt32(-1)));

                    Assert.That(math.Modulus(math.FromInt32(9), math.FromInt32(-2)),
                        Is.EqualTo(math.FromInt32(1)));

                    // remainder(x, y) = x - quotitent(x, y) * y

                    Assert.That(math.Remainder(math.FromInt32(-8), math.FromInt32(3)),
                        Is.EqualTo(math.FromInt32(1)));

                    Assert.That(math.Remainder(math.FromInt32(8), math.FromInt32(-3)),
                        Is.EqualTo(math.FromInt32(-1)));

                    Assert.That(math.Remainder(math.FromInt32(-9), math.FromInt32(2)),
                        Is.EqualTo(math.FromInt32(1)));

                    Assert.That(math.Remainder(math.FromInt32(9), math.FromInt32(-2)),
                        Is.EqualTo(math.FromInt32(-1)));
                }
        	}
        }
    }
}
