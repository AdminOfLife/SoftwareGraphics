using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace RubiksCubeBase
{
    internal abstract class CubePart : DrawableObject, IRotatable, ICube<Triangle[]>
    {
        protected Triangle[][] sides;
        protected ICollection<Triangle> allPolygons;

        public virtual Triangle[] this[CubeSide side]
        {
            get { return sides[(int)side]; }
            set
            {
                sides[(int)side] = value;
                allPolygons = new ConcatCollection<Triangle>(sides);
            }
        }

        public CubePart()
        {
        }

        public void RotateAroundLeft()
        {
            this.Rotate(Axis.Left, true);
        }

        public void RotateAroundTop()
        {
            this.Rotate(Axis.Top, true);
        }

        public void RotateAroundDepth()
        {
            this.Rotate(Axis.Depth, true);
        }

        public override void Draw(Scene scene, Matrix parentWorld)
        {
            scene.Device.Draw(allPolygons, World * parentWorld, scene.View, scene.Projection);
        }

        public override void Draw(Scene scene)
        {
            scene.Device.Draw(allPolygons, World, scene.View, scene.Projection);
        }
    }
}
