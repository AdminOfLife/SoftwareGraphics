using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericMathematics
{
    public static class Algorithms
    {
        public static T Power<T>(T value, T exp)
        {
            var math = Math<T>.Default;

            if (math.IsFractional)
            {
                return math.Pow(value, exp);
            }
            else
            {
                T result = math.FromInt32(1);
                for (T i = math.FromInt32(0); math.LessThan(i, exp); i = math.Successor(i))
                {
                    result = math.Multiply(result, value);
                }

                return result;
            }
        }

        public static T Euclidean<T>(T a, T b)
        {
            var math = Math<T>.Default;

            if (math.LessThanOrEqual(a, math.Zero) ||
                math.LessThanOrEqual(b, math.Zero))
            {
                throw new ArgumentException(TextResources.Algorithms_ABMustBeGreaterZero);
            }

            while (math.GreaterThan(b, math.Zero))
            {
                T t = b;
                b = math.Modulus(a, b);
                a = t;
            }

            return a;
        }

        public static T ExtendedEuclidean<T>(T a, T b, out Vector2<T> coefficients)
        {
            var math = Math<T>.Default;

            if (math.LessThanOrEqual(a, math.Zero) ||
                math.LessThanOrEqual(b, math.Zero))
            {
                throw new ArgumentException(TextResources.Algorithms_ABMustBeGreaterZero);
            }

            var va = new Vector2<T>(math.FromInt32(1), math.FromInt32(1));
            var vb = new Vector2<T>(math.FromInt32(0), math.FromInt32(0));

            while (math.GreaterThan(b, math.Zero))
            {
                T t = b;
                T q = math.DivideIntegralModulus(a, b, out b);
                a = t;

                Vector2<T> vt = vb;
                vb = va - vb * q;
                va = vt;
            }

            coefficients = va;
            return a;
        }

        public static T CalculateLcm<T>(T a, T b)
        {
            var math = Math<T>.Default;

            if (math.LessThanOrEqual(a, math.Zero) ||
                math.LessThanOrEqual(b, math.Zero))
            {
                throw new ArgumentException(TextResources.Algorithms_ABMustBeGreaterZero);
            }

            return math.Divide(math.Multiply(a, b), Euclidean(a, b));
        }

        public static T CalculateLcm<T>(params T[] values)
        {
            if (values == null || values.Length == 0)
                throw new ArgumentException(TextResources.Algorithms_NeedArgumentLcm, "values");

            T currentLcm = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                currentLcm = CalculateLcm(currentLcm, values[i]);
            }

            return currentLcm;
        }

        public static T[] Factorization<T>(T n)
        {
            var math = Math<T>.Default;

            if (math.IsFractional)
                throw new NotSupportedException(TextResources.Algorithms_TypeMustBeInteger);

            int maxMultiplerIndex = (int)Math.Floor(Math.Log(math.ToDouble(n), 2));
            T[] factors = new T[maxMultiplerIndex + 1];
            int next = 1;

            factors[0] = n;
            int i = 0;
            bool exit = false;

            while (!exit)
            {
                factors[next] = FermatsMethod(ref factors[i]);

                if (!math.Equals(factors[next], math.Zero))
                {
                    // if success then try to factor using the same index
                    next++;
                }
                else
                {
                    // if number is prime try factor next multipler
                    i++;
                    if (i == maxMultiplerIndex)
                        exit = true;
                }
            }

            int newSize = Array.IndexOf(factors, math.Zero);
            if (newSize == -1)
                newSize = factors.Length;

            Array.Resize(ref factors, newSize);
            Array.Sort(factors);
            return factors;
        }

        private static T FermatsMethod<T>(ref T n)
        {
            var math = Math<T>.Default;
            var one = math.FromInt32(1);
            var two = math.FromInt32(2);

            if (math.LessThanOrEqual(n, two))
                return math.Zero;

            // n % 2 == 0
            if (math.Equals(math.Modulus(n, two), math.Zero))
            {
                n = math.Divide(n, two);
                return two;
            }

            T sqrt = math.FromDouble(
                Math.Floor(Math.Sqrt(math.ToDouble(n))));

            T rx = math.Add(math.Multiply(sqrt, two), one);
            T ry = one;
            T r = math.Subtract(math.Multiply(sqrt, sqrt), n);
            T half = math.Divide(n, two);

            while (!math.Equals(r, math.Zero))
            {
                r = math.Add(r, rx);
                rx = math.Add(rx, two);

                while (math.GreaterThan(r, math.Zero))
                {
                    r = math.Subtract(r, ry);
                    ry = math.Add(ry, two);
                }

                if (math.GreaterThan(ry, half))
                {
                    // number is prime
                    return math.Zero;
                }
            }

            T x = math.Divide(math.Subtract(rx, one), two);
            T y = math.Divide(math.Subtract(ry, one), two);

            n = math.Add(x, y);
            return math.Subtract(x, y);
        }
    }
}
