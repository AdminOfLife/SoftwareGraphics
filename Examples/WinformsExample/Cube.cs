using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace WinformsExample
{
    public class Cube : ModelObject
    {
        public Cube(float size)
        {
            float half = size * 0.5f;

            Vector3[] high = new Vector3[4];
            high[0] = new Vector3(-half, half, half);
            high[1] = new Vector3(-half, half, -half);
            high[2] = new Vector3(half, half, -half);
            high[3] = new Vector3(half, half, half);

            Vector3[] low = high.Select(v => new Vector3(v.X, -v.Y, v.Z)).ToArray();

            // top
            polygons.Add(new Triangle(high[0], high[1], high[2], Color.Red));
            polygons.Add(new Triangle(high[2], high[3], high[0], Color.Red));

            // bottom
            polygons.Add(new Triangle(low[1], low[0], low[3], Color.Green));
            polygons.Add(new Triangle(low[3], low[2], low[1], Color.Green));

            // front
            polygons.Add(new Triangle(low[0], high[0], high[3], Color.CornflowerBlue));
            polygons.Add(new Triangle(high[3], low[3], low[0], Color.CornflowerBlue));

            // back
            polygons.Add(new Triangle(low[2], high[2], high[1], Color.Yellow));
            polygons.Add(new Triangle(high[1], low[1], low[2], Color.Yellow));

            // left
            polygons.Add(new Triangle(low[1], high[1], high[0], Color.Brown));
            polygons.Add(new Triangle(high[0], low[0], low[1], Color.Brown));

            // right
            polygons.Add(new Triangle(low[3], high[3], high[2], Color.Violet));
            polygons.Add(new Triangle(high[2], low[2], low[3], Color.Violet));
        }
    }
}
