using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeBase
{
    public sealed class SmallCube : IRotatable, ICube<int>, ICloneable
    {
        int[] data = new int[6];

        public int this[CubeSide side]
        {
            get { return data[(int)side]; }
            set { data[(int)side] = value; }
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

        public object Clone()
        {
            return new SmallCube() { data = (int[])this.data.Clone() };
        }
    }
}
