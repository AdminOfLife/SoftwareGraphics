using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics.Console;
using SoftwareGraphics;
using SoftwareGraphics.Devices;
using SoftwareGraphics.Objects;

namespace ConsoleDrawer
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(
                Console.LargestWindowWidth - 10, 
                Console.LargestWindowHeight - 10);

            Viewport viewport = ConsoleGraphicsDevice.GetConsoleViewport();
            using (var device = new ConsoleGraphicsDevice(viewport))
            {
                device.CullMode = CullMode.None;

                Console.CursorVisible = false;

                //Matrix projection = MatrixHelper.CreateOrthographic(
                //    device.Viewport.Width, device.Viewport.Height, 0, 1000);
                Matrix projection = MatrixHelper.CreatePerspectiveFieldOfView(
                    (float)Math.PI / 4, viewport.AspectRatio, 1, 500);

                Camera camera = new Camera(new Vector3(0, 0, 200f));

                Scene scene = new Scene(device, camera, projection);

                float k = 5;
                var p = new Vector3(0, k, 0);
                var a = new Vector3(-k, -k, k);
                var b = new Vector3(0, -k, -k);
                var c = new Vector3(k, -k, k);

                //ModelObject foo = new Tetrahedron(scene, p, a, b, c,
                //    System.Drawing.Color.Yellow,
                //    System.Drawing.Color.Green,
                //    System.Drawing.Color.Blue,
                //    System.Drawing.Color.Red);

                //ModelObject foo = new AxisLines(scene, 4, 15, 5);
                //ModelObject foo = new RandomSystem(scene, new Vector3(30), 5, new Random());
                //ModelObject foo = new BizzareSphere(scene, 20, Color.Blue, new Random());
                //ModelObject foo = new PythagorasTree(new Vector3(10, 0, 0), new Vector3(2, 5, 0), 1f);

                ModelObject foo = new ModelObject();
                //var polygons = ModelBuilder.CreateCone(
                //    VectorHelper.Right * 15, VectorHelper.Up, VectorHelper.Up * 25, 12, Color.Cyan);
                var polygons = ModelBuilder.CreateSphere(25, 8, 8,
                    ColorHelper.GetFixedColors(Color.White, Color.Blue).GetEnumerator());
                /*float w = 30;
                var polygons = ModelBuilder.CreateParallelepiped(
                	new Vector3(-w, w / 2, w / 2),
                	new Vector3(2 * w, 0, 0),
                	new Vector3(0, -w, 0),
                	new Vector3(0, 0, -w),
                	Color.Red);*/
                Random random = new Random();
                //for (int i = 0; i < polygons.Length; i++)
                //{
                //    polygons[i].Color = Color.FromArgb(
                //        random.Next(256),
                //        random.Next(256),
                //        random.Next(256));
                //}
                foo.Polygons.AddRange(polygons);

                var lights = new Tuple<PointLight, Color>[]
                    {
                        Tuple.Create(new PointLight(new Vector3(40), 5, Color.White), Color.Yellow),
                        Tuple.Create(new PointLight(new Vector3(40, -35, -40), 5, Color.Red), Color.Red),
                    };

                var lightSources = new List<ModelObject>();
                foreach (var tuple in lights)
                {
                    ModelObject source = new ModelObject();
                    polygons = ModelBuilder.CreateCube(10, tuple.Item2);
                    source.Polygons.AddRange(polygons);
                    source.Position = tuple.Item1.Position;

                    lightSources.Add(source);
                    device.Lights.Add(tuple.Item1);
                }

                device.LightingEnabled = true;

                bool exit = false;
                while (!exit)
                {
                    device.Clear(System.Drawing.Color.LightSlateGray);
                    device.Begin();

                    foo.Draw(scene);

                    device.LightingEnabled = false;
                    foreach (var lightSource in lightSources)
                    {
                        lightSource.Draw(scene);
                    }
                    device.LightingEnabled = true;

                    device.End();
                    
                    Console.SetCursorPosition(0, 0);
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey(true);
                    }
                    while (Console.KeyAvailable);
                    
                    const float CameraRotateVelocity = 0.2f;

					bool shift = (int)(key.Modifiers & ConsoleModifiers.Shift) != 0;
                    switch (key.Key)
                    {
                        case ConsoleKey.X:
                            if (shift)
                                foo.RelativeRotate(0.2f, 0, 0);
                            else
                                foo.Rotate(0.2f, 0, 0);
                            break;
                        case ConsoleKey.Y:
                            if (shift)
                                foo.RelativeRotate(0, 0.2f, 0);
                            else
                                foo.Rotate(0, 0.2f, 0);
                            break;
                        case ConsoleKey.Z:
                            if (shift)
                                foo.RelativeRotate(0, 0, 0.2f);
                            else
                                foo.Rotate(0, 0, 0.2f);
							break;
                        case ConsoleKey.NumPad2:
                            camera.RotateInYZAround(foo.Position, CameraRotateVelocity);
                            break;
                        case ConsoleKey.NumPad8:
                            camera.RotateInYZAround(foo.Position, -CameraRotateVelocity);
                            break;
                        case ConsoleKey.NumPad4:
                            camera.RotateInXZAround(foo.Position, CameraRotateVelocity);
                            break;
                        case ConsoleKey.NumPad6:
                            camera.RotateInXZAround(foo.Position, -CameraRotateVelocity);
                            break;
                        case ConsoleKey.UpArrow:
                            foo.MoveBy(camera.Up);
                            break;
                        case ConsoleKey.DownArrow:
                            foo.MoveBy(-camera.Up);
                            break;
                        case ConsoleKey.LeftArrow:
                            foo.MoveBy(-camera.Right);
                            break;
                        case ConsoleKey.RightArrow:
                            foo.MoveBy(camera.Right);
                            break;
                        case ConsoleKey.Q:
                            foo.MoveBy(camera.Look);
                            break;
                        case ConsoleKey.E:
                            foo.MoveBy(-camera.Look);
                            break;
                        case ConsoleKey.G:
                            if (foo is PythagorasTree)
                                ((PythagorasTree)foo).GenerateNextLayer();
                            break;
                        case ConsoleKey.Escape:
                            exit = true;
                            break;
                    }
                }
            }
        }
    }
}
