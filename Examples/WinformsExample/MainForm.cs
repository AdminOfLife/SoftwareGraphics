using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SoftwareGraphics;
using SoftwareGraphics.Devices;
using SoftwareGraphics.Objects;

namespace WinformsExample
{
    public partial class MainForm : Form
    {
        enum CameraCaptureState
        {
            None,
            Rotate,
            Move,
        }

        const float MoveByMouseSpeed = 750f;

        Graphics graphics;

        GraphicsDevice device;
        Camera camera;
        Viewport viewport;
        Scene scene;

        DrawableObject foo;
        DrawableObject bar;

        Timer timer1;

        CameraCaptureState cameraCapture;
        Point mousePosition;
        SelectionCursor selectionCursor;
        SelectionCursor highlightingCursor;

        HelpForm helpForm;
        bool isCursorEnabled = false;
        
        public MainForm()
        {
            InitializeComponent();

            helpForm = new HelpForm();
            this.helpForm.FormClosing += new FormClosingEventHandler(helpForm_FormClosing);
            this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);
            this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            camera = new Camera(new Vector3(0, 0, 1200.0f));

            viewport = new Viewport(0, 0, scenePanel.ClientSize.Width, scenePanel.ClientSize.Height);

            Matrix projection = MatrixHelper.CreatePerspectiveFieldOfView(
                (float)(Math.PI / 4), viewport.AspectRatio, 1f, 2000f);
            //Matrix projection = MatrixHelper.CreatePerspective(
            //    viewport.Width, viewport.Height, 1, 100000);
            //Matrix projection = MatrixHelper.CreateOrthographic(
            //    (float)viewport.Width, (float)viewport.Height, (float)0, (float)10000);

            graphics = scenePanel.CreateGraphics();
            device = new GdiGraphicsDevice(graphics, viewport);
            //device = new Direct3DGraphicsDevice(scenePanel.Handle, viewport);

            device.LightingEnabled = true;
            //device.Lights.Add(
            //    new GlobalDirectedLight(new Vector3(-1), 1f, Color.White));
            device.Lights.Add(
                new PointLight(new Vector3(0, 150, 0), 2f, Color.White));

            scene = new Scene(device, camera, projection);

            float k = 50;

            var p = new Vector3(0, k, 0);
            var a = new Vector3(-k, -k, k);
            var b = new Vector3(0, -k, -k);
            var c = new Vector3(k, -k, k);

            Tetrahedron tetra1 = new Tetrahedron(p, a, b, c,
                Color.Yellow, Color.Green, Color.Blue, Color.Red);

            Tetrahedron tetra2 = new Tetrahedron(p, a, b, c,
                Color.Turquoise, Color.Tomato, Color.SlateGray, Color.Orange);

            //foo = new Cube(scene, 150);
            //foo = new ManyCubes(scene);
            //foo = new AxisLines(scene, 20f, 100f, 30f);
            //foo = new RandomSystem(scene, new Vector3(400, 400, 400));
            //foo = new Sphere(scene, 100f, Color.CornflowerBlue);

            Bitmap colorMap = (Bitmap)Bitmap.FromFile("ColorMap1.bmp");
            Bitmap heightMap = (Bitmap)Bitmap.FromFile("HeightMap1.bmp");
            foo = new Earth(heightMap, colorMap, 20, 3);

            //foo = new PythagorasTree(new Vector3(60, 0, 0), new Vector3(10, 30, 0), 1f);
            //foo = new PrismTree(new Vector3(60, 0, 0), new Vector3(0, 0, -20), new Vector3(10, 30, 0));

            //bar = tetra2;
            bar = new BizzareSphere(100f, Color.Orange, new Random());
            bar.Position = new Vector3(-200, 120, 50);

            selectionCursor = new SelectionCursor(Color.FromArgb(128, Color.Red));
            highlightingCursor = new SelectionCursor(Color.FromArgb(128, Color.Blue));
            highlightingCursor.Visible = isCursorEnabled;

            timer1 = new Timer();
            timer1.Interval = 50;
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bar.Rotate(0.1f, 0.1f, 0);
            UpdateScreen();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            timer1.Start();
            UpdateScreen();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            const float speed = 10f;

            DrawableObject currentObject = e.Control ? bar : foo;

            if (!e.Shift) // Absolute
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        currentObject.MoveBy(new Vector3(-speed, 0, 0));
                        break;

