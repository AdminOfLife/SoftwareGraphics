using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace SoftwareGraphics.Objects
{
    public class RandomSystem : ModelObject
    {
        Random random;
        IEnumerator<Color> colorer;

        public Vector3 Size { get; private set; }

        public float PartSize { get; private set; }

        public RandomSystem(Vector3 size, float partSize, Random random)
        {
            Size = size;
            PartSize = partSize;
            this.random = random;
            colorer = ColorHelper.GetRandom(random).GetEnumerator();
        }

        public void Clear()
        {
            polygons.Clear();
        }

        public void GeneratePart()
        {
            colorer.MoveNext();

            Vector3 randomPoint = new Vector3(
                (float)(random.NextDouble() - 0.5) * Size.X,
                (float)(random.NextDouble() - 0.5) * Size.Y,
                (float)(random.NextDouble() - 0.5) * Size.Z);

            Vector3 start = randomPoint - new Vector3(
                -PartSize / 2, PartSize / 2, PartSize / 2);

            Matrix transform =
                MatrixHelper.CreateRotationX((float)(Math.PI * random.NextDouble())) *
                MatrixHelper.CreateRotationY((float)(Math.PI * random.NextDouble())) *
                MatrixHelper.CreateRotationZ((float)(Math.PI * random.NextDouble())) *
                MatrixHelper.CreateScale(
                    0.5f + (float)random.NextDouble(),
                    0.5f + (float)random.NextDouble(),
                    0.5f + (float)random.NextDouble());

            Triangle[] marker;
            int number = random.Next(100);

            if (number < 50)
            {
                marker = ModelBuilder.CreateParallelepiped(
                    start,
                    Vector3.Right * PartSize,
                    Vector3.Down * PartSize,
                    Vector3.Forward * PartSize,
                    colorer.Current);
            }
            else if (number < 96)
            {
                marker = ModelBuilder.CreatePyramid(
                    start,
                    PartSize,
                    Vector3.Right * PartSize,
                    Vector3.Forward * PartSize,
                    colorer.Current);
            }
            else
            {
                float a = PartSize / 5f;
                var axisLines = new AxisLines(2 * a, 10 * a, 1.5f * a);
                axisLines.MoveBy(randomPoint);
                transform *= axisLines.World;
                
                marker = axisLines.Polygons.ToArray();
            }

            ModelBuilder.TransformPolygons(marker, ref transform);
            polygons.AddRange(marker);
        }
    }
}
