using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoftwareGraphics.Devices;

namespace SoftwareGraphics.Controls
{
    public sealed class CanvasDrawEventArgs : EventArgs
    {
        public GraphicsDevice Device { get; private set; }
        public Scene Scene { get; private set; }

        public CanvasDrawEventArgs(GraphicsDevice device, Scene scene)
        {
            Device = device;
            Scene = scene;
        }
    }
}