                    case Keys.Right:
                        currentObject.MoveBy(new Vector3(speed, 0, 0));
                        break;

                    case Keys.Up:
                        currentObject.MoveBy(new Vector3(0, speed, 0));
                        break;

                    case Keys.Down:
                        currentObject.MoveBy(new Vector3(0, -speed, 0));
                        break;

                    case Keys.Q:
                        currentObject.MoveBy(Vector3.Forward * speed);
                        break;

                    case Keys.E:
                        currentObject.MoveBy(Vector3.Backward * speed);
                        break;

                    case Keys.Oemcomma: // <
                        currentObject.Rotate(0.1f, 0, 0);
                        break;

                    case Keys.OemPeriod: // >
                        currentObject.Rotate(-0.1f, 0, 0);
                        break;

                    case Keys.OemOpenBrackets: // [
                        currentObject.Rotate(0, 0.1f, 0);
                        break;

                    case Keys.OemCloseBrackets: // ]
                        currentObject.Rotate(0, -0.1f, 0);
                        break;

                    case Keys.PageUp:
                        currentObject.Rotate(0, 0.1f, 0);
                        break;

                    case Keys.PageDown:
                        currentObject.Rotate(0, -0.1f, 0);
                        break;

                    case Keys.G:
                        if (currentObject is RandomSystem)
                            ((RandomSystem)currentObject).GeneratePart();
                        else if (currentObject is PythagorasTree)
                            ((PythagorasTree)currentObject).GenerateNextLayer();
                        else if (currentObject is PrismTree)
                            ((PrismTree)currentObject).GenerateNextLayer();
                        break;
                }
            }
            else // Relative
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        currentObject.RelativeMoveBy(new Vector3(-speed, 0, 0));
                        break;

                    case Keys.Right:
                        currentObject.RelativeMoveBy(new Vector3(speed, 0, 0));
                        break;

                    case Keys.Up:
                        currentObject.RelativeMoveBy(new Vector3(0, speed, 0));
                        break;

                    case Keys.Down:
                        currentObject.RelativeMoveBy(new Vector3(0, -speed, 0));
                        break;

                    case Keys.Q:
                        currentObject.RelativeMoveBy(new Vector3(0, 0, -speed));
                        break;

                    case Keys.E:
                        currentObject.RelativeMoveBy(new Vector3(0, 0, speed));
                        break;

                    case Keys.Oemcomma: // <
                        currentObject.RelativeRotate(0.1f, 0, 0);
                        break;

                    case Keys.OemPeriod: // >
                        currentObject.RelativeRotate(-0.1f, 0, 0);
                        break;

                    case Keys.OemOpenBrackets: // [
                        currentObject.RelativeRotate(0, 0.1f, 0);
                        break;

                    case Keys.OemCloseBrackets: // ]
                        currentObject.RelativeRotate(0, -0.1f, 0);
                        break;

                    case Keys.PageUp:
                        currentObject.RelativeRotate(0, 0, 0.1f);
                        break;

                    case Keys.PageDown:
                        currentObject.RelativeRotate(0, 0, -0.1f);
                        break;
                }
            }

            UpdateScreen();
        }

        private void UpdateScreen()
        {
            device.Clear(Color.White);
            device.Begin();

            foo.Draw(scene);
            bar.Draw(scene);
            selectionCursor.Draw(scene);
            highlightingCursor.Draw(scene);

            labelPolygons.Text = device.PolygonsCount.ToString();
            device.End();
        }

        private void scenePanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (cameraCapture != CameraCaptureState.None)
                return;

            if (e.Button == MouseButtons.Right)
            {
                cameraCapture = CameraCaptureState.Rotate;
                mousePosition = e.Location;
                this.Cursor = Cursors.Cross;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                cameraCapture = CameraCaptureState.Move;
                mousePosition = e.Location;
                this.Cursor = Cursors.SizeAll;
            }
        }

        private void scenePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (cameraCapture == CameraCaptureState.Rotate)
            {
                float dx = (float)(e.X - mousePosition.X) / scenePanel.ClientSize.Width;
                float dy = (float)(e.Y - mousePosition.Y) / scenePanel.ClientSize.Height;
                mousePosition = e.Location;

                camera.RotateInXZAround(Vector3.Zero, (float)Math.PI * 2 * dx);
                camera.RotateInYZAround(Vector3.Zero, (float)Math.PI * 2 * dy);

                UpdateScreen();
            }
            else if (cameraCapture == CameraCaptureState.Move)
            {
                float dx = (float)(e.X - mousePosition.X) / scenePanel.ClientSize.Width;
                float dy = (float)(e.Y - mousePosition.Y) / scenePanel.ClientSize.Height;
                mousePosition = e.Location;

                camera.RelativeMoveBy(new Vector3(
                    -dx * MoveByMouseSpeed,
                    dy * MoveByMouseSpeed,
                    0));
            }
            else if (isCursorEnabled)
            {
                ModelObject model = (ModelObject)foo;
                Matrix wvp = model.World * scene.View * scene.Projection;

                Triangle nearestPolygon = default(Triangle);
                double minDistance = double.MaxValue;

                Triangle projected;

                foreach (Triangle polygon in model.Polygons)
                {
                    projected = polygon;
                    if (!device.TryProjectTriangle(ref projected, ref wvp))
                        continue;

                    projected.A = viewport.TranslateToScreenSize(projected.A);
                    projected.B = viewport.TranslateToScreenSize(projected.B);
                    projected.C = viewport.TranslateToScreenSize(projected.C);

                    Vector3 center = projected.Center;
                    double distance = Math.Pow(center.X - e.X, 2) + Math.Pow(center.Y - e.Y, 2);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestPolygon = polygon;
                    }
                }

                projected = nearestPolygon;
                device.TryProjectTriangle(ref projected, ref wvp);
                projected.A = viewport.TranslateToScreenSize(projected.A);
                projected.B = viewport.TranslateToScreenSize(projected.B);
                projected.C = viewport.TranslateToScreenSize(projected.C);

                Vector3 projectedCenter = projected.Center;
                float nearbyRadius = (float)Math.Max(Math.Max(
                    Vector3.DistanceSquared(projected.A, projectedCenter),
                    Vector3.DistanceSquared(projected.B, projectedCenter)),
                    Vector3.DistanceSquared(projected.C, projectedCenter));

                if (minDistance < float.MaxValue && minDistance < nearbyRadius)
                {
                    highlightingCursor.Visible = true;
                    highlightingCursor.Position = Vector3.Transform(
                        nearestPolygon.Center, ref model.World);
                }
                else
                {
                    highlightingCursor.Visible = false;
                }
            }
        }

        private void scenePanel_MouseUp(object sender, MouseEventArgs e)
        {
            cameraCapture = CameraCaptureState.None;
            this.Cursor = Cursors.Default;

            if (e.Button == MouseButtons.Left)
            {
                selectionCursor.Position = highlightingCursor.Position;
                selectionCursor.Visible = highlightingCursor.Visible;
            }
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            camera.Zoom(e.Delta);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void useCursor_CheckedChanged(object sender, EventArgs e)
        {
            isCursorEnabled = useCursorToolStripMenuItem.Checked;
        }

        private void wireframeMode_Click(object sender, EventArgs e)
        {
            device.IsWireframe = wireframeModeToolStripMenuItem.Checked;
        }

        private void showEdges_Click(object sender, EventArgs e)
        {
            device.ShowEdges = showEdgesToolStripMenuItem.Checked;
        }

        private void cullModeChange_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            cullClockwiseToolStripMenuItem.Checked = false;
            cullCounterToolStripMenuItem.Checked = false;

            if (sender == noneToolStripMenuItem)
            {
                noneToolStripMenuItem.Checked = true;
                device.CullMode = CullMode.None;
            }
            else if (sender == cullClockwiseToolStripMenuItem)
            {
                cullClockwiseToolStripMenuItem.Checked = true;
                device.CullMode = CullMode.CullClockwiseFace;
            }
            else if (sender == cullCounterToolStripMenuItem)
            {
                cullCounterToolStripMenuItem.Checked = true;
                device.CullMode = CullMode.CullCounterClockwiseFace;
            }
        }

        private void howDoI_Click(object sender, EventArgs e)
        {
            if (!helpForm.Visible)
            {
                helpForm.Left = this.Left + (this.Width - helpForm.Width) / 2;
                helpForm.Top = this.Top + (this.Height - helpForm.Height) / 2;
            }

            helpForm.Show();
        }

        private void helpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            helpForm.Hide();

            if (!Disposing)
                e.Cancel = true;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            device.Dispose();
            graphics.Dispose();
            helpForm.Dispose();
        }
    }
}
