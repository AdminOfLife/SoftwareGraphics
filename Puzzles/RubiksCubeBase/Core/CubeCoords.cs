using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeBase
{
    public struct CubeCoords
    {
        public int Left;
        public int Top;
        public int Depth;

        public CubeCoords(int left, int top, int depth)
        {
            Left = left;
            Top = top;
            Depth = depth;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CubeCoords?;
            if (other == null)
                return false;

            return Equals((CubeCoords)other);
        }

        public bool Equals(CubeCoords other)
        {
            return Left == other.Left && Top == other.Top && Depth == other.Depth;
        }

        public override int GetHashCode()
        {
            return unchecked(Left + 100 * Top + 100 * 100 * Depth);
        }

        public override string ToString()
        {
            return string.Format("{0}; {1}; {3}", Left, Top, Depth);
        }

        public static bool operator ==(CubeCoords first, CubeCoords second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(CubeCoords first, CubeCoords second)
        {
            return !first.Equals(second);
        }

        public static IEnumerable<CubeCoords> EnumerateCube(int cubeSize)
        {
            for (int i = 0; i < cubeSize; i++)
            {
                for (int j = 0; j < cubeSize; j++)
                {
                    for (int k = 0; k < cubeSize; k++)
                    {
                        yield return new CubeCoords(i, j, k);
                    }
                }
            }
        }
    }
}
