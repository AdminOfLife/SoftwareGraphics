using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeBase
{
    public sealed class Rotation
    {
        public Axis Axis { get; private set; }
        public int Layer { get; private set; }
        public bool Clockwise { get; private set; }

        public Rotation(Axis axis, int layer, bool clockwise)
        {
            if (layer < 0)
                throw new ArgumentOutOfRangeException("layer");

            Axis = axis;
            Layer = layer;
            Clockwise = clockwise;
        }

        public bool TryStart(RubiksCube cube, int animationSteps)
        {
            if (Layer >= cube.Size)
                throw new ArgumentException("Layer must be < cube.Size.", "cube");
            if (cube.AnimationInProgress)
                return false;

            cube.BeginLayerRotation(Axis, Layer, Clockwise, animationSteps);
            return true;
        }
    }
}
