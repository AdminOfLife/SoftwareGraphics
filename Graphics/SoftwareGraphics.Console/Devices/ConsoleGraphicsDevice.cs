using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using SoftwareGraphics.Console;

namespace SoftwareGraphics.Devices
{
    /// <summary>
    /// The realization of the GraphicsDevice that uses Console to output.
    /// </summary>
    public sealed class ConsoleGraphicsDevice : GraphicsDevice
    {
        const int StartBufferSize = 128;

        ConsoleBuffer consoleBuffer;

        /// <summary>
        /// Initializes a new instance of the ConsoleGraphicsDevice.
        /// </summary>
        /// <param name="viewport">Viewport of the console used to draw graphics.</param>
        public ConsoleGraphicsDevice(Viewport viewport)
            : base(viewport, StartBufferSize)
        {
            consoleBuffer = new ConsoleBuffer(viewport);
        }

        /// <summary>
        /// Gets the full-window viewport of the console.
        /// </summary>
        /// <returns>The full-window viewport of the console.</returns>
        public static Viewport GetConsoleViewport()
        {
            return new Viewport(0, 0, System.Console.WindowWidth, System.Console.WindowHeight);
        }

        /// <summary>
        /// Clear a screen by filling it with specified color.
        /// </summary>
        /// <param name="fillColor">Color to fill the screen.</param>
        public override void Clear(Color fillColor)
        {
            consoleBuffer.Clear(' ', ConsoleColor.Black, ColorHelper.ToConsole(fillColor));
        }

        /// <summary>
        /// Begins the operation of drawing by allowing to draw
        /// a polygons using device.
        /// </summary>
        public override void Begin()
        {
            base.Begin();
        }

        /// <summary>
        /// Ends the operation of drawing; builds a final picture on a screen.
        /// </summary>
        public override void End()
        {
            base.End();

            DrawTriangleBuffer();
            consoleBuffer.Flush();
        }

        private void DrawTriangleBuffer()
        {
            Array.Sort(depthBuffer, triangleBuffer, 0, polygonCount);
            
            int i = polygonCount - 1;

            // skipping polygons that too far
            while (i >= 0 && depthBuffer[i] > 1f)
            {
                i--;
            }

            for (; i >= 0; i--)
            {
                if (depthBuffer[i] < 0f)
                    break;

                Triangle polygon = triangleBuffer[i];
                polygon.A = Viewport.TranslateToScreenSize(polygon.A);
                polygon.B = Viewport.TranslateToScreenSize(polygon.B);
                polygon.C = Viewport.TranslateToScreenSize(polygon.C);

                var color = ColorHelper.ToConsole(polygon.Color);
                FillTriangle(
                    (int)polygon.A.X, (int)polygon.A.Y,
                    (int)polygon.B.X, (int)polygon.B.Y,
                    (int)polygon.C.X, (int)polygon.C.Y,
                    color);
            }
        }

        //private void DrawTriangle(
        //    int x1, int y1, int x2, int y2, int x3, int y3, ConsoleColor color)
        //{
        //    DrawLine(x1, y1, x2, y2, color);
        //    DrawLine(x1, y1, x3, y3, color);
        //    DrawLine(x2, y2, x3, y3, color);
        //}

        private void FillTriangle(
            int x1, int y1, int x2, int y2, int x3, int y3, ConsoleColor color)
        {
            int x, y, xmin, xmax, ymin, ymax;
            int xx1, xx2, xa, xb;

            /* Bubble-sort y1 <= y2 <= y3 */
            if (y1 > y2)
            {
                FillTriangle(x2, y2, x1, y1, x3, y3, color);
                return;
            }

            if (y2 > y3)
            {
                FillTriangle(x1, y1, x3, y3, x2, y2, color);
                return;
            }

            /* Compute slopes and promote precision */
            int k21 = (y2 == y1) ? 0 : (x2 - x1) * 0x10000 / (y2 - y1);
            int k31 = (y3 == y1) ? 0 : (x3 - x1) * 0x10000 / (y3 - y1);
            int k32 = (y3 == y2) ? 0 : (x3 - x2) * 0x10000 / (y3 - y2);

            x1 *= 0x10000;
            x2 *= 0x10000;
            x3 *= 0x10000;

            ymin = y1 < 0 ? 0 : y1;
            ymax = y3 + 1 < Viewport.Height ? y3 + 1 : Viewport.Height;

            if (ymin < y2)
            {
                xa = x1 + k21 * (ymin - y1);
                xb = x1 + k31 * (ymin - y1);
            }
            else if (ymin == y2)
            {
                xa = x2;
                xb = (y1 == y3) ? x3 : x1 + k31 * (ymin - y1);
            }
            else                        /* (ymin > y2) */
            {
                xa = x3 + k32 * (ymin - y3);
                xb = x3 + k31 * (ymin - y3);
            }

            /* Rasterize our triangle */
            for (y = ymin; y < ymax; y++)
            {
                /* Rescale xa and xb, recentering the division */
                if (xa < xb)
                {
                    xx1 = (xa + 0x800) / 0x10000;
                    xx2 = (xb + 0x801) / 0x10000;
                }
                else
                {
                    xx1 = (xb + 0x800) / 0x10000;
                    xx2 = (xa + 0x801) / 0x10000;
                }

                xmin = xx1 < 0 ? 0 : xx1;
                xmax = xx2 + 1 < Viewport.Width ? xx2 + 1 : Viewport.Width;

                for (x = xmin; x < xmax; x++)
                {
                    consoleBuffer.Write(x, y, ' ', ConsoleColor.Gray, color);
                }

                xa += y < y2 ? k21 : k32;
                xb += k31;
            }
        }
    }
}
