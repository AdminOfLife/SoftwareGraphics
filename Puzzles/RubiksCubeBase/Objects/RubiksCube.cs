using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace RubiksCubeBase
{
    public sealed class RubiksCube : DrawableObject
    {
        static readonly Color[] defaultColors = new[]
        {
            Color.Black,
            Color.Red,
            Color.Orange,
            Color.Blue,
            Color.Green,
            Color.White,
            Color.Yellow,
        };

        static readonly CubeSide[,] rotationRules = new CubeSide[,]
        {
            // left
            { CubeSide.Up, CubeSide.Back },
            { CubeSide.Back, CubeSide.Down },
            { CubeSide.Down, CubeSide.Front },
            { CubeSide.Front, CubeSide.Up },
            // top
            { CubeSide.Left, CubeSide.Front },
            { CubeSide.Front, CubeSide.Right },
            { CubeSide.Right, CubeSide.Back },
            { CubeSide.Back, CubeSide.Left },
            // depth
            { CubeSide.Up, CubeSide.Left },
            { CubeSide.Left, CubeSide.Down },
            { CubeSide.Down, CubeSide.Right },
            { CubeSide.Right, CubeSide.Up },
        };

        DataCube<SmallCube> dataCube;
        CubeView<DefaultCubePart> view;
        Color[] colorMap;

        public int Size
        {
            get { return dataCube.Size; }
        }

        public bool AnimationInProgress
        {
            get { return view.AnimationInProgress; }
        }

        public RubiksCube(int size)
            : this(size, null)
        {
        }

        public RubiksCube(int size, Color[] colorMap)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", "size must be > 0.");
            if (colorMap == null)
                colorMap = defaultColors;
            if (colorMap.Length < 7)
            {
                throw new ArgumentException(
                    "colorMap.Length must be not less than 7 (non-color + face colors).");
            }

            dataCube = GetDefaultCube(size);
            view = new CubeView<DefaultCubePart>(dataCube, colorMap, true);
            this.colorMap = colorMap;
        }

        private static void SetCubeSide(ref SmallCube cube, CubeSide side, int value)
        {
            if (cube == null)
                cube = new SmallCube();

            cube[side] = value;
        }

        public static DataCube<SmallCube> GetDefaultCube(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", "size must be > 0.");

            var bigCube = new DataCube<SmallCube>(size);
            int maxIndex = bigCube.Size - 1;

            for (int i = 0; i < bigCube.Size; i++)
            {
                for (int j = 0; j < bigCube.Size; j++)
                {
                    for (int k = 0; k < bigCube.Size; k++)
                    {
                        SmallCube cube = null;

                        if (i == 0)
                            SetCubeSide(ref cube, CubeSide.Left, 1);
                        if (i == maxIndex)
                            SetCubeSide(ref cube, CubeSide.Right, 2);

                        if (j == 0)
                            SetCubeSide(ref cube, CubeSide.Up, 3);
                        if (j == maxIndex)
                            SetCubeSide(ref cube, CubeSide.Down, 4);

                        if (k == 0)
                            SetCubeSide(ref cube, CubeSide.Front, 5);
                        if (k == maxIndex)
                            SetCubeSide(ref cube, CubeSide.Back, 6);

                        bigCube[i, j, k] = cube;
                    }
                }
            }

            return bigCube;
        }

        public DataCube<SmallCube> GetCurrentState()
        {
            return (DataCube<SmallCube>)dataCube.Clone();
        }

        public void ReplaceCube(DataCube<SmallCube> newCube)
        {
            if (newCube == null)
                throw new ArgumentNullException("newCube");
            if (newCube.Size != Size)
                throw new ArgumentException("newCube.Size must equals Size.", "newCube");

            var replacer = new DataCube<SmallCube>(newCube.Size);
            int maxIndex = newCube.Size - 1;

            foreach (var coords in CubeCoords.EnumerateCube(Size))
            {
                bool isOuterCube = 
                    coords.Left  == 0 || coords.Left  == maxIndex ||
                    coords.Top   == 0 || coords.Top   == maxIndex ||
                    coords.Depth == 0 || coords.Depth == maxIndex;

                if (isOuterCube)
                {
                    if (newCube[coords] == null)
                    {
                        throw new ArgumentException(
                            "Outer cube element at (" + coords.ToString() + ") is null.", "newCube");
                    }

                    replacer[coords] = (SmallCube)newCube[coords].Clone();
                }
            }

            if (view.AnimationInProgress)
                view.EndAnimation();

            var viewReplacer = new CubeView<DefaultCubePart>(
                replacer, colorMap, true);

            viewReplacer.World = view.World;
            viewReplacer.Up = view.Up;
            viewReplacer.Look = view.Look;
            viewReplacer.Right = viewReplacer.Right;

            dataCube = replacer;
            view = viewReplacer;
        }

        public void BeginLayerRotation(Axis around, int layer, bool clockwise, int stepCount)
        {
            if (view.AnimationInProgress)
                view.EndAnimation();

            view.BeginLayerRotation(around, layer, stepCount, clockwise);
            dataCube.Rotate(around, layer, clockwise);
        }

        public void UpdateAnimation()
        {
            view.UpdateAnimation();
        }

        public void EndAnimation()
        {
            view.EndAnimation();
        }

        public bool FindSideByPoint(Point point, Scene scene, Matrix parentWorld,
            out CubeCoords coords, out CubeSide side)
        {
            var worldView = view.World * World * parentWorld * scene.View;
            return view.FindSideByPoint(point, scene, worldView, scene.Projection,
                out coords, out side);
        }

        public Rotation CreateRotationFromSides(
            CubeCoords coords1, CubeSide side1,
            CubeCoords coords2, CubeSide side2)
        {
            int left1 = coords1.Left;
            int top1 = coords1.Top;
            int depth1 = coords1.Depth;

            int left2 = coords2.Left;
            int top2 = coords2.Top;
            int depth2 = coords2.Depth;

            if (side1 == side2 && coords1 != coords2)
            {
                bool invert;
                Axis? equalsWay = GetEqualsWay(side1,
                    left1, top1, depth1,
                    left2, top2, depth2,
                    out invert);

                if (equalsWay == null)
                    return null;

                Axis rotation = (Axis)equalsWay;
                int layer = CoordsHelper.GetLayerFromAxis(rotation, coords1);

                return new Rotation(rotation, layer, !invert);
            }
            else if (coords1 == coords2 && side1 != side2)
            {
                int sum = (int)Axis.Left + (int)Axis.Top + (int)Axis.Depth;
                Axis axis1 = CoordsHelper.GetAxisFromSide(side1);
                Axis axis2 = CoordsHelper.GetAxisFromSide(side2);

                Axis rotation = (Axis)(sum - (int)axis1 - (int)axis2);
                int layer = CoordsHelper.GetLayerFromAxis(rotation, coords1);

                bool clockwise = false;
                for (int i = 0; i < rotationRules.GetLength(0); i++)
                {
                    if (rotationRules[i, 0] == side1 &&
                        rotationRules[i, 1] == side2)
                    {
                        clockwise = true;
                        break;
                    }
                }

                return new Rotation(rotation, layer, clockwise);
            }

            return null;
        }

        private Axis? GetEqualsWay(CubeSide side,
            int left1, int top1, int depth1,
            int left2, int top2, int depth2, out bool invert)
        {
            Axis? equalsWay = null;
            invert = false;
            
            if (side == CubeSide.Left || side == CubeSide.Right)
            {
                if (top1 == top2)
                {
                    equalsWay = Axis.Top;
                    invert = depth1 < depth2;
                }
                else if (depth1 == depth2)
                {
                    equalsWay = Axis.Depth;
                    invert = top1 > top2;
                }
            }
            else if (side == CubeSide.Up || side == CubeSide.Down)
            {
                if (left1 == left2)
                {
                    equalsWay = Axis.Left;
                    invert = depth1 > depth2;
                }
                else if (depth1 == depth2)
                {
                    equalsWay = Axis.Depth;
                    invert = left1 < left2;
                }
            }
            else if (side == CubeSide.Front || side == CubeSide.Back)
            {
                if (left1 == left2)
                {
                    equalsWay = Axis.Left;
                    invert = top1 < top2;
                }
                else if (top1 == top2)
                {
                    equalsWay = Axis.Top;
                    invert = left1 > left2;
                }
            }

            invert ^= side == CubeSide.Right || side == CubeSide.Down || side == CubeSide.Back;
            return equalsWay;
        }

        public string DebugClick(Point p, Scene scene, Matrix parentWorld)
        {
            var worldView = view.World * World * parentWorld * scene.View;

            CubeCoords coords;
            CubeSide side;
            if (view.FindSideByPoint(p, scene, worldView, scene.Projection, out coords, out side))
            {
                return string.Format("{{l:{0}, t:{1}, d:{2}, side:{3}}}",
                    coords.Left, coords.Top, coords.Depth, side);
            }
            else
            {
                return "{<none>}";
            }
        }

        public override void Draw(Scene scene, Matrix parentWorld)
        {
            view.Draw(scene, World * parentWorld);
        }

        public override void Draw(Scene scene)
        {
            view.Draw(scene, World);
        }
    }
}
