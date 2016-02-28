using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace RubiksCubeBase
{
    internal sealed class CircleCubePart : CubePart
    {
        const int CirclePointCount = 8;

        public CircleCubePart()
        {
            float side = 1f;
            float halfSide = side / 2;
            
            sides = new[]
            {
                ModelBuilder.CreateCircle(  // left
                    new Vector3(-halfSide, 0, 0),
                    new Vector3(0, halfSide, 0),
                    new Vector3(-1, 0, 0),
                    CirclePointCount, Color.White),
                ModelBuilder.CreateCircle(  // right
                    new Vector3(halfSide, 0, 0),
                    new Vector3(0, halfSide, 0),
                    new Vector3(1, 0, 0),
                    CirclePointCount, Color.White),
                ModelBuilder.CreateCircle(  // top
                    new Vector3(0, halfSide, 0),
                    new Vector3(halfSide, 0, 0),
                    new Vector3(0, 1, 0),
                    CirclePointCount, Color.White),
                ModelBuilder.CreateCircle(  // bottom
                    new Vector3(0, -halfSide, 0),
                    new Vector3(halfSide, 0, 0),
                    new Vector3(0, -1, 0),
                    CirclePointCount, Color.White),
                ModelBuilder.CreateCircle(  // front
                    new Vector3(0, 0, halfSide),
                    new Vector3(halfSide, 0, 0),
                    new Vector3(0, 0, 1),
                    CirclePointCount, Color.White),
                ModelBuilder.CreateCircle(  // back
                    new Vector3(0, 0, -halfSide),
                    new Vector3(halfSide, 0, 0),
                    new Vector3(0, 0, -1),
                    CirclePointCount, Color.White),
            };

            allPolygons = new ConcatCollection<Triangle>(sides);
        }
    }
}
