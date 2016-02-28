using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SoftwareGraphics.Objects
{
    public class Tetrahedron : ModelObject
    {
        public Tetrahedron(
            Vector3 p, Vector3 a, Vector3 b, Vector3 c,
            Color abc, Color pab, Color pbc, Color pac)
        {
            polygons.Add(new Triangle(a, c, b, abc));
            polygons.Add(new Triangle(p, a, b, pab));
            polygons.Add(new Triangle(p, b, c, pbc));
            polygons.Add(new Triangle(p, c, a, pac));
        }
    }
}
