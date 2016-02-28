using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Objects
{
    public class ModelObject : DrawableObject
    {
        protected List<Triangle> polygons = new List<Triangle>();

        public List<Triangle> Polygons
        {
            get { return polygons; }
        }

        public ModelObject()
        {
        }

        public override void Draw(Scene scene)
        {
            scene.Device.Draw(polygons, World, scene.View, scene.Projection);
        }

        public override void Draw(Scene scene, Matrix parentWorld)
        {
            scene.Device.Draw(polygons, World * parentWorld, scene.View, scene.Projection);
        }
    }
}
