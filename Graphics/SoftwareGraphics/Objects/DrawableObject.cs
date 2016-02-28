using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Objects
{
    public abstract class DrawableObject : BaseObject
    {
        public DrawableObject()
        {
        }

        public abstract void Draw(Scene scene);

        public abstract void Draw(Scene scene, Matrix parentWorld);
    }
}
