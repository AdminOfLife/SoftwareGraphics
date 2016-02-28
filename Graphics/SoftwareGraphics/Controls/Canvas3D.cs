using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SoftwareGraphics.Devices;
using System.Reflection;

namespace SoftwareGraphics.Controls
{
    [DefaultEvent("Draw")]
    [DefaultProperty("GraphicsDeviceType")]
    public partial class Canvas3D : Control
    {
        GraphicsDevice device;
        Scene scene;

        CanvasDrawEventArgs drawEventArgs;
        System.Drawing.Graphics graphics;

        bool updating = false;
        DeviceType? newDeviceType;

        public DeviceType GraphicsDeviceType
        {
            get
            {
                if (device is GdiGraphicsDevice)
                    return DeviceType.Gdi;
                else if (device.GetType().Name == "Direct3DGraphicsDevice")
                    return DeviceType.Direct3D;
                else
                    return DeviceType.None;
            }
            set
            {
                newDeviceType = value;
                ApplyDeviceParams();
            }
        }

        [Browsable(false)]
        public GraphicsDevice GraphicsDevice
        {
            get { return device; }
        }

        [Browsable(false)]
        public Viewport Viewport
        {
            get { return device != null ? device.Viewport : null; }
        }

        [Browsable(false)]
        public Camera Camera
        {
            get { return scene.Camera; }
            set
            {
                Scene original = scene;
                RecreateScene(original.Device, value, original.Projection);
            }
        }

        [Browsable(false)]
        public Matrix Projection
        {
            get { return scene.Projection; }
            set
            {
                Scene original = scene;
                RecreateScene(original.Device, original.Camera, value);
            }
        }

        [Browsable(false)]
        public Scene Scene
        {
            get { return scene; }
        }

        protected override Size DefaultSize
        {
            get { return new Size(200, 200); }
        }

        [DefaultValue(typeof(Color), "CornflowerBlue")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public event EventHandler<CanvasDrawEventArgs> Draw;

        public Canvas3D()
        {
            InitializeComponent();

            MinimumSize = new Size(1, 1);
            TabStop = false;
            BackColor = Color.CornflowerBlue;
            SetStyle(ControlStyles.Opaque, true);
            
            this.Resize += Canvas3D_Resize;
            this.Disposed += Canvas3D_Disposed;

            newDeviceType = DeviceType.Gdi;

            RecreateScene(null, null, new Matrix());
            updating = true;

            graphics = this.CreateGraphics();
            updating = false;
            ApplyDeviceParams();
        }

        protected Viewport GetCurrentViewport()
        {
            return new Viewport(0, 0, ClientSize.Width, ClientSize.Height);
        }

        public void BeginUpdate()
        {
            if (updating)
                throw new InvalidOperationException("EndUpdate must be called first.");

            updating = true;
        }

        public void EndUpdate()
        {
            if (!updating)
                throw new InvalidOperationException("BeginUpdate must be called first.");

            updating = false;
            ApplyDeviceParams();
        }

        protected virtual void ApplyDeviceParams()
        {
            if (updating)
                return;

            DeviceParams deviceParams = null;
            if (device != null)
            {
                deviceParams = device.Parameters;
                device.Dispose();
            }
            else
            {
                deviceParams = new DeviceParams();
            }

            if (newDeviceType == null)
                newDeviceType = GraphicsDeviceType;

            switch (newDeviceType)
            {
                case DeviceType.Gdi:
                    device = new GdiGraphicsDevice(graphics, GetCurrentViewport());
                    break;

                case DeviceType.Direct3D:
                    Type xnaDeviceType = GetXnaGraphicsDeviceType();
                    if (xnaDeviceType == null)
                        goto case DeviceType.Gdi;
                    device = (GraphicsDevice)Activator.CreateInstance(xnaDeviceType, this.Handle, GetCurrentViewport());
                    break;

                default:
                    device = null;
                    break;
            }

            if (device != null)
            {
                deviceParams.Viewport = null;
                device.Parameters = deviceParams;
            }

            Scene original = scene;
            RecreateScene(device, original.Camera, original.Projection);

            newDeviceType = null;

            Invalidate();
        }

        private Type GetXnaGraphicsDeviceType()
        {
            Type deviceType = (
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                let type = TryGetDirect3DGraphicsDeviceType(assembly)
                where type != null
                select type).FirstOrDefault();

            if (deviceType == null)
            {
                string xnaAssemblyName = string.Format("{0}.Xna", nameof(SoftwareGraphics));
                try
                {
                    Assembly loadedXnaAssembly = AppDomain.CurrentDomain.Load(xnaAssemblyName);
                    deviceType = TryGetDirect3DGraphicsDeviceType(loadedXnaAssembly);
                }
                catch (Exception) { /* ignore */ }
            }

            return deviceType;
        }

        private Type TryGetDirect3DGraphicsDeviceType(Assembly assembly)
        {
            return assembly.GetType(string.Format("{0}.{1}.{2}",
                    nameof(SoftwareGraphics),
                    nameof(SoftwareGraphics.Devices),
                    "Direct3DGraphicsDevice"));
        }

        private void RecreateScene(GraphicsDevice device, Camera camera, Matrix projection)
        {
            scene = new Scene(device, camera, projection);
            drawEventArgs = new CanvasDrawEventArgs(device, scene);
        }

        private void Canvas3D_Resize(object sender, EventArgs e)
        {
            if (ClientSize.Width == 0 ||
                ClientSize.Height == 0)
            {
                return;
            }

            graphics = this.CreateGraphics();
            ApplyDeviceParams();
        }

        protected virtual void OnDraw(CanvasDrawEventArgs e)
        {
            var temp = System.Threading.Interlocked.CompareExchange(
                ref Draw, null, null);

            if (temp != null)
            {
                temp(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                e.Graphics.Clear(BackColor);
            }
            
            OnDraw(drawEventArgs);
        }

        private void Canvas3D_Disposed(object sender, EventArgs e)
        {
            if (device != null)
                device.Dispose();
            if (graphics != null)
                graphics.Dispose();
        }
    }
}
