using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace RubiksCubeBase
{
    internal sealed class DefaultCubePart : CubePart
    {
        public DefaultCubePart()
        {
            float side = 1f;
            float halfSide = side / 2;
            
            sides = new[]
            {
                ModelBuilder.CreateParallelogram(  // left
                    new Vector3(-halfSide, -halfSide, halfSide),
                    new Vector3(0, 0, -side),
                    new Vector3(0, side, 0), 
                    Color.White),
                ModelBuilder.CreateParallelogram(  // right
                    new Vector3(halfSide, -halfSide, halfSide),
                    new Vector3(0, side, 0), 
                    new Vector3(0, 0, -side),
                    Color.White),
                ModelBuilder.CreateParallelogram(  // top
                    new Vector3(-halfSide, halfSide, halfSide),
                    new Vector3(0, 0, -side),
                    new Vector3(side, 0, 0),
                    Color.White),
                ModelBuilder.CreateParallelogram(  // bottom
                    new Vector3(-halfSide, -halfSide, halfSide),
                    new Vector3(side, 0, 0), 
                    new Vector3(0, 0, -side),
                    Color.White),
                ModelBuilder.CreateParallelogram(  // front
                    new Vector3(-halfSide, -halfSide, halfSide),
                    new Vector3(0, side, 0), 
                    new Vector3(side, 0, 0),
                    Color.White),
                ModelBuilder.CreateParallelogram(  // back
                    new Vector3(-halfSide, -halfSide, -halfSide),
                    new Vector3(side, 0, 0),
                    new Vector3(0, side, 0),
                    Color.White),
            };

            allPolygons = new ConcatCollection<Triangle>(sides);
        }
    }
}
