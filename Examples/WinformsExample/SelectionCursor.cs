using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace WinformsExample
{
    public class SelectionCursor : ModelObject
    {
        const float CircleRadius = 30f;
        const int CirclePoints = 16;

        public bool Visible { get; set; }

        public SelectionCursor(Color color)
        {
            Visible = false;

            Triangle[] normalFace = ModelBuilder.CreateCircle(
                Vector3.Zero,
                new Vector3(0, CircleRadius, 0),
                Vector3.Forward,
                CirclePoints,
                color);

            Triangle[] backFace = (Triangle[])normalFace.Clone();
            Matrix rotation = MatrixHelper.CreateRotationY((float)Math.PI);
            ModelBuilder.TransformPolygons(backFace, ref rotation);

            polygons.AddRange(normalFace);
            polygons.AddRange(backFace);
        }

        public override void Draw(Scene scene)
        {
            if (Visible)
                base.Draw(scene);
        }
    }
}
