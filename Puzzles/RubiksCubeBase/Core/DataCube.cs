using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeBase
{
    public sealed class DataCube<T> : IRotatable, ICloneable
    {
        int size;
        T[] data;

        public int Size
        {
            get { return size; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                size = value;
                Array.Resize(ref data, size * size * size);
            }
        }

        public T this[int left, int top, int depth]
        {
            get
            {
                if (left  < 0 || left  >= size ||
                    top   < 0 || top   >= size ||
                    depth < 0 || depth >= size)
                {
                    throw new ArgumentOutOfRangeException(
                        "left, top and depth must be > 0 and < Size.", (Exception)null);
                }

                return data[(depth * size + top) * size + left];
            }
            set
            {
                if (left  < 0 || left  >= size ||
                    top   < 0 || top   >= size ||
                    depth < 0 || depth >= size)
                {
                    throw new ArgumentOutOfRangeException(
                        "left, top and depth must be > 0 and < Size.", (Exception)null);
                }

                data[(depth * size + top) * size + left] = value;
            }
        }

        public T this[CubeCoords coords]
        {
            get { return this[coords.Left, coords.Top, coords.Depth]; }
            set { this[coords.Left, coords.Top, coords.Depth] = value; }
        }

        public DataCube(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");

            this.size = size;
            data = new T[size * size * size];
        }

        void IRotatable.RotateAroundLeft()
        {
            for (int i = 0; i < size; i++)
            {
                this.Rotate(Axis.Left, i, true);
            }
        }

        void IRotatable.RotateAroundTop()
        {
            for (int i = 0; i < size; i++)
            {
                this.Rotate(Axis.Top, i, true);
            }
        }

        void IRotatable.RotateAroundDepth()
        {
            for (int i = 0; i < size; i++)
            {
                this.Rotate(Axis.Depth, i, true);
            }
        }

        public object Clone()
        {
            DataCube<T> clone = new DataCube<T>(Size);
            for (int i = 0; i < data.Length; i++)
            {
                ICloneable clonable = data[i] as ICloneable;
                if (data[i] != null)
                    clone.data[i] = (T)clonable.Clone();
                else
                    clone.data[i] = data[i];
            }

            return clone;
        }
    }
}
