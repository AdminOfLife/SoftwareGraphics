using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SoftwareGraphics.Devices
{
    /// <summary>
    /// The realization of the GraphicsDevice that uses GDI graphics to draw.
    /// </summary>
    public sealed class GdiGraphicsDevice : GraphicsDevice
    {
        const int StartBufferSize = 128;

        /// <summary>
        /// Gets the graphics used to draw polygons on.
        /// </summary>
        public System.Drawing.Graphics Graphics { get; private set; }

        public override Viewport Viewport
        {
            get { return base.Viewport; }
            set
            {
                if (drawBuffer != null)
                    drawBuffer.Dispose();
                if (bufferGraphics != null)
                    bufferGraphics.Dispose();

                base.Viewport = value;
                drawBuffer = new Bitmap(Viewport.Width, Viewport.Height);
                bufferGraphics = System.Drawing.Graphics.FromImage(drawBuffer);
            }
        }

        Bitmap drawBuffer;
        System.Drawing.Graphics bufferGraphics;
        // array for storing triangle vertices
        PointF[] polygonPoints = new PointF[3];

        /// <summary>
        /// Initializes a new instance of the GdiGraphicsDevice.
        /// </summary>
        /// <param name="graphics">Graphics used to draw polygons on.</param>
        /// <param name="viewport">Viewport of the screen used to draw graphics.</param>
        public GdiGraphicsDevice(System.Drawing.Graphics graphics, Viewport viewport)
            : base(viewport, StartBufferSize)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");

            Graphics = graphics;
        }

        /// <summary>
        /// Clear a screen by filling it with specified color.
        /// </summary>
        /// <param name="fillColor">Color to fill the screen.</param>
        public override void Clear(Color fillColor)
        {
            bufferGraphics.Clear(fillColor);
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

            DrawBuffer();
            Graphics.DrawImageUnscaled(drawBuffer, Viewport.X, Viewport.Y);
        }

        private void DrawBuffer()
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

                polygonPoints[0] = new PointF(polygon.A.X, polygon.A.Y);
                polygonPoints[1] = new PointF(polygon.B.X, polygon.B.Y);
                polygonPoints[2] = new PointF(polygon.C.X, polygon.C.Y);

                if (!IsWireframe)
                {
                    using (Brush brush = new SolidBrush(polygon.Color))
                    {
                        bufferGraphics.FillPolygon(brush, polygonPoints);
                    }

                    if (ShowEdges)
                    {
                        unchecked
                        {
                            float hue = 180f + polygon.Color.GetHue();
                            if (hue > 360f)
                                hue -= 360f;

                            Color edgeColor = ColorHelper.FromAhsb(
                                polygon.Color.A,
                                hue,
                                polygon.Color.GetSaturation(),
                                polygon.Color.GetBrightness());

                            using (Pen pen = new Pen(edgeColor))
                            {
                                bufferGraphics.DrawPolygon(pen, polygonPoints);
                            }
                        }
                    }
                }
                else
                {
                    using (Pen pen = new Pen(polygon.Color))
                    {
                        bufferGraphics.DrawPolygon(pen, polygonPoints);
                    }
                }
            }
        }

        /// <summary>
        /// Disposes current object.
        /// </summary>
        /// <param name="disposing">True if object are not finilizing; false otherwise.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            base.Dispose(disposing);

            drawBuffer.Dispose();
        }
    }
}
