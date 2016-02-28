using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoftwareGraphics;

using Vector3Int = GenericMathematics.Vector3<int>;

namespace RubiksCubeBase
{
    public static class CoordsHelper
    {
        public static int GetLayerFromAxis(Axis axis, CubeCoords coords)
        {
            switch (axis)
            {
                case Axis.Left:
                    return coords.Left;
                case Axis.Top:
                    return coords.Top;
                case Axis.Depth:
                    return coords.Depth;
                default:
                    throw new ArgumentException("Invalid axis value.", "axis");
            }
        }

        public static Axis GetAxisFromSide(CubeSide side)
        {
            switch (side)
            {
                case CubeSide.Left:
                case CubeSide.Right:
                    return Axis.Left;
                case CubeSide.Up:
                case CubeSide.Down:
                    return Axis.Top;
                case CubeSide.Front:
                case CubeSide.Back:
                    return Axis.Depth;
                default:
                    throw new ArgumentException("Invalid side value.", "side");
            }
        }

        public static Vector3 GetNormalFromSide(CubeSide side)
        {
            switch (side)
            {
                case CubeSide.Left:
                    return Vector3.Left;
                case CubeSide.Right:
                    return Vector3.Right;
                case CubeSide.Up:
                    return Vector3.Up;
                case CubeSide.Down:
                    return Vector3.Down;
                case CubeSide.Front:
                    return Vector3.Backward;
                case CubeSide.Back:
                    return Vector3.Forward;
                default:
                    throw new ArgumentException("Invalid side value.", "side");
            }
        }

        public static Vector3 GetPositionFromCoords(CubeCoords coords, int cubeSize)
        {
            if (cubeSize <= 0)
                throw new ArgumentOutOfRangeException("cubeSize");

            float shift = ((float)cubeSize - 1) / 2;

            return new Vector3(
                coords.Left - shift,
                -coords.Top + shift,
                -coords.Depth + shift) * 2f / cubeSize;
        }

        /// <summary>
        /// Gets vectors for iterating cube side, where each vector represents
        /// (left, top, depth) coords or difference in this coords.
        /// </summary>
        /// <param name="side">Cube side to iterate it's small cubes' coords.</param>
        /// <param name="nextColumn">Vector that should be added to go to next column.</param>
        /// <param name="nextRow">Vector that should be added to go to next row.</param>
        /// <returns>Start position of iterating, that must be multiplicated to max index (size - 1).</returns>
        private static Vector3Int GetSideIterators(
            CubeSide side,
            out Vector3Int nextColumn,
            out Vector3Int nextRow)
        {
            switch (side)
            {
                case CubeSide.Left:
                    nextColumn = new Vector3Int(0, 0, -1);
                    nextRow    = new Vector3Int(0, 1, 0);
                    return       new Vector3Int(0, 0, 1);
                case CubeSide.Right:
                    nextColumn = new Vector3Int(0, 0, 1);
                    nextRow    = new Vector3Int(0, 1, 0);
                    return       new Vector3Int(1, 0, 0);
                case CubeSide.Up:
                    nextColumn = new Vector3Int(1, 0, 0);
                    nextRow    = new Vector3Int(0, 0, -1);
                    return       new Vector3Int(0, 0, 1);
                case CubeSide.Down:
                    nextColumn = new Vector3Int(1, 0, 0);
                    nextRow    = new Vector3Int(0, 0, 1);
                    return       new Vector3Int(0, 1, 0);
                case CubeSide.Front:
                    nextColumn = new Vector3Int(1, 0, 0);
                    nextRow    = new Vector3Int(0, 1, 0);
                    return       new Vector3Int(0, 0, 0);
                case CubeSide.Back:
                    nextColumn = new Vector3Int(-1, 0, 0);
                    nextRow    = new Vector3Int(0, 1, 0);
                    return       new Vector3Int(1, 0, 1);
                default:
                    throw new ArgumentException("Invalid side value.", "side");
            }
        }

        public static IEnumerable<CubeCoords> EnumerateSide(CubeSide side, int cubeSize)
        {
            Vector3Int nextRow;
            Vector3Int nextColumn;

            Vector3Int position = GetSideIterators(
                side, out nextColumn, out nextRow);

            position *= cubeSize - 1;

            for (int i = 0; i < cubeSize; i++)
            {
                Vector3Int current = position;
                for (int j = 0; j < cubeSize; j++)
                {
                    yield return new CubeCoords(
                        current.X, current.Y, current.Z);

                    current += nextColumn;
                }

                position += nextRow;
            }
        }
    }
}
