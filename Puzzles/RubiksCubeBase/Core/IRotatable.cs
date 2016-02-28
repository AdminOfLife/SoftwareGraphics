using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeBase
{
    public interface IRotatable
    {
        void RotateAroundLeft();
        void RotateAroundTop();
        void RotateAroundDepth();
    }
}
