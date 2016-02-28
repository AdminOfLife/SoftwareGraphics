using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoftwareGraphics.Devices;
using SoftwareGraphics.Objects;

namespace SoftwareGraphics
{
    public sealed class Scene
    {
        public GraphicsDevice Device;

        public Camera Camera;
        public Matrix Projection;

        public Matrix View
        {
            get { return Camera.View; }
        }

        public Viewport Viewport
        {
            get { return Device.Viewport; }
        }

        public Scene(GraphicsDevice graphicsDevice, Camera camera, Matrix projection)
        {
            Device = graphicsDevice;
            Camera = camera;
            Projection = projection;
        }
    }
}
