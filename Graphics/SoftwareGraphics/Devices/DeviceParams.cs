using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Devices
{
    public sealed class DeviceParams : ICloneable
    {
        public Viewport Viewport { get; set; }

        public bool IsWireframe { get; set; }

        public bool ShowEdges { get; set; }

        public CullMode CullMode { get; set; }

        public bool LightingEnabled { get; set; }

        public IEnumerable<Light> Lights { get; set; }

        public object Clone()
        {
            return new DeviceParams()
            {
                Viewport = this.Viewport,
                IsWireframe = this.IsWireframe,
                ShowEdges = this.ShowEdges,
                CullMode = this.CullMode,
                LightingEnabled = this.LightingEnabled,
                Lights = this.Lights.ToArray(),
            };
        }
    }
}
