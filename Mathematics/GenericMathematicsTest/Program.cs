using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericMathematics;
using System.Diagnostics;

namespace MathematicsTest
{
    static class Program
    {
        static readonly Complex i = Complex.ImOne;

        static double CalculateAtan(double x, int n)
        {
            double result = 0;
            int sign = 1;

            double squareX = Math.Pow(x, 2);
            double numerator = x;
            double denominator = 1;

            for (int i = 0; i < n; i++)
            {
                result += sign * numerator / denominator;
                
                sign = -sign;
                numerator *= squareX;
                denominator += 2;
            }

            return result;
        }

        static double CalculatePi(int n)
        {
            double piOver4 = 4 * CalculateAtan(0.2, n) - CalculateAtan(1.0 / 239.0, n);
            return piOver4 * 4;
        }

        static void ComplexRoots()
        {
            Complex number = -4;
            Complex[] roots = number.Roots(4);

            Console.WriteLine("Roots of root^4({0})", number);
            foreach (Complex complex in roots)
            {
                Console.WriteLine(complex);
            }
        }

        static void ComplexPow()
        {
            Complex number = 1 + i;
            Complex powered = number.Pow(25);
            Console.WriteLine("({0})^25 = {1}", number, powered);
        }

        static void PiCalculating()
        {
            Console.WriteLine("Pi Original   = {0}", Math.PI);
            Console.WriteLine("Pi Calculated = {0}", CalculatePi(10000));
        }

        static void MatrixMultiply()
        {
            var m1 = new Matrix<double>(2, 4, new double[]
            {
                0,  1, -2, 3,
                1, -1,  2, 1,
            });

            var m2 = new Matrix<double>(4, 3, new double[]
            {
                1, -1,  0,
                1,  1,  2,
                2,  1, -1,
                0,  1,  3,
            });

            var result = m1 * m2;

            Console.WriteLine("{1}{0}    XXX{0}{0}{2}{0}    ==={0}{0}{3}",
                Environment.NewLine, m1, m2, result);
        }

        static void GaussianElimination()
        {
            var m = new Matrix<double>(4, 4, new double[]
            {
                1,  3,  2,  0,
                2, -1,  3,  0,
                3, -5,  4,  0,
                1, 17,  4,  0,
            });

            m = m.ToReducedRowEchelonForm();
            Console.WriteLine(m.ToString(5, "F2"));
        }

        static void ConcatMatrix()
        {
            var m1 = new Matrix<double>(4, 4, new double[]
            {
                1, 1, 1, 1,
                2, 2, 2, 2,
                3, 3, 3, 3,
                4, 4, 4, 4,
            });

            var m2 = new Matrix<double>(4, 4, new double[]
            {
                5, 5, 5, 5,
                6, 6, 6, 6,
                7, 7, 7, 7,
                8, 8, 8, 8,
            });

            Console.WriteLine(m1.ConcatRight(m2));
        }

        static void InverseMatrix()
        {
            var m = new Matrix<double>(3, 3, new double[]
            {
                1, 1, 2,
                0, 2, 1,
                1, 1, 1,
            });

            Console.WriteLine("Inverse for");
            Console.WriteLine(m);
            Console.WriteLine("is 0.5 * matrix:");
            Console.WriteLine((m.Inverse() * 2).ToString(5, "F2"));

            m = new Matrix<double>(3, 3, new double[]
            {
                2,  1, -1,
                0,  1, -2,
                2, -1,  3,
            });

            Console.WriteLine("Is matrix");
            Console.WriteLine(m);
            Console.WriteLine("has no inversion? " + (m.Inverse() == null).ToString());
        }

        static void RankMatrix()
        {
            var m = new Matrix<Fraction>(6, 4, new Fraction[]
            {
                2, 1, 1, 1,
                1, 3, 1, 1,
                1, 1, 4, 1,
                1, 1, 1, 5,
                1, 1, 1, 1,
                1, 2, 3, 4,
            });

            var m1 = m.ToRowEchelonForm();
            //Fraction.SetCommonDenominator(m1.Elements);

            Console.WriteLine(m1.ToString(5));
            Console.WriteLine(m1.Rank());
        }

        private class CharMath : Math<char>
        {
            public override char Add(char x, char y)
            {
                return (char)((int)x + (int)y);
            }

            public override char Multiply(char x, char y)
            {
                return (char)((int)x * (int)y);
            }

            public override char Subtract(char x, char y)
            {
                return (char)((int)x - (int)y);
            }

            public override char DivideIntegralModulus(char x, char y, out char modulus)
            {
                int x1 = (int)x;
                int y1 = (int)y;
                modulus = (char)(x1 % y1);
                return (char)(x1 / y1);
            }
        }

        static void CharMatrix()
        {
            //Math<char>.SetDefault(new CharMath());

            var m1 = new Matrix<char>(3, 3, new char[]
            {
                'A', 'B', 'C',
                'D', 'E', 'F',
                'G', 'H', 'I',
            });

            var m2 = new Matrix<char>(3, 3, new int[]
            {
                1, 2, 3,
                4, 5, 6,
                7, 8, 9,
            }
            .Select(i => (char)i).ToArray());

            Console.WriteLine((m1 + m2).ToString(1));
        }

        private class StringMath : Math<string>
        {
            public override string Add(string x, string y)
            {
                return string.Concat(x, y);
            }

            public override string Multiply(string x, string y)
            {
                throw new NotImplementedException();
            }

