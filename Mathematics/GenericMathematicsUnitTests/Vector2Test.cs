using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GenericMathematics;
using NUnit.Framework;

namespace MathematicsUnitTests
{
    [TestFixture]
    public sealed class Vector2Test
    {
        [Test]
        public void BasicData()
        {
            Vector2<decimal> v = new Vector2<decimal>(2.0m, -7.0m);

            Assert.That(v.X, Is.EqualTo(2.0m));
            Assert.That(v.Y, Is.EqualTo(-7.0m));

            Assert.That(Vector2<float>.Zero.X, Is.EqualTo(0.0f));
            Assert.That(Vector2<double>.Zero.Y, Is.EqualTo(0.0));
            Assert.That(Vector2<sbyte>.UnitX.X, Is.EqualTo(1));
            Assert.That(Vector2<uint>.UnitX.Y, Is.EqualTo(0));
            Assert.That(Vector2<long>.UnitY.X, Is.EqualTo(0));
            Assert.That(Vector2<ushort>.UnitY.Y, Is.EqualTo(1));
        }

        [Test]
        public void EqualsAndCompare()
        {
            var vInt1 = new Vector2<int>(-1, 2);
            var vInt2 = new Vector2<int>(2, -1);

            var vFloat1 = new Vector2<float>(1.5f, -6f);
            var vFloat2 = new Vector2<float>(1.5f, -6f);

            var vLong1 = new Vector2<long>(-100, 75);
            var vLong2 = new Vector2<long>(-100, 75);

            var vByte1 = new Vector2<byte>(4, 77);
            var vByte2 = new Vector2<byte>(4, 88);

            // Not Equals
            Assert.That(!vInt1.Equals(vInt2));
            Assert.That(!vInt1.Equals((object)vInt2));
            Assert.That(!vInt1.Equals(new object()));
            Assert.That(!vInt2.Equals(vInt1));
            Assert.That(!vInt2.Equals((object)vInt1));
            Assert.That(!vInt2.Equals(new object()));

            // Equals
            Assert.That(vFloat1.Equals(vFloat2));
            Assert.That(vFloat1.Equals((object)vFloat2));
            Assert.That(vFloat2.Equals(vFloat1));
            Assert.That(vFloat2.Equals((object)vFloat1));

            // GetHashCode
            Assert.That(vLong1.GetHashCode(), Is.EqualTo(vLong2.GetHashCode()));

            // Compare
            Assert.That(vInt1.CompareTo(vInt2), Is.LessThan(0));
            Assert.That(vInt2.CompareTo(vInt1), Is.GreaterThan(0));
            Assert.That(vByte1.CompareTo(vByte2), Is.LessThan(0));
            Assert.That(vByte2.CompareTo(vByte1), Is.GreaterThan(0));
            Assert.That(vFloat1.CompareTo(vFloat2), Is.EqualTo(0));
            Assert.That(vFloat2.CompareTo(vFloat1), Is.EqualTo(0));
        }

        [Test]
        public void Operations()
        {
            var v1 = new Vector2<float>(3f, 4f);
            var v2 = new Vector2<int>(-4, 5);
            var v3 = new Vector2<int>(7, 2);

            // Length
            Assert.That(v1.Length(), Is.EqualTo((float)Math.Sqrt(Math.Pow(3f, 2) + Math.Pow(4f, 2))));
            Assert.That(v1.LengthSquared(), Is.EqualTo((float)(Math.Pow(3f, 2) + Math.Pow(4f, 2))));

            // Normalize
            Assert.That(v1.Normalize().Length(), Is.EqualTo(1f).Within(2).Ulps);
            Assert.That(Vector2<double>.Zero.Normalize(), Is.EqualTo(Vector2<double>.Zero));

            // Negate
            Assert.That(v1.Negate(), Is.EqualTo(new Vector2<float>(-3f, -4f)));

            // Add
            Assert.That(v2.Add(v3), Is.EqualTo(new Vector2<int>(3, 7)));
            Assert.That(v3.Add(v2), Is.EqualTo(new Vector2<int>(3, 7)));

            // Subtract
            Assert.That(v2.Subtract(v3), Is.EqualTo(new Vector2<int>(-11, 3)));
            Assert.That(v3.Subtract(v2), Is.EqualTo(new Vector2<int>(11, -3)));

            // Multiply (by scalar)
            Assert.That(v2.Multiply(3), Is.EqualTo(new Vector2<int>(-12, 15)));
            Assert.That(v3.Multiply(-2), Is.EqualTo(new Vector2<int>(-14, -4)));

            // Divide (by scalar)
            Assert.That(v2.Divide(2), Is.EqualTo(new Vector2<int>(-2, 2)));
            Assert.That(v3.Divide(-3), Is.EqualTo(new Vector2<int>(-2, 0)));

            // Dot
            Assert.That(v2.Dot(v3), Is.EqualTo(-18));
            Assert.That(v3.Dot(v2), Is.EqualTo(-18));

            // Clamp
            Assert.That(v2.Clamp(new Vector2<int>(-2, -10), new Vector2<int>(2, 10)),
                Is.EqualTo(new Vector2<int>(-2, 5)));
            Assert.That(v3.Clamp(new Vector2<int>(-8, 0), new Vector2<int>(8, 1)),
                Is.EqualTo(new Vector2<int>(7, 1)));

            // rotation by PI matrix
            var rotation = new Matrix<float>(4, 4, new double[]
            {
                Math.Cos(Math.PI), Math.Sin(Math.PI), 0, 0,
                -Math.Sin(Math.PI), Math.Cos(Math.PI), 0, 0,
                0, 0, 0, 0,
                0, 0, 0, 1,
            }.Select(d => (float)d).ToArray());

            // Transform3D
            Assert.That(v1.Transform3D(rotation), Is.EqualTo(-v1).Within(2).Ulps);

            // TransformNormal
            Assert.That(v1.TransformNormal(rotation), Is.EqualTo(-v1).Within(2).Ulps);
        }

        [Test]
        public void OperatorsOverloading()
        {
            var v1 = new Vector2<decimal>(-4, 7);
            var v2 = new Vector2<decimal>(3, 4);

            Assert.That(+v1, Is.EqualTo(v1));
            Assert.That(-v1, Is.EqualTo(v1.Negate()));
            Assert.That(v1 + v2, Is.EqualTo(v1.Add(v2)));
            Assert.That(v1 - v2, Is.EqualTo(v1.Subtract(v2)));
            Assert.That(v1 * 2.0m, Is.EqualTo(v1.Multiply(2.0m)));
            Assert.That(v1 / 3.0m, Is.EqualTo(v1.Divide(3.0m)));
            Assert.That(v1 == v2, Is.EqualTo(v1.Equals(v2)));
            Assert.That(v1 != v2, Is.EqualTo(!v1.Equals(v2)));
        }
    }
}
