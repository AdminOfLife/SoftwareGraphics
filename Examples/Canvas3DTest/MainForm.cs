using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using SoftwareGraphics;
using SoftwareGraphics.Controls;
using SoftwareGraphics.Objects;

namespace Canvas3DTest
{
    public partial class MainForm : Form
    {
        string WindowTitle;

        ModelObject foo;
        Stopwatch stopwatch = new Stopwatch();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            canvas.Camera = new Camera(new Vector3(0, 0, 500f));
            canvas.Projection = MatrixHelper.CreatePerspectiveFieldOfView(
                (float)Math.PI / 4,
                canvas.Viewport.AspectRatio,
                1f,
                1000f);

            Light light1 = new PointLight(new Vector3(0, 20, 0), 2f, Color.Red);
            Light light2 = new PointLight(new Vector3(-40, 20, 50), 2f, Color.Green);
            Light light3 = new PointLight(new Vector3(40, 20, 50), 2f, Color.Blue);
            canvas.GraphicsDevice.Lights.Add(light1);
            canvas.GraphicsDevice.Lights.Add(light2);
            canvas.GraphicsDevice.Lights.Add(light3);

            canvas.GraphicsDevice.Lights.Add(
                new GlobalDirectedLight(Vector3.Down, 0.2f, Color.Gold));

            canvas.GraphicsDevice.Lights.Add(
                new ConeLight(new Vector3(-20, 20, -20), new Vector3(-1), (float)Math.PI / 4, 1f, Color.White));

            canvas.GraphicsDevice.LightingEnabled = true;

            var colorer = ColorHelper.GetFixedColors(
                Color.White, Color.Gray).GetEnumerator();

            foo = new ModelObject();
            var p = ModelBuilder.CreatePlane(49, 49, colorer);
            Matrix scale = MatrixHelper.CreateScale(175);
            ModelBuilder.TransformPolygons(p, ref scale);

            foo.Polygons.AddRange(p);
            foo.Rotate((float)Math.PI / 2.4f, 0, 0);

            canvas.Select();
            Application.Idle += new EventHandler(Application_Idle);

            WindowTitle = this.Text;
            UpdateInfo();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            foo.RelativeRotate(0, 0, (float)Math.PI / 256);
            canvas.Invalidate();
        }

        private void canvas_Draw(object sender, CanvasDrawEventArgs e)
        {
            e.Device.Clear(Color.CornflowerBlue);
            e.Device.Begin();

            foo.Draw(canvas.Scene);

            e.Device.End();
        }

        private void UpdateInfo()
        {
            labelMode.Text = canvas.GraphicsDeviceType.ToString();
            labelLight.Text = canvas.GraphicsDevice.LightingEnabled.ToString();

            this.Text = string.Format("{0} - Time: {1:F2}",
                WindowTitle, stopwatch.ElapsedMilliseconds / 1000.0);
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.M:
                    switch (canvas.GraphicsDeviceType)
                    {
                        case DeviceType.Gdi:
                            canvas.GraphicsDeviceType = DeviceType.Direct3D;
                            break;
                        case DeviceType.Direct3D:
                            canvas.GraphicsDeviceType = DeviceType.Gdi;
                            break;
                    }
                    UpdateInfo();
                    break;
                case Keys.L:
                    canvas.GraphicsDevice.LightingEnabled = !canvas.GraphicsDevice.LightingEnabled;
                    UpdateInfo();
                    break;
                case Keys.W:
                    canvas.GraphicsDevice.IsWireframe = !canvas.GraphicsDevice.IsWireframe;
                    break;
                case Keys.T:
                    UpdateInfo();
                    stopwatch.Restart();
                    break;
            }
        }
    }
}
