using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using RubiksCubeBase;
using SoftwareGraphics;
using SoftwareGraphics.Controls;
using SoftwareGraphics.Devices;
using SoftwareGraphics.Objects;

namespace WinformsRubiksCube
{
    public partial class MainForm : Form
    {
        enum CameraCaptureState
        {
            None,
            Move,
            Rotate,
        }

        const float MoveByMouseSpeed = 750f;

        const int AnimationSteps = 25;
        const int ShuffleAnimationSteps = 20;
        const int UndoAnimationSteps = 20;

        readonly Dictionary<char, Rotation> sideRotationByLetter =
            new Dictionary<char, Rotation>()
            {
                { 'L', new Rotation(Axis.Left, 0, false)  },
                { 'M', new Rotation(Axis.Left, 1, false)  },
                { 'R', new Rotation(Axis.Left, 2, true)   },
                { 'U', new Rotation(Axis.Top, 0, false)   },
                { 'E', new Rotation(Axis.Top, 1, true)    },
                { 'D', new Rotation(Axis.Top, 2, true)    },
                { 'F', new Rotation(Axis.Depth, 0, false) },
                { 'S', new Rotation(Axis.Depth, 1, false) },
                { 'B', new Rotation(Axis.Depth, 2, true)  },
            };

        TimeSpan updateStep = TimeSpan.FromSeconds(1.0 / 60);
        DateTime lastUpdateTime;

        RubiksCube cube;

        CameraCaptureState cameraCapture;
        Point mousePosition;

        CubeCoords firstCoords;
        CubeSide firstSide;
        bool wasFirstClick = false;

        DrawableObject rotationCursor;
        bool cursorVisible = false;

        int shuffleCount = 10;
        Random random = new Random();
        Axis lastAxis;
        Queue<Rotation> rotationsToApply = new Queue<Rotation>();

        bool undoInProcess = false;
        Stack<Rotation> undoRotations = new Stack<Rotation>();

        public MainForm()
        {
            InitializeComponent();

            canvas.Projection = MatrixHelper.CreatePerspectiveFieldOfView(
                (float)Math.PI / 4, canvas.Viewport.AspectRatio, 1, 1000);

            canvas.Camera = new Camera(new Vector3(0, 0, 500));

            cube = new RubiksCube(10);
            cube.World *= MatrixHelper.CreateScale(100);

            rotationCursor = new Sphere(125f * 0.3f / cube.Size, Color.Cyan);

            canvas.GraphicsDevice.Lights.Add(new PointLight(new Vector3(150), 4.5f, Color.White));
            canvas.GraphicsDevice.Lights.Add(new PointLight(new Vector3(-150), 4.5f, Color.White));

            //canvas.GraphicsDevice.LightingEnabled = true;
            canvas.GraphicsDevice.CullMode = CullMode.CullCounterClockwiseFace;

            lastUpdateTime = DateTime.Now;
            Application.Idle += Application_Idle;
            canvas.Select();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            while (lastUpdateTime + updateStep <= now)
            {
                UpdateCube();
                lastUpdateTime += updateStep;
            }

            canvas.Invalidate();
        }

        private void UpdateCube()
        {
            if (undoInProcess)
            {
                if (undoRotations.Any())
                {
                    if (undoRotations.Peek().TryStart(cube, UndoAnimationSteps))
                        undoRotations.Pop();
                }
                else
                {
                    undoInProcess = false;
                }
            }
            else if (rotationsToApply.Any() &&
                rotationsToApply.Peek().TryStart(cube, ShuffleAnimationSteps))
            {
                Rotation rotation = rotationsToApply.Dequeue();
                undoRotations.Push(new Rotation(rotation.Axis, rotation.Layer, !rotation.Clockwise));
            }

            cube.UpdateAnimation();
        }

