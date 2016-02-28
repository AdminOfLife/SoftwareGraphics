using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public struct Matrix
    {
        const int Size = 4;
        const string ElementFormat = "F3";

        static readonly Matrix identityMatrix = new Matrix(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);

        public static Matrix Identity
        {
            get { return identityMatrix; }
        }

        public float M11, M12, M13, M14;
        public float M21, M22, M23, M24;
        public float M31, M32, M33, M34;
        public float M41, M42, M43, M44;

        public Matrix(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        #region Static Methods

        public static bool Equals(ref Matrix m1, ref Matrix m2)
        {
            return
                m1.M11 == m2.M11 && m1.M12 == m2.M12 && m1.M13 == m2.M13 && m1.M14 == m2.M14 &&
                m1.M21 == m2.M21 && m1.M22 == m2.M22 && m1.M23 == m2.M23 && m1.M24 == m2.M24 &&
                m1.M31 == m2.M31 && m1.M32 == m2.M32 && m1.M33 == m2.M33 && m1.M34 == m2.M34 &&
                m1.M41 == m2.M41 && m1.M42 == m2.M42 && m1.M43 == m2.M43 && m1.M44 == m2.M44;
        }

        public static bool Equals(Matrix m1, Matrix m2)
        {
            return Equals(ref m1, ref m2);
        }

        public static void Negate(ref Matrix m)
        {
            m.M11 = -m.M11; m.M12 = -m.M12; m.M13 = -m.M13; m.M14 = -m.M14;
            m.M21 = -m.M21; m.M22 = -m.M22; m.M23 = -m.M23; m.M24 = -m.M24;
            m.M31 = -m.M31; m.M32 = -m.M32; m.M33 = -m.M33; m.M34 = -m.M34;
            m.M41 = -m.M41; m.M42 = -m.M42; m.M43 = -m.M43; m.M44 = -m.M44;
        }

        public static Matrix Negate(Matrix m)
        {
            Negate(ref m);
            return m;
        }

        public static void Add(ref Matrix m1, ref Matrix m2, out Matrix result)
        {
            result.M11 = m1.M11 + m2.M11;
            result.M12 = m1.M12 + m2.M12;
            result.M13 = m1.M13 + m2.M13;
            result.M14 = m1.M14 + m2.M14;

            result.M21 = m1.M21 + m2.M21;
            result.M22 = m1.M22 + m2.M22;
            result.M23 = m1.M23 + m2.M23;
            result.M24 = m1.M24 + m2.M24;

            result.M31 = m1.M31 + m2.M31;
            result.M32 = m1.M32 + m2.M32;
            result.M33 = m1.M33 + m2.M33;
            result.M34 = m1.M34 + m2.M34;

            result.M41 = m1.M41 + m2.M41;
            result.M42 = m1.M42 + m2.M42;
            result.M43 = m1.M43 + m2.M43;
            result.M44 = m1.M44 + m2.M44;
        }

        public static Matrix Add(Matrix m1, Matrix m2)
        {
            Matrix result;
            Add(ref m1, ref m2, out result);
            return result;
        }

        public static void Subtract(ref Matrix m1, ref Matrix m2, out Matrix result)
        {
            result.M11 = m1.M11 - m2.M11;
            result.M12 = m1.M12 - m2.M12;
            result.M13 = m1.M13 - m2.M13;
            result.M14 = m1.M14 - m2.M14;

            result.M21 = m1.M21 - m2.M21;
            result.M22 = m1.M22 - m2.M22;
            result.M23 = m1.M23 - m2.M23;
            result.M24 = m1.M24 - m2.M24;

            result.M31 = m1.M31 - m2.M31;
            result.M32 = m1.M32 - m2.M32;
            result.M33 = m1.M33 - m2.M33;
            result.M34 = m1.M34 - m2.M34;

            result.M41 = m1.M41 - m2.M41;
            result.M42 = m1.M42 - m2.M42;
            result.M43 = m1.M43 - m2.M43;
            result.M44 = m1.M44 - m2.M44;
        }

        public static Matrix Subtract(Matrix m1, Matrix m2)
        {
            Matrix result;
            Subtract(ref m1, ref m2, out result);
            return result;
        }

        public static void Multiply(ref Matrix m, float scalar)
        {
            float s = scalar;
            m.M11 *= s; m.M12 *= s; m.M13 *= s; m.M14 *= s;
            m.M21 *= s; m.M22 *= s; m.M23 *= s; m.M24 *= s;
            m.M31 *= s; m.M32 *= s; m.M33 *= s; m.M34 *= s;
            m.M41 *= s; m.M42 *= s; m.M43 *= s; m.M44 *= s;
        }

        public static Matrix Multiply(Matrix m, float scalar)
        {
            Multiply(ref m, scalar);
            return m;
        }

        private static void MultiplyInternal(ref Matrix a, ref Matrix b, out Matrix r)
        {
            // r.M_i_j = a.M_i1 * b.M1_j + a.M_i2 * b.M2_j + a.M_i3 * b.M3_j + a.M_i4 * b.M4_j;

            r.M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41;
            r.M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41;
            r.M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41;
            r.M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41;

            r.M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42;
            r.M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42;
            r.M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42;
            r.M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42;

            r.M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43;
            r.M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43;
            r.M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43;
            r.M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43;

            r.M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44;
            r.M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44;
            r.M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44;
            r.M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44;
        }

        public static void Multiply(ref Matrix m1, ref Matrix m2, out Matrix result)
        {
            MultiplyInternal(ref m1, ref m2, out result);
        }

        public static Matrix Multiply(Matrix m1, Matrix m2)
        {
            Matrix result;
            MultiplyInternal(ref m1, ref m2, out result);
            return result;
        }

        private static void TransposeInternal(ref Matrix m, out Matrix r)
        {
            r.M11 = m.M11; r.M12 = m.M21; r.M13 = m.M31; r.M14 = m.M41;
            r.M21 = m.M12; r.M22 = m.M22; r.M23 = m.M32; r.M24 = m.M42;
            r.M31 = m.M13; r.M32 = m.M23; r.M33 = m.M33; r.M34 = m.M43;
            r.M41 = m.M14; r.M42 = m.M24; r.M43 = m.M34; r.M44 = m.M44;
        }

        public static void Transpose(ref Matrix m, out Matrix result)
        {
            TransposeInternal(ref m, out result);
        }

        public static Matrix Transpose(Matrix m)
        {
            Matrix result;
            TransposeInternal(ref m, out result);
            return result;
        }

        public static float Determinant(ref Matrix m)
        {
            float c1 = m.M33 * m.M44 - m.M43 * m.M34;
            float c2 = m.M32 * m.M44 - m.M42 * m.M34;
            float c3 = m.M32 * m.M43 - m.M42 * m.M33;
            float c4 = m.M31 * m.M44 - m.M41 * m.M34;
            float c5 = m.M31 * m.M43 - m.M41 * m.M33;
            float c6 = m.M31 * m.M42 - m.M41 * m.M32;

            return m.M21 * (c1 + c2 + c3) + m.M22 * (c1 - c4 - c5)
                + m.M23 * (c6 - c2 - c4) + m.M24 * (c3 + c5 + c6);
        }

        public static float Determinant(Matrix m)
        {
            return Determinant(ref m);
        }

        public static void Invert(ref Matrix m, out Matrix result)
        {
            throw new NotImplementedException();
        }

        public static Matrix Invert(Matrix m)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods Overriding

        public override bool Equals(object obj)
        {
            Matrix? other = obj as Matrix?;
            if (other == null)
                return false;

            return Equals(other.Value);
        }

        public bool Equals(Matrix m)
        {
            return Equals(ref this, ref m);
        }

        public override int GetHashCode()
        {
            return (
                M11 + M12 + M13 + M14 +
                M21 + M22 + M23 + M24 +
                M31 + M32 + M33 + M34 +
                M41 + M42 + M43 + M44).GetHashCode();
        }

        private IEnumerable<float> GetValues()
        {
            yield return M11;
            yield return M12;
            yield return M13;
            yield return M14;

            yield return M21;
            yield return M22;
            yield return M23;
            yield return M24;

            yield return M31;
            yield return M32;
            yield return M33;
            yield return M34;

            yield return M31;
            yield return M32;
            yield return M33;
            yield return M34;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            var enumerator = GetValues().GetEnumerator();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    enumerator.MoveNext();
                    result.Append("M").Append(i + 1).Append(j + 1).Append("=")
                          .Append(enumerator.Current.ToString(ElementFormat, null))
                          .Append(" ");
                }

                // remove last space
                result.Length = result.Length - 1;
                result.AppendLine();
            }

            return result.ToString();
        }

        #endregion

        #region Operators Overloading

        public static bool operator ==(Matrix m1, Matrix m2)
        {
            return Equals(ref m1, ref m2);
        }

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !Equals(ref m1, ref m2);
        }

        public static Matrix operator +(Matrix m)
        {
            return m;
        }

        public static Matrix operator -(Matrix m)
        {
            Negate(ref m);
            return m;
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            Matrix result;
            Add(ref m1, ref m2, out result);
            return result;
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            Matrix result;
            Subtract(ref m1, ref m2, out result);
            return result;
        }

        public static Matrix operator *(Matrix m, float scalar)
        {
            Multiply(ref m, scalar);
            return m;
        }

        public static Matrix operator *(float scalar, Matrix m)
        {
            Multiply(ref m, scalar);
            return m;
        }

        public static Matrix operator /(Matrix m, float scalar)
        {
            Multiply(ref m, 1 / scalar);
            return m;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            Matrix result;
            MultiplyInternal(ref m1, ref m2, out result);
            return result;
        }

        #endregion
    }
}
