using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace SoftwareGraphics.Objects
{
    public class PythagorasTree : ModelObject
    {
        const float ColorHueStep = 4f;
        static readonly Color StartColor = Color.Yellow;

        Vector3 normal;
        Matrix rotation;
        float sideRelation;
        float depthStep;

        List<Triangle> currentLayer = new List<Triangle>();
        IEnumerator<Color> rainbowColorer;

        public int LayerCount { get; private set; }

        public PythagorasTree(Vector3 baseSide, Vector3 height, float depthStep)
        {
            this.depthStep = depthStep;
            rainbowColorer = ColorHelper.GetRainbow(
                StartColor, ColorHueStep).GetEnumerator();

            Triangle start = new Triangle(
                - baseSide / 2, baseSide / 2, height, StartColor);

            normal = Vector3.Normalize(Vector3.Cross(baseSide, height));
            
            Vector3 side = start.C - start.A;
            sideRelation = side.Length / baseSide.Length;
            float rotationAngle = -(float)Math.Acos(Math.Abs(
                Vector3.Dot(baseSide, side) / (baseSide.Length * side.Length)));

            rotation = MatrixHelper.CreateFromAxisAngle(
                normal, rotationAngle);

            currentLayer.Add(start);
            polygons.AddRange(ModelBuilder.CreateParallelogram(
                start.A, baseSide, Vector3.Cross(baseSide, normal), StartColor));

            LayerCount = 1;
        }

        public void GenerateNextLayer()
        {
            rainbowColorer.MoveNext();
            Color color = rainbowColorer.Current;

            List<Triangle> newLayer = new List<Triangle>(currentLayer.Count);
            Vector3 depthShift = normal * LayerCount * depthStep;

            foreach (Triangle triangle in currentLayer)
            {
                Vector3 base1 = triangle.C - triangle.A;
                Vector3 base2 = triangle.B - triangle.C;

                Vector3 shift1 = Vector3.Cross(normal, base1);
                Vector3 shift2 = Vector3.Cross(normal, base2);

                Vector3 side1 = Vector3.Transform(base1, ref rotation) * sideRelation;
                Vector3 side2 = Vector3.Transform(base2, ref rotation) * sideRelation;

                Triangle triangle1 = new Triangle(
                    triangle.A + shift1,
                    triangle.C + shift1,
                    triangle.A + shift1 + side1,
                    color);
                Triangle triangle2 = new Triangle(
                    triangle.C + shift2,
                    triangle.B + shift2,
                    triangle.C + shift2 + side2,
                    color);

                newLayer.Add(triangle1);
                newLayer.Add(triangle2);

                polygons.AddRange(ModelBuilder.CreateParallelogram(
                    triangle1.A + depthShift, base1, -shift1, color));
                polygons.AddRange(ModelBuilder.CreateParallelogram(
                    triangle2.A + depthShift, base2, -shift2, color));
            }

            currentLayer = newLayer;
            LayerCount++;
        }
    }
}