        private void canvas_Draw(object sender, CanvasDrawEventArgs e)
        {
            e.Device.Clear(Color.CornflowerBlue);
            e.Device.Begin();

            cube.Draw(canvas.Scene);

            if (cursorVisible)
                rotationCursor.Draw(canvas.Scene);

            e.Device.End();
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
            if (cameraCapture == CameraCaptureState.None)
            {
                this.Text = cube.DebugClick(e.Location, canvas.Scene, Matrix.Identity);
            }
            else if (cameraCapture == CameraCaptureState.Move)
            {
                float dx = (float)(e.X - mousePosition.X) / canvas.Width;
                float dy = (float)(e.Y - mousePosition.Y) / canvas.Height;
                mousePosition = e.Location;

                canvas.Camera.RelativeMoveBy(new Vector3(
                    -dx * MoveByMouseSpeed,
                    dy * MoveByMouseSpeed,
                    0));

                canvas.Invalidate();
            }
            else if (cameraCapture == CameraCaptureState.Rotate)
            {
                float dx = (float)(e.X - mousePosition.X) / canvas.Width;
                float dy = (float)(e.Y - mousePosition.Y) / canvas.Height;
                mousePosition = e.Location;

                canvas.Camera.RotateInXZAround(Vector3.Zero, (float)Math.PI * 2 * dx);
                canvas.Camera.RotateInYZAround(Vector3.Zero, (float)Math.PI * 2 * dy);

                canvas.Invalidate();
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (cameraCapture == CameraCaptureState.None)
            {
                if (!wasFirstClick)
                {
                    if (cube.FindSideByPoint(e.Location, canvas.Scene,
                            Matrix.Identity, out firstCoords, out firstSide))
                    {
                        wasFirstClick = true;

                        Vector3 cubePosition = CoordsHelper.GetPositionFromCoords(
                            firstCoords, cube.Size);

                        // move cursor by half size of small cube 
                        cubePosition += CoordsHelper.GetNormalFromSide(firstSide) / cube.Size;
                        cubePosition = Vector3.Transform(cubePosition, ref cube.World);

                        rotationCursor.Position = cubePosition;
                        cursorVisible = true;
                    }
                }
                else
                {
                    CubeCoords secondCoords;
                    CubeSide secondSide;

                    if (cube.FindSideByPoint(e.Location, canvas.Scene,
                            Matrix.Identity, out secondCoords, out secondSide))
                    {
                        Rotation rotation = cube.CreateRotationFromSides(
                            firstCoords, firstSide, secondCoords, secondSide);

                        if (rotation != null)
                        {
                            rotationsToApply.Enqueue(rotation);
                        }

                        wasFirstClick = false;
                        cursorVisible = false;
                    }
                }
            }

            cameraCapture = CameraCaptureState.None;
            this.Cursor = Cursors.Default;
        }

        private void Shuffle()
        {
            if (rotationsToApply.Count == 0)
                lastAxis = (Axis)random.Next(3);

            for (int i = 0; i < shuffleCount; i++)
            {
                Axis axis;
                do
                {
                    axis = (Axis)random.Next(3);
                }
                while (axis == lastAxis);
                lastAxis = axis;

                int layer = random.Next(cube.Size);
                bool clockwise = random.Next(2) == 0;

                rotationsToApply.Enqueue(new Rotation(axis, layer, clockwise));
            }
        }

        private void CopyToClipboard()
        {
            StringBuilder result = new StringBuilder();

            var dataCube = cube.GetCurrentState();
            for (var side = CubeSide.Left; side <= CubeSide.Back; side++)
            {
                result.Append('(');

                foreach (var coords in CoordsHelper.EnumerateSide(side, cube.Size))
                {
                    result.Append(dataCube[coords][side]);
                }

                result.Append(')');
            }

            Clipboard.SetText(result.ToString());
        }

        private bool PasteFromClipboard()
        {
            string text = Clipboard.GetText();
            if (text.Length == 0)
                return false;

            var dataCube = RubiksCube.GetDefaultCube(cube.Size);

            // cune has 6 sides
            int[][] sides = new int[6][];
            int index = 0;

            for (int i = 0; i < sides.Length; i++)
            {
                int openBracket = text.IndexOf('(', index);
                if (openBracket == -1)
                    return false;

                int closeBracket = text.IndexOf(')', openBracket + 1);
                if (closeBracket == -1)
                    return false;

                index = closeBracket + 1;

                string colors = text.Substring(
                    openBracket + 1, closeBracket - openBracket - 1);
                colors = new string(colors.ToCharArray()
                    .Where(ch => char.IsDigit(ch)).ToArray());

                // cube side has 3x3 elements
                if (colors.Length != cube.Size * cube.Size)
                    return false;

                sides[i] = new int[colors.Length];
                for (int j = 0; j < colors.Length; j++)
                {
                    char digit = colors[j];
                    if (digit < '1' || digit > '6')
                        return false;

                    sides[i][j] = (int)(digit - '0');
                }
            }

            // check that remainder part of text is whitespace chars
            for (; index < text.Length; index++)
            {
                if (!char.IsWhiteSpace(text[index]))
                    return false;
            }

            for (int i = 0; i < sides.Length; i++)
            {
                var side = (CubeSide)i;
                
                int j = 0;
                foreach (var coords in CoordsHelper.EnumerateSide(side, cube.Size))
                {
                    dataCube[coords][side] = sides[i][j];
                    j++;
                }
            }

            cube.ReplaceCube(dataCube);
            return true;
        }

        private void canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        Shuffle();
                        break;
                    case Keys.R:
                        rotationsToApply.Clear();
                        break;
                    case Keys.U:
                        undoInProcess = true;
                        break;
                    case Keys.C:
                        CopyToClipboard();
                        break;
                    case Keys.V:
                        PasteFromClipboard();
                        break;
                    case Keys.W:
                        canvas.GraphicsDevice.IsWireframe =
                            !canvas.GraphicsDevice.IsWireframe;
                        break;
                }
            }
            else
            {
                string letter = e.KeyCode.ToString();
                if (letter.Length == 1)
                {
                    Rotation rotation;
                    if (sideRotationByLetter.TryGetValue(letter[0], out rotation))
                    {
                        if (e.Shift)
                        {
                            rotation = new Rotation(
                                rotation.Axis, rotation.Layer, !rotation.Clockwise);
                        }

                        rotationsToApply.Enqueue(rotation);
                    }
                }
            }
        }
    }
}
