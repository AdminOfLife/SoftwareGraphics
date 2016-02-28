using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace RubiksCubeBase
{
    internal sealed class CubeView<TPart> : DrawableObject
        where TPart : CubePart, new()
    {
        private struct SearchCandidate
        {
            public CubeCoords Coords;
            public CubeSide Side;
            public Matrix Wvp;

            public SearchCandidate(CubeCoords coords, CubeSide side, Matrix wvp)
            {
                Coords = coords;
                Side = side;
                Wvp = wvp;
            }
        }

        DataCube<CubePart> viewCube;

        int animatedLayer;
        Axis animatedAxis;
        
        float rotationAngle;
        int stepsLeft = 0;

        public bool AnimationInProgress
        {
            get { return stepsLeft > 0; }
        }

        public CubeView(DataCube<SmallCube> model, Color[] colorMap, bool removeBlackParts)
        {
            if (colorMap == null)
                throw new ArgumentNullException("colorMap");

            viewCube = new DataCube<CubePart>(model.Size);
            var partTransform = MatrixHelper.CreateScale(0.9f * 2f / model.Size);
            EmptyCubePart emptyPart = new EmptyCubePart();

            int maxIndex = model.Size - 1;
            for (int i = 0; i < model.Size; i++)
            {
                for (int j = 0; j < model.Size; j++)
                {
                    for (int k = 0; k < model.Size; k++)
                    {
                        bool isInnerCube =
                            i != 0 && j != 0 && k != 0 &&
                            i != maxIndex && j != maxIndex && k != maxIndex;

                        if (model[i, j, k] == null || removeBlackParts && isInnerCube)
                        {
                            viewCube[i, j, k] = emptyPart;
                        }
                        else
                        {
                            CubePart part = new TPart();
                            PaintCubePart(part, model[i, j, k], colorMap);

                            part.World *= partTransform;
                            part.Position = CoordsHelper.GetPositionFromCoords(
                                new CubeCoords(i, j, k), model.Size);

                            viewCube[i, j, k] = part;
                        }
                    }
                }
            }
        }

        private void PaintCubePart(CubePart part, SmallCube model, Color[] colorMap)
        {
            for (var side = CubeSide.Left; side <= CubeSide.Back; side++)
            {
                Triangle[] sidePolygons = part[side];
                Color sideColor = colorMap[model[side]];

                for (int j = 0; j < sidePolygons.Length; j++)
                {
                    sidePolygons[j].Color = sideColor;
                }
            }
        }

        public void BeginLayerRotation(Axis rotatedAxis, int rotatedLayer, int stepCount, bool clockwise)
        {
            if (rotatedLayer < 0 || rotatedLayer >= viewCube.Size)
                throw new ArgumentOutOfRangeException("rotatedLayer");
            if (stepCount <= 0)
                throw new ArgumentOutOfRangeException("stepCount", "stepCount must be >= 0.");
            if (AnimationInProgress)
                throw new InvalidOperationException("Animation already in progress.");

            animatedAxis = rotatedAxis;
            animatedLayer = rotatedLayer;
            stepsLeft = stepCount;

            rotationAngle = (float)Math.PI / (2 * stepCount) * (clockwise ? +1 : -1);
        }

        private void RotateLayer(float angle)
        {
            for (int i = 0; i < viewCube.Size; i++)
            {
                for (int j = 0; j < viewCube.Size; j++)
                {
                    switch (animatedAxis)
                    {
                        case Axis.Left:
                            viewCube[animatedLayer, i, j].RotateAround(Vector3.Zero, angle, 0, 0);
                            break;
                        case Axis.Top:
                            viewCube[i, animatedLayer, j].RotateAround(Vector3.Zero, 0, -angle, 0);
                            break;
                        case Axis.Depth:
                            viewCube[i, j, animatedLayer].RotateAround(Vector3.Zero, 0, 0, -angle);
                            break;
                    }
                }
            }
        }

        public void UpdateAnimation()
        {
            if (!AnimationInProgress)
                return;

            RotateLayer(rotationAngle);

            stepsLeft--;
            if (stepsLeft == 0)
            {
                EndRotation();
            }
        }

        public void EndAnimation()
        {
            if (!AnimationInProgress)
                throw new InvalidOperationException("No animation in progress.");

            float rotationLeft = rotationAngle * stepsLeft;
            RotateLayer(rotationLeft);

            stepsLeft = 0;
            EndRotation();
        }

        private void EndRotation()
        {
            viewCube.Rotate(animatedAxis, animatedLayer, rotationAngle > 0);
        }

        public bool FindSideByPoint(Point point, Scene scene, Matrix worldView, Matrix projection,
            out CubeCoords coords, out CubeSide side)
        {
            coords = new CubeCoords();
            side = CubeSide.Left;

            Matrix wvp = worldView * projection;
            Vector3 p = new Vector3(
                (float)point.X * 2f / scene.Viewport.Width - 1f,
                -(float)point.Y * 2f / scene.Viewport.Height + 1f,
                0);

            bool[] visibility = new bool[]
            {
                Vector3.TransformNormal(Vector3.Left,     ref worldView).Z > 0,
                Vector3.TransformNormal(Vector3.Right,    ref worldView).Z > 0,
                Vector3.TransformNormal(Vector3.Up,       ref worldView).Z > 0,
                Vector3.TransformNormal(Vector3.Down,     ref worldView).Z > 0,
                Vector3.TransformNormal(Vector3.Backward, ref worldView).Z > 0,
                Vector3.TransformNormal(Vector3.Forward,  ref worldView).Z > 0,
            };

            var candidates = new List<SearchCandidate>();
            CubeCoords[] indices = new CubeCoords[6];

            int maxIndex = viewCube.Size - 1;
            for (int i = 0; i < viewCube.Size; i++)
            {
                for (int j = 0; j < viewCube.Size; j++)
                {
                    indices[(int)CubeSide.Left]  = new CubeCoords(0, i, j);
                    indices[(int)CubeSide.Right] = new CubeCoords(maxIndex, i, j);
                    indices[(int)CubeSide.Up]    = new CubeCoords(i, 0, j);
                    indices[(int)CubeSide.Down]  = new CubeCoords(i, maxIndex, j);
                    indices[(int)CubeSide.Front] = new CubeCoords(i, j, 0);
                    indices[(int)CubeSide.Back]  = new CubeCoords(i, j, maxIndex);

                    for (int k = 0; k < indices.Length; k++)
                    {
                        coords = indices[k];
                        side = (CubeSide)k;
                        Matrix localWvp = viewCube[coords].World * wvp;

                        if (visibility[(int)side] &&
                            IsSideContainsPoint(viewCube[coords], side, p, scene, localWvp))
                        {
                            candidates.Add(new SearchCandidate(coords, side, localWvp));
                        }
                    }
                }
            }
            
            var cubes = candidates.OrderBy(c => Vector3.Transform(Vector3.Zero, ref c.Wvp).Z);
            if (cubes.Any())
            {
                coords = cubes.First().Coords;
                side = cubes.First().Side;
                return true;
            }

            return false;
        }

        private bool IsSideContainsPoint(CubePart cubePart, CubeSide side, Vector3 point,
            Scene scene, Matrix worldViewProjection)
        {
            foreach (Triangle polygon in cubePart[side])
            {
                if (IsTriangleContainsPoint(polygon, point, scene, worldViewProjection))
                    return true;
            }

            return false;
        }

        private bool IsTriangleContainsPoint(Triangle triangle, Vector3 point,
            Scene scene, Matrix worldViewProjection)
        {
            if (!scene.Device.TryProjectTriangle(ref triangle, ref worldViewProjection))
                return false;

            Vector3 a = triangle.A;
            Vector3 b = triangle.B;
            Vector3 c = triangle.C;
            Vector3 p = point;
            
            a.Z = 0;
            b.Z = 0;
            c.Z = 0;

            if (Vector3.Cross(b - a, c - a).Z * Vector3.Cross(b - a, p - a).Z >= 0 &&
                Vector3.Cross(c - b, a - b).Z * Vector3.Cross(c - b, p - b).Z >= 0 &&
                Vector3.Cross(a - c, b - c).Z * Vector3.Cross(a - c, p - c).Z >= 0)
            {
                return true;
            }

            return false;
        }

        private void DrawParts(Scene scene, Matrix currentWorld)
        {
            int maxIndex = viewCube.Size - 1;

            // draw Up and Down sides
            for (int i = 0; i < viewCube.Size; i++)
            {
                for (int j = 0; j < viewCube.Size; j++)
                {
                    viewCube[i, 0, j].Draw(scene, currentWorld);
                    viewCube[i, maxIndex, j].Draw(scene, currentWorld);
                }
            }

            // draw Left and Right sides without up and down rows
            for (int i = 1; i < maxIndex; i++)
            {
                for (int j = 0; j < viewCube.Size; j++)
                {
                    viewCube[0, i, j].Draw(scene, currentWorld);
                    viewCube[maxIndex, i, j].Draw(scene, currentWorld);
                }
            }

            // draw Front and Back sides without side rows and columns
            for (int i = 1; i < maxIndex; i++)
            {
                for (int j = 1; j < maxIndex; j++)
                {
                    viewCube[i, j, 0].Draw(scene, currentWorld);
                    viewCube[i, j, maxIndex].Draw(scene, currentWorld);
                }
            }
        }

        public override void Draw(Scene scene, Matrix parentWorld)
        {
            DrawParts(scene, World * parentWorld);
        }

        public override void Draw(Scene scene)
        {
            DrawParts(scene, World);
        }
    }
}
