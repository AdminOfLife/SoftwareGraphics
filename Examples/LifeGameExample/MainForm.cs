using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SoftwareGraphics;
using SoftwareGraphics.Controls;

namespace LifeGameExample
{
    public partial class MainForm : Form
    {
        enum CameraCaptureState
        {
            None,
            Move,
            Rotate,
        }

        string WindowTitle;

        const float MoveByMouseSpeed = 500f;

        CameraCaptureState cameraCapture;
        Point mousePosition;

        Life life;
        Random random = new Random();

        string[] picture = new string[]
        {
            "#    # #### ####    ####   ## #   # ####",
            "#    # #    #       #     # # ## ## #   ",
            "#    # #    #       #    #  # # # # #   ",
            "#    # ###  ###     # ## #### #   # ### ",
            "#    # #    #       #  # #  # #   # #   ",
            "#### # #    ####    #### #  # #   # ####",
        };

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            canvas.Camera = new Camera(new Vector3(0, 0, 500f));
            canvas.Projection = MatrixHelper.CreateOrthographic(
                canvas.Viewport.Width, canvas.Viewport.Height, 0, 1000);

            life = new Life(60, 60, Color.Orange);
            life.World *= MatrixHelper.CreateScale(
                (float)canvas.ClientSize.Width / 2, (float)canvas.ClientSize.Height / 2, 1f);

            DrawPicture();

            //foo = new Life3D(scene, 9, 9, 9, Color.Orange);
            //foo.World *= MatrixHelper.CreateScale(xScale, yScale, zScale) * 0.7f;
            //lifeCursor = new LifeCursor(scene, (Life3D)foo);
            //lifeCursor.LocationOnField =
            //    new GenericMathematics.Vector3<int>(
            //        ((Life3D)foo).FieldWidth,
            //        ((Life3D)foo).FieldHeight,
            //        ((Life3D)foo).FieldDepth) / 2;

            canvas.Select();
            Application.Idle += Application_Idle;
            this.MouseWheel += MainForm_MouseWheel;

            WindowTitle = this.Text;
            UpdateInfo();
        }

        private void DrawPicture()
        {
            int left = (life.FieldWidth - picture[0].Length) / 2;
            int top = (life.FieldHeight - picture.Length) / 2;

            for (int i = 0; i < picture.Length; i++)
            {
                string line = picture[i];
                for (int j = 0; j < line.Length; j++)
                {
                    if (!char.IsWhiteSpace(line[j]))
                        life.PokeAt(left + j, top + i, true);
                }
            }

            life.GenerateView();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            canvas.Invalidate();
        }

        private void UpdateInfo()
        {
            this.Text = string.Format("{0}",
                WindowTitle);
        }

        private void canvas_Draw(object sender, CanvasDrawEventArgs e)
        {
            e.Device.Clear(Color.CornflowerBlue);
            e.Device.Begin();

            life.Draw(e.Scene);

            e.Device.End();
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.G:
                    RandomizeLife(0.3);
                    life.GenerateView();
                    break;
                case Keys.Space:
                    life.NextGeneration();
                    life.GenerateView();
                    break;
                case Keys.C:
                    life.Clear();
                    life.GenerateView();
                    break;
            }

