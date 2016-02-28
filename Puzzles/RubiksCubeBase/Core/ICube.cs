using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeBase
{
    public interface ICube<T>
    {
        T this[CubeSide side] { get; set; }
    }
}
