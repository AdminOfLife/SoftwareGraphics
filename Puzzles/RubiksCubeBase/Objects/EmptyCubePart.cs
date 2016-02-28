using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace RubiksCubeBase
{
    internal sealed class EmptyCubePart : CubePart
    {
        static readonly Triangle[] emptyPolygons = new Triangle[] { };

        public override Triangle[] this[CubeSide side]
        {
            get { return emptyPolygons; }
            set { }
        }

        public EmptyCubePart()
        {
        }

        public override void Draw(Scene scene)
        {
        }

        public override void Draw(Scene scene, Matrix parentWorld)
        {
        }
    }
}
