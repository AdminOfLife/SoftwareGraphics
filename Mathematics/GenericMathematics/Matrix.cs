using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericMathematics
{
    /// <summary>
    /// Represents a matrix of values.
    /// </summary>
    /// <typeparam name="T">The numerical type to use in calculations.</typeparam>
    public sealed class Matrix<T> : IEquatable<Matrix<T>>, ICloneable, IFormattable
    {
        const int DefaultElementWidth = 5;
        const float DefaultFloatingPointEpsilon = 1e-07f;

        static readonly Math<T> math = Math<T>.Default;

        /// <summary>
        /// Contains a number of rows in matrix.
        /// </summary>
        public readonly int Rows;
        /// <summary>
        /// Contains a number of columns in matrix.
        /// </summary>
        public readonly int Columns;
        /// <summary>
        /// Contains elements of matrix a row-major order.
        /// </summary>
        public readonly T[] Elements;
        /// <summary>
        /// Contains a value that any value less than or equal it considered
        /// as zero in such calculations as gaussian elimination.
        /// </summary>
        public T Epsilon;

        /// <summary>
        /// Returns a default epsilon for <typeparamref name="T"/>.
        /// </summary>
        public static T DefaultEpsilon
        {
            get
            {
                if (Math<T>.Default.IsFloatingPoint)
                    return Math<T>.Default.FromDouble(DefaultFloatingPointEpsilon);
                else
                    return Math<T>.Default.Zero;
            }
        }

        /// <summary>
        /// Gets or sets a value in specified position.
        /// </summary>
        /// <param name="i">Row index, counts from zero.</param>
        /// <param name="j">Column index, counts from zero.</param>
        /// <returns>Current value in specified position.</returns>
        public T this[int i, int j]
        {
            get { return Elements[i * Columns + j]; }
            set
            {
                Elements[i * Columns + j] = value;
            }
        }

        /// <summary>
        /// Initialize a new instance of <see cref="T:GenericMathematics.Matrix`1"/> with
        /// default epsilon and row count equals to column count.
        /// </summary>
        /// <param name="size">Row and column count.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public Matrix(int size)
            : this(size, size, DefaultEpsilon)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="T:GenericMathematics.Matrix`1"/> with
        /// default epsilon and specified row and column count.
        /// </summary>
        /// <param name="rows">Row count.</param>
        /// <param name="columns">Column count.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public Matrix(int rows, int columns)
            : this(rows, columns, DefaultEpsilon)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="T:GenericMathematics.Matrix`1"/>
        /// with specified epsilon, row and column count.
        /// </summary>
        /// <param name="rows">Row count.</param>
        /// <param name="columns">Column count.</param>
        /// <param name="epsilon">
        /// A value that any value less than or equal it considered
        /// as zero in such calculations as gaussian elimination.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public Matrix(int rows, int columns, T epsilon)
        {
            if (rows <= 0)
                throw new ArgumentOutOfRangeException("rows");
            if (columns <= 0)
                throw new ArgumentOutOfRangeException("columns");
            
            Rows = rows;
            Columns = columns;
            Elements = new T[Rows * Columns];
            Epsilon = epsilon;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="T:GenericMathematics.Matrix`1"/> with
        /// default epsilon, specified row, column count and initial elements values.
        /// </summary>
        /// <param name="rows">Row count.</param>
        /// <param name="columns">Column count.</param>
        /// <param name="values">Initial elements values.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public Matrix(int rows, int columns, T[] values)
            : this(rows, columns, DefaultEpsilon, values)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="T:GenericMathematics.Matrix`1"/> with
        /// specified epsilon, row, column count and initial elements values.
        /// </summary>
        /// <param name="rows">Row count.</param>
        /// <param name="columns">Column count.</param>
        /// <param name="epsilon">
        /// A value that any value less than or equal it considered
        /// as zero in such calculations as gaussian elimination.
        /// </param>
        /// <param name="values">Initial elements values.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public Matrix(int rows, int columns, T epsilon, T[] values)
            : this(rows, columns, epsilon)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != rows * columns)
                throw new ArgumentException(TextResources.Matrix_LengthEqualsRowsColumns, "values");

            Array.Copy(values, Elements, Elements.Length);
            Epsilon = epsilon;
        }

        #region Methods Overriding

        private bool WithinEpsilon(T value)
        {
            return math.LessThanOrEqual(math.Abs(value), Epsilon);
        }

        /// <summary>
        /// Returns a value indicating whether this instance equals a specified matrix.
        /// </summary>
        /// <param name="obj">A matrix compare to this instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="obj"/> equals current instance;
        /// <c>false</c> otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            Matrix<T> other = obj as Matrix<T>;
            if (ReferenceEquals(obj, null))
                return false;

            return this.Equals(other);
        }

        /// <summary>
        /// Returns a value indicating whether this instance equals a specified matrix.
        /// </summary>
        /// <param name="m">A matrix compare to this instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="m"/> equals current instance;
        /// <c>false</c> otherwise.
        /// </returns>
        public bool Equals(Matrix<T> m)
        {
            if (ReferenceEquals(m, null))
                return false;

            Matrix<T> m1 = this;
            Matrix<T> m2 = m;

            if (m1.Rows != m2.Rows ||
                m1.Columns != m2.Columns)
            {
                return false;
            }

            for (int i = 0; i < m1.Elements.Length; i++)
            {
                T diff = math.Abs(math.Subtract(m1.Elements[i], m2.Elements[i]));
                if (math.LessThanOrEqual(diff, Epsilon))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            int hashCode = Elements[0].GetHashCode();
            for (int i = 1; i < Elements.Length; i++)
            {
                hashCode ^= Elements[i].GetHashCode();
            }

            return hashCode;
        }

        /// <summary>
        /// Creates an exact copy of this matrix.
        /// </summary>
        /// <returns>The matrix this method creates, casted as an object.</returns>
        public object Clone()
        {
            var clone = new Matrix<T>(Rows, Columns, Epsilon);
            Array.Copy(Elements, clone.Elements, Elements.Length);
            return clone;
        }

        /// <summary>
        /// Converts this instance to its string representation in
        /// traditional table form.
        /// </summary>
        /// <returns>
        /// The string representation of matrix as <see cref="Rows"/>
        /// lines with <see cref="Columns"/> elements in each, where an element is
        /// padded to default width with spaces.
        /// </returns>
        public override string ToString()
        {
            return ToString(DefaultElementWidth);
        }

        /// <summary>
        /// Converts this instance to its string representation in
        /// traditional table form, with specified minimum width for an element.
        /// </summary>
        /// <param name="elementWidth">
        /// Minimum width for string representation of an element in table;
        /// if width less than this value, element is padded to it with spaces.
        /// </param>
        /// <returns>
        /// The string representation of matrix as <see cref="Rows"/>
        /// lines with <see cref="Columns"/> elements in each, where an element is
        /// padded to specified width with spaces.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public string ToString(int elementWidth)
        {
            return ToString(elementWidth, null, null);
        }

        /// <summary>
        /// Converts this instance to its string representation in
        /// traditional table form, with specified minimum width and format for an element.
        /// </summary>
        /// <param name="elementWidth">
        /// Minimum width for string representation of an element in table;
        /// if width less than this value, element is padded to it with spaces.
        /// </param>
        /// <param name="elementFormat">A format that applied to each element.</param>
        /// <returns>
        /// The string representation of matrix as <see cref="Rows"/>
        /// lines with <see cref="Columns"/> elements in each, where an element is
        /// padded to specified width with spaces and formatted with <paramref name="elementFormat"/>.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        /// <exception cref="System.FormatException"/>
        public string ToString(int elementWidth, string elementFormat)
        {
            return ToString(elementWidth, elementFormat, null);
        }

        private string ToString(int elementWidth, string elementFormat, IFormatProvider formatProvider)
        {
            if (elementWidth < 0)
                throw new ArgumentOutOfRangeException("elementWidth");

            bool formattable = typeof(IFormattable).IsAssignableFrom(typeof(T));
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    string element;
                    if (formattable && elementFormat != null)
                        element = ((IFormattable)this[i, j]).ToString(elementFormat, formatProvider);
                    else
                        element = this[i, j].ToString();

                    result.Append(element.PadLeft(elementWidth) + " ");
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            string elementFormat = format;
            var spec = FormatParser.FirstSpecifierOrDefault(ref format, 'W', -1);

            if (char.ToUpperInvariant(spec.Item1) == 'W')
            {
                return ToString(
                    spec.Item2 == -1 ? DefaultElementWidth : spec.Item2,
                    format,
                    formatProvider);
            }
            else
            {
                return ToString(DefaultElementWidth, elementFormat, formatProvider);
            }
        }

        #endregion

        #region Operations

        private static void CheckSizes(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1.Rows != m2.Rows ||
                m1.Columns != m2.Columns)
            {
                throw new ArgumentException(TextResources.Matrix_SizesMustBeEqual);
            }
        }

        private static void CheckSizes(Matrix<T> m1, Matrix<T> m2,
            bool isMatrix1RowsCheck, bool isMatrix2RowsCheck)
        {
            int checkValue1 = isMatrix1RowsCheck ? m1.Rows : m1.Columns;
            int checkValue2 = isMatrix2RowsCheck ? m2.Rows : m2.Columns;

            if (checkValue1 != checkValue2)
            {
                string property1 = isMatrix1RowsCheck ? "Rows" : "Columns";
                string property2 = isMatrix2RowsCheck ? "Rows" : "Columns";

                throw new ArgumentException(string.Format(
                    TextResources.Matrix_RowsOrColumnsEqual, property1, property2));
            }
        }

        /// <summary>
        /// Returns a negated matrix.
        /// </summary>
        /// <returns>A matrix with the same size, where each element is negated.</returns>
        public Matrix<T> Negate()
        {
            Matrix<T> result = new Matrix<T>(Rows, Columns, Epsilon);
            for (int i = 0; i < Elements.Length; i++)
            {
                result.Elements[i] = math.Negate(Elements[i]);
            }

            return result;
        }

        /// <summary>
        /// Returns a sum of this matrix and another. Matrix sizes must be equals.
        /// </summary>
        /// <param name="m">A matrix to add to this instance.</param>
        /// <returns>A sum of this matrix and another.</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public Matrix<T> Add(Matrix<T> m)
        {
            if (ReferenceEquals(m, null))
                throw new ArgumentNullException("m");
            
            Matrix<T> m1 = this;
            Matrix<T> m2 = m;
            CheckSizes(m1, m2);

            Matrix<T> result = new Matrix<T>(m1.Rows, m1.Columns, Epsilon);
            for (int i = 0; i < m1.Elements.Length; i++)
            {
                result.Elements[i] = math.Add(m1.Elements[i], m2.Elements[i]);
            }

            return result;
        }

        /// <summary>
        /// Returns a difference between this matrix and another. Matrix sizes must be equals.
        /// </summary>
        /// <param name="m">A matrix to subtract from this instance.</param>
        /// <returns>A difference between this matrix and another.</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public Matrix<T> Subtract(Matrix<T> m)
        {
            if (ReferenceEquals(m, null))
                throw new ArgumentNullException("m");

            Matrix<T> m1 = this;
            Matrix<T> m2 = m;
            CheckSizes(m1, m2);

            Matrix<T> result = new Matrix<T>(m1.Rows, m1.Columns, Epsilon);
            for (int i = 0; i < m1.Elements.Length; i++)
            {
                result.Elements[i] = math.Subtract(m1.Elements[i], m2.Elements[i]);
            }

            return result;
        }

        /// <summary>
        /// Returns this matrix multiplied by scalar.
        /// </summary>
        /// <param name="scalar">A value to multiply each element of matrix.</param>
        /// <returns>
        /// A matrix that represents this matrix with each
        /// element multiplied by <paramref name="scalar"/>.
        /// </returns>
        public Matrix<T> Multiply(T scalar)
        {
            Matrix<T> result = new Matrix<T>(Rows, Columns, Epsilon);
            for (int i = 0; i < Elements.Length; i++)
            {
                result.Elements[i] = math.Multiply(Elements[i], scalar);
            }

            return result;
        }

        /// <summary>
        /// Returns an algebraic multiplication of this matrix an another.
        /// </summary>
        /// <param name="m">
        /// A matrix on which current matrix would be multuplied;
        /// the matrix must have <see cref="Rows"/> equals to <see cref="Columns"/>
        /// of current matrix.
        /// </param>
        /// <returns>An algebraic multiplication of this matrix an another.</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public Matrix<T> Multiply(Matrix<T> m)
        {
            if (ReferenceEquals(m, null))
                throw new ArgumentNullException("m");

            Matrix<T> m1 = this;
            Matrix<T> m2 = m;
            // is m1.Columns == m2.Rows
            CheckSizes(m1, m2, false, true);

            Matrix<T> result = new Matrix<T>(m1.Rows, m2.Columns, Epsilon);

            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m2.Columns; j++)
                {
                    T sum = math.Zero;

                    for (int k = 0; k < m1.Columns; k++)
                    {
                        sum = math.Add(sum,
                            math.Multiply(m1[i, k], m2[k, j]));
                    }

                    result[i, j] = sum;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a transposed matrix.
        /// </summary>
        /// <returns>
        /// A transposed matrix, where element from position (i, j)
        /// will be on the position (j, i).
        /// </returns>
        public Matrix<T> Transpose()
        {
            Matrix<T> result = new Matrix<T>(Columns, Rows, Epsilon);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    result[j, i] = this[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Returns this matrix with another concateneted at the bottom.
        /// </summary>
        /// <param name="m">
        /// A matrix to concatenate to this instance;
        /// the matrix must have <see cref="Columns"/> equals to current matrix.
        /// </param>
        /// <returns>
        /// A matrix that have row count equals to sum of this
        /// matrix row count and another, with up part equals this matrix
        /// and bottom part equals <paramref name="m"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public Matrix<T> ConcatBottom(Matrix<T> m)
        {
            if (ReferenceEquals(m, null))
                throw new ArgumentNullException("m");

            Matrix<T> top = this;
            Matrix<T> bottom = m;
            // is m1.Columns == m2.Columns
            CheckSizes(top, bottom, false, false);

            Matrix<T> result = new Matrix<T>(top.Rows + bottom.Rows, top.Columns, Epsilon);

            int topElementsCount = top.Elements.Length;
            Array.Copy(top.Elements, result.Elements, topElementsCount);
            Array.Copy(bottom.Elements, 0, result.Elements, topElementsCount, bottom.Elements.Length);

            return result;
        }

        /// <summary>
        /// Returns this matrix with another concateneted at the right.
        /// </summary>
        /// <param name="m">
        /// A matrix to concatenate to this instance;
        /// the matrix must have <see cref="Rows"/> equals to current matrix.
        /// </param>
        /// <returns>
        /// A matrix that have column count equals to sum of this
        /// matrix column count and another, with left part equals this matrix
        /// and right part equals <paramref name="m"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public Matrix<T> ConcatRight(Matrix<T> m)
        {
            if (ReferenceEquals(m, null))
                throw new ArgumentNullException("m");

            Matrix<T> left = this;
            Matrix<T> right = m;
            // is m1.Rows == m2.Rows
            CheckSizes(left, right, true, true);

            Matrix<T> result = new Matrix<T>(
                left.Rows, left.Columns + right.Columns, left.Epsilon);

            for (int j = 0; j < left.Columns; j++)
            {
                for (int i = 0; i < left.Rows; i++)
                {
                    result[i, j] = left[i, j];
                }
            }

            for (int j = 0; j < right.Columns; j++)
            {
                for (int i = 0; i < right.Rows; i++)
                {
                    result[i, j + left.Columns] = right[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a determinant of matrix.
        /// </summary>
        /// <returns>
        /// A determinant of this matrix, computed using
        /// transformation to row echelon form.
        /// </returns>
        /// <exception cref="System.ArgumentException"/>
        public T Determinant()
        {
            if (Rows != Columns)
                throw new ArgumentException(TextResources.Matrix_MustBeSquareMatrix);

            Matrix<T> rowEchelon = (Matrix<T>)this.Clone();

            int swaps;
            rowEchelon.GaussianElimination(false, out swaps);

            T determinant = math.FromInt32(swaps % 2 == 0 ? 1 : -1);

            for (int i = 0; i < Rows; i++)
            {
                determinant = math.Multiply(determinant, rowEchelon[i, i]);
            }

            return determinant;
        }

        /// <summary>
        /// Returns a matrix without specified row and column.
        /// </summary>
        /// <param name="row">A row to exclude from matrix.</param>
        /// <param name="column">A column to exclude from matrix.</param>
        /// <returns>
        /// A matrix that have <c>Rows - 1</c> rows and 
        /// <c>Columns - 1</c> columns, with other elements equals
        /// to current matrix.
        /// </returns>
        /// <exception cref="System.ArgumentException"/>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public Matrix<T> MinorMatrix(int row, int column)
        {
            if (Rows != Columns)
                throw new ArgumentException(TextResources.Matrix_MustBeSquareMatrix);
            if (Rows == 1)
                throw new ArgumentException(TextResources.Matrix_SizeMustBeMoreOne);
            if (row >= Rows || row < 0)
                throw new ArgumentOutOfRangeException(TextResources.Matrix_RowMustBeInRangle);
            if (column >= Columns || column < 0)
                throw new ArgumentOutOfRangeException(TextResources.Matrix_ColumnMustBeInRangle);

            Matrix<T> minor = new Matrix<T>(Rows - 1);
            int iMinor = 0;
            int jMinor = 0;

            for (int i = 0; i < Rows; i++)
            {
                if (i == row)
                    continue;

                for (int j = 0; j < Columns; j++)
                {
                    if (j == column)
                        continue;

                    minor[iMinor, jMinor] = this[i, j];
                    jMinor++;
                }

                jMinor = 0;
                iMinor++;
            }

            return minor;
        }

        /// <summary>
        /// Returns an inverse of matrix if exists; <c>null</c> otherwise.
        /// Matrix must be square (<see cref="Rows"/> equals <see cref="Columns"/>).
        /// </summary>
        /// <returns>
        /// An inverse matrix, computed using transformation to row
        /// echelon form, if <see cref="Determinant"/> is not zero;
        /// <c>null</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentException"/>
        public Matrix<T> Inverse()
        {
            if (Rows != Columns)
                throw new ArgumentException(TextResources.Matrix_MustBeSquareMatrix);

            Matrix<T> extended = this.ConcatRight(Matrix<T>.Identity(Rows, Epsilon));
            extended = extended.ToReducedRowEchelonForm();

            for (int i = extended.Rows - 1; i >= 0; i--)
            {
                bool allZeroes = true;
                for (int j = 0; j < this.Columns; j++)
                {
                    if (!WithinEpsilon(extended[i, j]))
                        allZeroes = false;
                }

                if (allZeroes)
                {
                    return null;
                }
            }

            for (int i = 1; i < extended.Rows; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    T f = extended[j, i];
                    extended[j, i] = math.Zero;

                    for (int k = i + 1; k < extended.Columns; k++)
                    {
                        extended[j, k] = math.Subtract(
                            extended[j, k],
                            math.Multiply(f, extended[i, k]));
                    }
                }
            }

            Matrix<T> transposed = extended.Transpose();
            Matrix<T> result = new Matrix<T>(this.Rows, this.Rows, Epsilon);
            Array.Copy(transposed.Elements, Elements.Length, result.Elements, 0, Elements.Length);
            result = result.Transpose();

            return result;
        }

        /// <summary>
        /// Gets the rank of matrix by rows.
        /// </summary>
        /// <returns>A non-negative integer value less than or equals <see cref="Rows"/>.</returns>
        public int Rank()
        {
            Matrix<T> echelonForm = this.ToRowEchelonForm();
            int allZeroesRows = echelonForm.Rows;

            for (int i = echelonForm.Rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < echelonForm.Columns; j++)
                {
                    if (!WithinEpsilon(echelonForm[i, j]))
                    {
                        allZeroesRows--;
                        break;
                    }
                }
            }

            return echelonForm.Rows - allZeroesRows;
        }

        #endregion

        #region Operators Overloading

        /// <summary>
        /// Returns a value indicating whether one matrix equals another.
        /// </summary>
        /// <param name="m1">First matrix to compare.</param>
        /// <param name="m2">Second matrix to comare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="m1"/> equals <paramref name="m2"/>;
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool operator ==(Matrix<T> m1, Matrix<T> m2)
        {
            if (ReferenceEquals(m1, null))
                return ReferenceEquals(m2, null);
            else
                return m1.Equals(m2);
        }

        /// <summary>
        /// Returns a value indicating whether one matrix not equals another.
        /// </summary>
        /// <param name="m1">First matrix to compare.</param>
        /// <param name="m2">Second matrix to comare.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="m1"/> not equals <paramref name="m2"/>;
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool operator !=(Matrix<T> m1, Matrix<T> m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// Returns a copy of matrix.
        /// </summary>
        /// <param name="m">A matrix to copy.</param>
        /// <returns>A matrix that equals <paramref name="m"/>.</returns>
        public static Matrix<T> operator +(Matrix<T> m)
        {
            return (Matrix<T>)m.Clone();
        }

        /// <summary>
        /// Returns a negated copy of matrix.
        /// </summary>
        /// <param name="m">A matrix to negate.</param>
        /// <returns>A matrix with the same size, where each element is negated.</returns>
        public static Matrix<T> operator -(Matrix<T> m)
        {
            return m.Negate();
        }

        /// <summary>
        /// Returns a sum of two matrices. Matrix sizes must be equals.
        /// </summary>
        /// <param name="m1">First matrix to add.</param>
        /// <param name="m2">Second matrix to add.</param>
        /// <returns>A sum of <paramref name="m1"/> and <paramref name="m2"/>.</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public static Matrix<T> operator +(Matrix<T> m1, Matrix<T> m2)
        {
            return m1.Add(m2);
        }

        /// <summary>
        /// Returns a difference between two matrices. Matrix sizes must be equals.
        /// </summary>
        /// <param name="m1">First matrix (minuend).</param>
        /// <param name="m2">Second matrix (subtrahend).</param>
        /// <returns>A difference between <paramref name="m1"/> and <paramref name="m2"/>.</returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public static Matrix<T> operator -(Matrix<T> m1, Matrix<T> m2)
        {
            return m1.Subtract(m2);
        }

        /// <summary>
        /// Returns a matrix multiplied by scalar.
        /// </summary>
        /// <param name="m">A multiplied matrix.</param>
        /// <param name="scalar">A value to multiply each element of matrix.</param>
        /// <returns>
        /// A matrix that represents <paramref name="m"/> with each
        /// element multiplied by <paramref name="scalar"/>.
        /// </returns>
        public static Matrix<T> operator *(Matrix<T> m, T scalar)
        {
            return m.Multiply(scalar);
        }

        /// <summary>
        /// Returns a matrix multiplied by scalar.
        /// </summary>
        /// <param name="m">A multiplied matrix.</param>
        /// <param name="scalar">A value to multiply each element of matrix.</param>
        /// <returns>
        /// A matrix that represents <paramref name="m"/> with each
        /// element multiplied by <paramref name="scalar"/>.
        /// </returns>
        public static Matrix<T> operator *(T scalar, Matrix<T> m)
        {
            return m.Multiply(scalar);
        }

        /// <summary>
        /// Returns an algebraic multiplication of two matrices.
        /// </summary>
        /// <param name="m1">Left matrix to multiply.</param>
        /// <param name="m2">Right matrix to multiply.</param>
        /// <returns>
        /// An algebraic multiplication of
        /// <paramref name="m1"/> and <paramref name="m2"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.ArgumentException"/>
        public static Matrix<T> operator *(Matrix<T> m1, Matrix<T> m2)
        {
            return m1.Multiply(m2);
        }

        /// <summary>
        /// Returns a matrix divided by scalar.
        /// </summary>
        /// <param name="m">A matrix to divide.</param>
        /// <param name="scalar">A value to divide each element of matrix.</param>
        /// <returns>
        /// A matrix that represents <paramref name="m"/> with each
        /// element divided by <paramref name="scalar"/>.
        /// </returns>
        public static Matrix<T> operator /(Matrix<T> m, T scalar)
        {
            return m.Multiply(math.Reciprocal(scalar)); // m * (1 / a)
        }

        #endregion

        #region Standard Matrices

        private static void CheckNewMatrixSize(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", TextResources.Matrix_SizeMustBeMoreZero);
        }

        /// <summary>
        /// Returns a identity matrix with specified size and default epsilon.
        /// </summary>
        /// <param name="size">A size of identity matrix.</param>
        /// <returns>
        /// A matrix with <paramref name="size"/> rows and columns,
        /// that have value of one on the main diagonal and all other
        /// elements equal zero.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public static Matrix<T> Identity(int size)
        {
            return Identity(size, DefaultEpsilon);
        }

        /// <summary>
        /// Returns a identity matrix with specified size and epsilon.
        /// </summary>
        /// <param name="size">A size of identity matrix.</param>
        /// <param name="epsilon">
        /// A value that any value less than or equal it considered
        /// as zero in such calculations as gaussian elimination.
        /// </param>
        /// <returns>
        /// A matrix with <paramref name="size"/> rows and columns,
        /// that have value of one on the main diagonal and all other
        /// elements equal zero.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public static Matrix<T> Identity(int size, T epsilon)
        {
            CheckNewMatrixSize(size);

            Matrix<T> result = new Matrix<T>(size, size, epsilon);

            for (int i = 0; i < result.Elements.Length; i++)
            {
                result.Elements[i] = math.Zero;
            }

            for (int i = 0; i < size; i++)
            {
                result[i, i] = math.FromInt32(1);
            }

            return result;
        }

        #endregion

        #region Standard Algorithms

        private void GaussianElimination(bool reducedForm, out int swapCount)
        {
            int i = 0;
            int j = 0;
            swapCount = 0;

            while (i < Rows && j < Columns)
            {
                // Find max element index in column j
                int maxIndexInColumn = i;
                for (int k = i + 1; k < Rows; k++)
                {
                    if (math.GreaterThan(
                            math.Abs(this[k, j]),
                            math.Abs(this[maxIndexInColumn, j])))
                    {
                        maxIndexInColumn = k;
                    }
                }

                if (!WithinEpsilon(this[maxIndexInColumn, j]))
                {
                    swapCount++;

                    // Swap rows i and maxIndexInColumn
                    for (int s = 0; s < Columns; s++)
                    {
                        T t = this[i, s];
                        this[i, s] = this[maxIndexInColumn, s];
                        this[maxIndexInColumn, s] = t;
                    }

                    // Divide each element in row i by this[i, j]
                    if (reducedForm)
                    {
                        T reciprocal = math.Reciprocal(this[i, j]);  // 1 / this[i, j]
                        this[i, j] = math.FromInt32(1);
                        for (int s = j + 1; s < Columns; s++)
                        {
                            this[i, s] = math.Multiply(this[i, s], reciprocal);
                        }
                    }

                    // Subtract (this[k, j] / this[i, j]) * row i from row k
                    for (int k = i + 1; k < Rows; k++)
                    {
                        T f = math.Divide(this[k, j], this[i, j]);
                        this[k, j] = math.Zero;

                        for (int s = j + 1; s < Columns; s++)
                        {
                            this[k, s] = math.Subtract(
                                this[k, s],
                                math.Multiply(f, this[i, s]));
                        }
                    }

                    i++;
                }
                else
                {
                    this[maxIndexInColumn, j] = math.Zero;
                }

                j++;
            }
        }

        /// <summary>
        /// Returns a row echelon form of this instance.
        /// </summary>
        /// <returns>
        /// A matrix transformed to row echelon form using
        /// standart transformations.
        /// </returns>
        public Matrix<T> ToRowEchelonForm()
        {
            Matrix<T> result = (Matrix<T>)this.Clone();

            int swaps;
            result.GaussianElimination(false, out swaps);

            return result;
        }

        /// <summary>
        /// Returns a reduced row echelon form of this instance
        /// (where first non zero element in each row is one).
        /// </summary>
        /// <returns>
        /// A matrix transformed to reduced row echelon form 
        /// using standart transformations.
        /// </returns>
        public Matrix<T> ToReducedRowEchelonForm()
        {
            Matrix<T> result = (Matrix<T>)this.Clone();

            int swaps;
            result.GaussianElimination(true, out swaps);

            return result;
        }

        #endregion
    }
}
