using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Devices;
using RubiksCubeBase;

namespace ConsoleRubiksCube
{
    public sealed class App : IDisposable
    {
        static readonly Dictionary<char, Rotation> sideRotationByLetter =
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

        const float CameraRotateVelocity = 0.2f;

        const int AnimationSteps = 25;
        const int ShuffleAnimationSteps = 20;

        Viewport viewport;
        GraphicsDevice device;

        Camera camera;
        Scene scene;

        RubiksCube cube;
        Axis lastAxis;
        Queue<Rotation> rotationsToApply = new Queue<Rotation>();

        Random random = new Random();

        public bool ExitQueried { get; private set; }

        public App()
        {
            viewport = ConsoleGraphicsDevice.GetConsoleViewport();
            device = new ConsoleGraphicsDevice(viewport);

            Matrix projection = MatrixHelper.CreatePerspectiveFieldOfView(
                    (float)Math.PI / 4, viewport.AspectRatio, 1, 500);

            camera = new Camera(new Vector3(0, 0, 200f));
            scene = new Scene(device, camera, projection);

            cube = new RubiksCube(3, new Color[]
                {
                    Color.Black,
                    Color.Red,
                    Color.Magenta,
                    Color.Blue,
                    Color.Green,
                    Color.White,
                    Color.Yellow,
                });
            cube.World *= MatrixHelper.CreateScale(50);
            
            ExitQueried = false;
        }

        public void Update()
        {
            ConsoleKeyInfo? key = null;
            while (Console.KeyAvailable)
            {
                key = Console.ReadKey(true);
            }

            cube.UpdateAnimation();
            if (rotationsToApply.Count > 0)
            {
                Rotation top = rotationsToApply.Peek();
                if (top.TryStart(cube, AnimationSteps))
                    rotationsToApply.Dequeue();
            }

            if (!key.HasValue)
                return;

            if ((key.Value.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt)
            {
                switch (key.Value.Key)
                {
                    case ConsoleKey.S:
                        Shuffle(15);
                        break;
                }
            }
            else
            {
                switch (key.Value.Key)
                {
                    case ConsoleKey.Escape:
                        ExitQueried = true;
                        break;
                    case ConsoleKey.NumPad2:
                        camera.RotateInYZAround(cube.Position, CameraRotateVelocity);
                        break;
                    case ConsoleKey.NumPad8:
                        camera.RotateInYZAround(cube.Position, -CameraRotateVelocity);
                        break;
                    case ConsoleKey.NumPad4:
                        camera.RotateInXZAround(cube.Position, -CameraRotateVelocity);
                        break;
                    case ConsoleKey.NumPad6:
                        camera.RotateInXZAround(cube.Position, CameraRotateVelocity);
                        break;
                    default:
                        char letter = key.Value.Key.ToString()[0];
                        Rotation rotation;
                        if (sideRotationByLetter.TryGetValue(letter, out rotation))
                        {
                            if ((key.Value.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift)
                            {
                                rotation = new Rotation(
                                    rotation.Axis, rotation.Layer, !rotation.Clockwise);
                            }

                            rotationsToApply.Enqueue(rotation);
                        }
                        break;
                }
            }
        }

        public void Draw()
        {
            device.Clear(Color.LightSlateGray);
            device.Begin();

            cube.Draw(scene);

            device.End();
        }

        private void Shuffle(int shuffleCount)
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

        public void Dispose()
        {
            device.Dispose();
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(
                Console.LargestWindowWidth - 10,
                Console.LargestWindowHeight - 10);
            Console.CursorVisible = false;

            TimeSpan updateStep = TimeSpan.FromSeconds(1.0 / 60);
            DateTime lastUpdateTime;

            using (var app = new App())
            {
                lastUpdateTime = DateTime.Now;

                while (!app.ExitQueried)
                {
                    DateTime now = DateTime.Now;
                    while (lastUpdateTime + updateStep <= now)
                    {
                        app.Update();
                        lastUpdateTime += updateStep;
                    }

                    app.Draw();
                }
            }
        }
    }
}
