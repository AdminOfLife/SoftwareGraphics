using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeBase
{
    public static class DataCubeExtensions
    {
        public static void Rotate<T>(this DataCube<T> cube, Axis axis, int layer, bool clockwise)
        {
            if (cube == null)
                throw new ArgumentNullException("cube");
            if (layer < 0 || layer >= cube.Size)
                throw new ArgumentOutOfRangeException("layer");

            switch (axis)
            {
                case Axis.Left:
                    RotateLeftLayer(cube, layer);
                    break;
                case Axis.Top:
                    RotateTopLayer(cube, layer);
                    break;
                case Axis.Depth:
                    RotateDepthLayer(cube, layer);
                    break;
            }

            if (!clockwise)
            {
                switch (axis)
                {
                    case Axis.Left:
                        RotateLeftLayer(cube, layer);
                        RotateLeftLayer(cube, layer);
                        break;
                    case Axis.Top:
                        RotateTopLayer(cube, layer);
                        RotateTopLayer(cube, layer);
                        break;
                    case Axis.Depth:
                        RotateDepthLayer(cube, layer);
                        RotateDepthLayer(cube, layer);
                        break;
                }
            }
        }

        public static void Rotate<T>(this ICube<T> cube, Axis axis, bool clockwise)
        {
            if (cube == null)
                throw new ArgumentNullException("cube");

            switch (axis)
            {
                case Axis.Left:
                    RotateAroundLeft(cube);
                    break;
                case Axis.Top:
                    RotateAroundTop(cube);
                    break;
                case Axis.Depth:
                    RotateAroundDepth(cube);
                    break;
            }

            if (!clockwise)
            {
                switch (axis)
                {
                    case Axis.Left:
                        RotateAroundLeft(cube);
                        RotateAroundLeft(cube);
                        break;
                    case Axis.Top:
                        RotateAroundTop(cube);
                        RotateAroundTop(cube);
                        break;
                    case Axis.Depth:
                        RotateAroundDepth(cube);
                        RotateAroundDepth(cube);
                        break;
                }
            }
        }

        private static void RotateLeftLayer<T>(DataCube<T> cube, int left)
        {
            int max = cube.Size - 1;
            for (int i = 0; i < cube.Size / 2; i++)
            {
                for (int j = i; j < max - i; j++)
                {
                    T temp = cube[left, i, j];

                    cube[left, i, j] = cube[left, max - j, i];
                    cube[left, max - j, i] = cube[left, max - i, max - j];
                    cube[left, max - i, max - j] = cube[left, j, max - i];
                    cube[left, j, max - i] = temp;
                }
            }

            for (int i = 0; i < cube.Size; i++)
            {
                for (int j = 0; j < cube.Size; j++)
                {
                    IRotatable rotatable = cube[left, i, j] as IRotatable;
                    if (rotatable != null)
                        rotatable.RotateAroundLeft();
                }
            }
        }

        private static void RotateTopLayer<T>(DataCube<T> cube, int top)
        {
            if (top < 0 || top > cube.Size)
                throw new ArgumentOutOfRangeException("top");

            int max = cube.Size - 1;
            for (int i = 0; i < cube.Size / 2; i++)
            {
                for (int j = i; j < max - i; j++)
                {
                    T temp = cube[j, top, i];

                    cube[j, top, i] = cube[i, top, max - j];
                    cube[i, top, max - j] = cube[max - j, top, max - i];
                    cube[max - j, top, max - i] = cube[max - i, top, j];
                    cube[max - i, top, j] = temp;
                }
            }

            for (int i = 0; i < cube.Size; i++)
            {
                for (int j = 0; j < cube.Size; j++)
                {
                    IRotatable rotatable = cube[i, top, j] as IRotatable;
                    if (rotatable != null)
                        rotatable.RotateAroundTop();
                }
            }
        }

        private static void RotateDepthLayer<T>(DataCube<T> cube, int depth)
        {
            if (depth < 0 || depth > cube.Size)
                throw new ArgumentOutOfRangeException("depth");

            int max = cube.Size - 1;
            for (int i = 0; i < cube.Size / 2; i++)
            {
                for (int j = i; j < max - i; j++)
                {
                    T temp = cube[i, j, depth];

                    cube[i, j, depth] = cube[max - j, i, depth];
                    cube[max - j, i, depth] = cube[max - i, max - j, depth];
                    cube[max - i, max - j, depth] = cube[j, max - i, depth];
                    cube[j, max - i, depth] = temp;
                }
            }

            for (int i = 0; i < cube.Size; i++)
            {
                for (int j = 0; j < cube.Size; j++)
                {
                    IRotatable rotatable = cube[i, j, depth] as IRotatable;
                    if (rotatable != null)
                        rotatable.RotateAroundDepth();
                }
            }
        }

        private static void RotateAroundLeft<T>(ICube<T> cube)
        {
            var temp = cube[CubeSide.Up];
            cube[CubeSide.Up] = cube[CubeSide.Front];
            cube[CubeSide.Front] = cube[CubeSide.Down];
            cube[CubeSide.Down] = cube[CubeSide.Back];
            cube[CubeSide.Back] = temp;
        }

        private static void RotateAroundTop<T>(ICube<T> cube)
        {
            var temp = cube[CubeSide.Front];
            cube[CubeSide.Front] = cube[CubeSide.Left];
            cube[CubeSide.Left] = cube[CubeSide.Back];
            cube[CubeSide.Back] = cube[CubeSide.Right];
            cube[CubeSide.Right] = temp;
        }

        private static void RotateAroundDepth<T>(ICube<T> cube)
        {
            var temp = cube[CubeSide.Up];
            cube[CubeSide.Up] = cube[CubeSide.Right];
            cube[CubeSide.Right] = cube[CubeSide.Down];
            cube[CubeSide.Down] = cube[CubeSide.Left];
            cube[CubeSide.Left] = temp;
        }
    }
}
