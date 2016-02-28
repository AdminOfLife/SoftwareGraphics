using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SoftwareGraphics.Objects
{
    public class Sphere : ModelObject
    {
        const int LayerCount = 8;
        const int LayerPoints = 16;

        public Sphere(float radius, Color color)
            : this(radius, ColorHelper.GetStatic(color).GetEnumerator())
        {
        }

        public Sphere(float radius, IEnumerator<Color> colorer)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException("radius");

            var triangles = ModelBuilder.CreateSphere(
                radius, LayerCount, LayerPoints, colorer);

            polygons.AddRange(triangles);
        }
    }
}
