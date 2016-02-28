using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace SoftwareGraphics.Objects
{
    public class BizzareSphere : Sphere
    {
        public BizzareSphere(float radius, Color color, Random random)
            : base(radius, color)
        {
            Triangle[] triangles = polygons.ToArray();

            Matrix transform = new Matrix();

            for (int i = 0; i < triangles.Length; i++)
            {
                transform = MatrixHelper.CreateScale(0.5f * (1 + (float)random.NextDouble()));
                Triangle triangle = triangles[i];
                triangles[i] = new Triangle(
                    Vector3.Transform(triangle.A, ref transform),
                    Vector3.Transform(triangle.B, ref transform),
                    Vector3.Transform(triangle.C, ref transform),
                    color);
            }

            polygons.Clear();
            polygons.AddRange(triangles);
        }
    }
}