            UpdateInfo();
        }

        private void RandomizeLife(double lifeDensity)
        {
            if (lifeDensity < 0 || lifeDensity > 1)
                throw new ArgumentOutOfRangeException("lifeDensity", "lifeDensity must be in [0..1].");

            for (int left = 0; left < life.FieldWidth; left++)
            {
                for (int top = 0; top < life.FieldHeight; top++)
                {
                    bool liveCell = random.NextDouble() <= lifeDensity;
                    life.PokeAt(left, top, liveCell);
                }
            }
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (cameraCapture != CameraCaptureState.None)
                return;

            mousePosition = e.Location;

            if (e.Button == MouseButtons.Middle)
            {
                cameraCapture = CameraCaptureState.Move;
                this.Cursor = Cursors.SizeAll;
            }
            else if (e.Button == MouseButtons.Right)
            {
                cameraCapture = CameraCaptureState.Rotate;
                this.Cursor = Cursors.Cross;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (cameraCapture == CameraCaptureState.Move)
            {
                float dx = (float)(e.X - mousePosition.X) / canvas.ClientSize.Width;
                float dy = (float)(e.Y - mousePosition.Y) / canvas.ClientSize.Height;
                mousePosition = e.Location;

                canvas.Camera.RelativeMoveBy(new Vector3(
                    -dx * MoveByMouseSpeed,
                    dy * MoveByMouseSpeed,
                    0));
            }
            else if (cameraCapture == CameraCaptureState.Rotate)
            {
                float dx = (float)(e.X - mousePosition.X) / canvas.ClientSize.Width;
                float dy = (float)(e.Y - mousePosition.Y) / canvas.ClientSize.Height;
                mousePosition = e.Location;

                canvas.Camera.RotateInXZAround(Vector3.Zero, (float)Math.PI * 2 * dx);
                canvas.Camera.RotateInYZAround(Vector3.Zero, (float)Math.PI * 2 * dy);
            }
            //else if (foo is Life3D)
            //{
            //    Life3D life = (Life3D)foo;

            //    Matrix viewProjection = canvas.Scene.View * canvas.Projection;
            //    Matrix inverse = viewProjection.Inverse();

            //    Vector3 mousePosition = new Vector3(
            //        2f * (float)e.X / canvas.ClientSize.Width - 1f,
            //        1f - 2f * (float)e.Y / canvas.ClientSize.Height,
            //        0.5f);

            //    Vector3 spaceCoords = mousePosition.Transform3D(inverse);
            //    float left = (spaceCoords.X / canvas.ClientSize.Width + 0.5f) * life.FieldWidth;
            //    float top = (-spaceCoords.Y / canvas.ClientSize.Height + 0.5f) * life.FieldHeight;

            //    var cursorLocation = new GenericMathematics.Vector3<int>(
            //        (int)left, (int)top, lifeCursor.LocationOnField.Z);

            //    if (cursorLocation.X >= 0 && cursorLocation.X < life.FieldWidth &&
            //        cursorLocation.Y >= 0 && cursorLocation.Y < life.FieldHeight &&
            //        cursorLocation.Z >= 0 && cursorLocation.Z < life.FieldDepth)
            //    {
            //        lifeCursor.LocationOnField = cursorLocation;
            //    }
            //}
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left &&
                cameraCapture == CameraCaptureState.None)
            {
                if (life is Life)
                    PokeLifeAt(e.Location, null);
                //else if (life is Life3D)
                //    lifeCursor.Poke(null);
            }

            cameraCapture = CameraCaptureState.None;
            this.Cursor = Cursors.Default;
        }

        private void PokeLifeAt(Point location, bool? cellState)
        {
            if (life is Life)
            {
                float left = (float)location.X * life.FieldWidth / canvas.ClientSize.Width;
                float top = (float)location.Y * life.FieldHeight / canvas.ClientSize.Height;

                if (left >= 0 && left < life.FieldWidth &&
                    top >= 0 && top < life.FieldHeight)
                {
                    life.PokeAt((int)left, (int)top, cellState);
                    life.GenerateView();
                }
            }
            //else if (foo is Life3D)
            //{
            //    float left = (float)location.X * life.FieldWidth / canvas.ClientSize.Width;
            //    float top = (float)location.Y * life.FieldHeight / canvas.ClientSize.Height;

            //    if (left >= 0 && left < life.FieldWidth &&
            //        top >= 0 && top < life.FieldHeight)
            //    {
            //        life.PokeAt((int)left, (int)top, life.FieldDepth / 2, cellState);
            //    }
            //}
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            //if (foo is Life3D)
            //{
            //    var cursorLocation = lifeCursor.LocationOnField;

            //    cursorLocation.Z = Math.Max(0, Math.Min(((Life3D)foo).FieldDepth - 1,
            //        cursorLocation.Z + e.Delta / 120));

            //    lifeCursor.LocationOnField = cursorLocation;
            //}
        }
    }
}