            public override string Subtract(string x, string y)
            {
                throw new NotImplementedException();
            }

            public override string DivideIntegralModulus(string x, string y, out string modulus)
            {
                throw new NotImplementedException();
            }
        }

        static void StringMatrix()
        {
            //Math<string>.SetDefault(new StringMath());

            var m1 = new Matrix<string>(2, 2, new string[]
            {
                "hello", "hi",
                "привет", "хай",
            });

            var m2 = new Matrix<string>(2, 2, new string[]
            {
                " ", " ",
                " ", " ",
            });

            var m3 = new Matrix<string>(2, 2, new string[]
            {
                "world", "wld",
                "мир", "мирок",
            });

            Console.WriteLine((m1 + m2 + m3).ToString(15));
        }

        public static void FractionInverseMatrix()
        {
            var m = new Matrix<Fraction>(3, 3, new Fraction[]
            {
                1, 1, 2,
                0, 2, 1,
                1, 1, 1,
            });

            Console.WriteLine("Inverse for");
            Console.WriteLine(m);
            Console.WriteLine("is matrix:");
            Console.WriteLine(m.Inverse().ToString(5));
        }

        public static void Sqrt()
        {
            Console.Write("Input a: ");
            //Fraction a = Fraction.Parse(Console.ReadLine());
            double a = double.Parse(Console.ReadLine());

            Console.Write("Input x: ");
            //Fraction x = Fraction.Parse(Console.ReadLine());
            double x = double.Parse(Console.ReadLine());

            Console.WriteLine("Press Esc to break");
            int i = 0;

            while (true)
            {
                i++;
                //Console.WriteLine("{0:D2}: x = {1} ~ {2}", i, x, x.ToDouble());
                Console.WriteLine("{0:D2}: x = {1}", i, x);
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;

                try
                {
                    x = (x + a / x) / 2;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Max precision achieved");
                    break;
                }
            }
        }

        public static void ComplexParse()
        {
            string input = Console.ReadLine();

            while (input.Length > 0)
            {
                Complex c;
                if (Complex.TryParse(input, out c))
                {
                    Console.WriteLine("Succ! {0}", c);
                }
                else
                {
                    Console.WriteLine("Fail!");
                }

                input = Console.ReadLine();
            }
        }

        public static void ComplexMatrix()
        {
            var m = new Matrix<Complex>(3, 3, new Complex[]
            {
                i, 0, 0,
                0, i, 0,
                0, 0, i,
            });

            Console.WriteLine(m.ToString(3, "S"));

            Console.WriteLine((m * m).ToString(3, "S"));
            Console.WriteLine(m.Inverse().ToString(3, "S"));
        }

        public static void Determinant()
        {
            var m = new Matrix<Fraction>(3, 3, 0, new Fraction[]
            {
                 1,  0,  3,
                 2,  1,  2,
                 3,  0,  1,
            });

            Console.WriteLine(m);
            Console.WriteLine("Determinant: {0}", m.Determinant());
        }

        public static void Pow()
        {
            int a = 5;
            int b = 3;

            int p = Algorithms.Power(a, b);
            Console.WriteLine("{0}^{1} = {2}", a, b, p);
        }

        public static void MatrixFormat()
        {
            var m = new Matrix<Complex>(6);
            Random random = new Random();

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    m[i, j] = new Complex(random.NextDouble() * 2 - 1, random.NextDouble() * 2 - 1);
                }
            }

            Console.WriteLine("{0:W12SF2}", m);
            Console.WriteLine();
            Console.WriteLine("{0:W12SF2}", m.ToRowEchelonForm());
            Console.WriteLine();
            Console.WriteLine("{0:W12SF2}", m.ToReducedRowEchelonForm());
            Console.WriteLine();
            Console.WriteLine("Determinant = {0:SF2}", m.Determinant());
        }

        static Matrix<T> Row<T>(params T[] values)
        {
            return new Matrix<T>(1, values.Length, values);
        }

        static Matrix<T> Column<T>(params T[] values)
        {
            return new Matrix<T>(values.Length, 1, values);
        }

        struct Number<T>
        {
            static Math<T> math = Math<T>.Default;

            T Value;

            public Number(T value)
            {
                Value = value;
            }

            public static implicit operator Number<T>(T value)
            {
                return new Number<T>(value);
            }

            public static implicit operator T(Number<T> num)
            {
                return num.Value;
            }

            public static Number<T> operator +(Number<T> x, Number<T> y)
            {
                return math.Add(x.Value, y.Value);
            }

            public static Number<T> operator *(Number<T> x, Number<T> y)
            {
                return math.Multiply(x, y);
            }
        }

        static T MakeFoo<T>(Number<T> a)
        {
            return Math<T>.Default.Sin(a) + a * a;
        }

        static void Main(string[] args)
        {
            //var A = new Matrix<Fraction>(3, 3, new int[]
            //{
            //      1,  1, -1,
            //      0,  2, -1,
            //      0,  0,  1,
            //}.Select(i => new Fraction(i, 1)).ToArray());

            //var e1 = Column<Fraction>(1, 0, 0);
            //var e2 = Column<Fraction>(0, 1, 1);
            //var e3 = Column<Fraction>(1, 1, 0);

            //var C = e1.ConcatRight(e2).ConcatRight(e3);
            //var invC = C.Inverse();

            //var Ae = invC * A * C;
            //Console.WriteLine(Ae);

            Console.WriteLine(MakeFoo<double>(1.0));

            Console.ReadLine();
        }
    }
}