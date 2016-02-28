using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareGraphics.Devices
{
    /// <summary>
    /// Defines polygon vertex order that may be used to identify back faces for culling.
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// Do not cull polygons.
        /// </summary>
        None,
        /// <summary>
        /// Cull polygons which have their vertexes in clockwise order.
        /// </summary>
        CullClockwiseFace,
        /// <summary>
        /// Cull polygons which have their vertexes in counter clockwise order.
        /// </summary>
        CullCounterClockwiseFace,
    }

    /// <summary>
    /// An abstract device for drawing polygons on screen.
    /// </summary>
    public abstract class GraphicsDevice : IDisposable
    {
        Viewport viewport;

        // buffers for optimization purposes
        // (used in Draw())
        Triangle[] trianglesToDraw = new Triangle[4];
        float[] depthsToDraw = new float[4];

        /// <summary>
        /// True if device is ready to receive polygons to draw; false otherwise.
        /// </summary>
        protected bool drawMode = false;
        /// <summary>
        /// The value indicating that the device was disposed.
        /// </summary>
        protected bool disposed = false;

        /// <summary>
        /// Container used for accumulating polygons in draw mode.
        /// </summary>
        protected Triangle[] triangleBuffer;
        /// <summary>
        /// Symmetrical to triangle buffer container with polygons average depths.
        /// </summary>
        protected float[] depthBuffer;
        /// <summary>
        /// Count of currently stored polygons in triangle buffer.
        /// </summary>
        protected int polygonCount = 0;

        /// <summary>
        /// Container with lights applied to current device.
        /// </summary>
        public List<Light> Lights { get; private set; }

        /// <summary>
        /// Gets or sets viewport of the screen used to draw.
        /// </summary>
        public virtual Viewport Viewport
        {
            get { return viewport; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                viewport = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating that device should not fill polygons.
        /// </summary>
        public virtual bool IsWireframe { get; set; }
        /// <summary>
        /// Gets or sets a value indicating that device should highlight polygons edges.
        /// </summary>
        public virtual bool ShowEdges { get; set; }
        /// <summary>
        /// Gets or sets a polygon culling mode for device.
        /// </summary>
        public virtual CullMode CullMode { get; set; }
        /// <summary>
        /// Gets or sets a value indicating that device should
        /// calculate lighting for each polygon.
        /// </summary>
        public virtual bool LightingEnabled { get; set; }

        /// <summary>
        /// Gets or sets general device parameters.
        /// </summary>
        public DeviceParams Parameters
        {
            get
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
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value.Viewport != null)
                    Viewport = value.Viewport;
                if (value.Lights != null)
                {
                    Lights.Clear();
                    Lights.AddRange(value.Lights);
                }

                IsWireframe = value.IsWireframe;
                ShowEdges = value.ShowEdges;
                CullMode = value.CullMode;
                LightingEnabled = value.LightingEnabled;
            }
        }

        /// <summary>
        /// Gets currently added to draw polygons count. Use this value before End() call
        /// to determine total drawing polygons count.
        /// </summary>
        public int PolygonsCount
        {
            get { return polygonCount; }
        }

        /// <summary>
        /// Initializes a new instance of the GraphicsDevice.
        /// </summary>
        /// <param name="viewport">Viewport of the screen used to draw graphics.</param>
        /// <param name="startBufferSize">The initial size of traingle and depth buffer.</param>
        public GraphicsDevice(Viewport viewport, int startBufferSize)
        {
            if (viewport == null)
                throw new ArgumentNullException("viewport");
            if (startBufferSize < 0)
                throw new ArgumentOutOfRangeException("startBufferSize");

            Viewport = viewport;
            IsWireframe = false;
            ShowEdges = false;
            CullMode = CullMode.CullCounterClockwiseFace;

            ResizeBuffers(startBufferSize);
            Lights = new List<Light>();
        }

        /// <summary>
        /// Clear a screen by filling it with specified color.
        /// </summary>
        /// <param name="fillColor">Color to fill the screen.</param>
        public virtual void Clear(Color fillColor)
        {
        }

        /// <summary>
        /// Begins the operation of drawing by allowing to draw
        /// a polygons using device.
        /// </summary>
        public virtual void Begin()
        {
            if (drawMode)
                throw new InvalidOperationException("Begin already called.");

            drawMode = true;
            polygonCount = 0;
        }

        /// <summary>
        /// Ends the operation of drawing; builds a final picture on a screen.
        /// </summary>
        public virtual void End()
        {
            if (!drawMode)
                throw new InvalidOperationException("Begin must be called before End.");

            drawMode = false;
        }

        /// <summary>
        /// Adds a projected triangles to triangle buffer, resizing it if needed.
        /// </summary>
        /// <param name="projectedTriangles">The projected on a screen polygons.</param>
        /// <param name="depths">Average depths of the polygons' vertexes.</param>
        /// <param name="startIndex">The start index to take polygons.</param>
        /// <param name="length">The count of polygons to take to draw.</param>
        protected void AddToDraw(Triangle[] projectedTriangles, float[] depths, int startIndex, int length)
        {
            // assume that we have enough space
            if (length > triangleBuffer.Length - polygonCount)
            {
                int blockCount = projectedTriangles.Length / triangleBuffer.Length;
                int newBufferSize = (blockCount + 2) * triangleBuffer.Length;
                ResizeBuffers(newBufferSize);
            }

            Array.Copy(projectedTriangles, startIndex, triangleBuffer, polygonCount, length);
            Array.Copy(depths, startIndex, depthBuffer, polygonCount, length);
            polygonCount += length;
        }

        private void ResizeBuffers(int newBufferSize)
        {
            Array.Resize(ref triangleBuffer, newBufferSize);
            Array.Resize(ref depthBuffer, newBufferSize);
        }

        private Vector3 ProjectVector(Vector3 v, ref Matrix transform, out float wComponent)
        {
            // matrix applied to right side (row-vector)
            float x =
                v.X * transform.M11 +
                v.Y * transform.M21 +
                v.Z * transform.M31 +
                transform.M41;
            float y =
                v.X * transform.M12 +
                v.Y * transform.M22 +
                v.Z * transform.M32 +
                transform.M42;
            float z =
                v.X * transform.M13 +
                v.Y * transform.M23 +
                v.Z * transform.M33 +
                transform.M43;
            wComponent =
                v.X * transform.M14 +
                v.Y * transform.M24 +
                v.Z * transform.M34 +
                transform.M44;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Projects a triangle, culling it if necessary.
        /// </summary>
        /// <param name="triangle">The polygon to project; the projected polygon.</param>
        /// <param name="worldViewProjection">Transformations applied to polygon.</param>
        /// <returns>True if polygon was not culled; false otherwise.</returns>
        public bool TryProjectTriangle(ref Triangle triangle, ref Matrix worldViewProjection)
        {
            Vector3 w;
            var pA = ProjectVector(triangle.A, ref worldViewProjection, out w.X);
            var pB = ProjectVector(triangle.B, ref worldViewProjection, out w.Y);
            var pC = ProjectVector(triangle.C, ref worldViewProjection, out w.Z);

            const float epsilon = float.Epsilon;
            // return projection failure if w.XYZ <= 0,
            // because it causes rendering glitches otherwise
            if (w.X < epsilon || w.Y < epsilon || w.Z < epsilon)
            {
                return false;
            }

            pA /= w.X;
            pB /= w.Y;
            pC /= w.Z;

            float x1 = pB.X - pA.X;
            float y1 = pB.Y - pA.Y;
            float x2 = pC.X - pA.X;
            float y2 = pC.Y - pA.Y;

            //        (i,  j,  k)
            // if det (x1, y1, 0) < 0 => v1 cross v2 is directed down (z < 0)
            //        (x2, y2, 0)
            bool clockwise = x1 * y2 < x2 * y1;

            if (!clockwise && CullMode == CullMode.CullCounterClockwiseFace ||
                clockwise && CullMode == CullMode.CullClockwiseFace)
            {
                return false;
            }

            triangle = new Triangle(pA, pB, pC, triangle.Color);
            return true;
        }

        private void EnsureDrawBuffersSize(int length)
        {
            if (trianglesToDraw.Length < length)
            {
                int newSize = trianglesToDraw.Length;
                while (newSize < length)
                {
                    newSize *= 2;
                }

                trianglesToDraw = new Triangle[newSize];
                depthsToDraw = new float[newSize];
            }
        }

        private void CalculateLights(Triangle[] triangles, int startIndex, int triangleCount, ref Matrix world)
        {
            for (int j = 0; j < triangleCount; j++)
            {
                int i = startIndex + j;
                Triangle triangle = triangles[i];

                Vector3 w;
                triangle.A = ProjectVector(triangle.A, ref world, out w.X);
                triangle.B = ProjectVector(triangle.B, ref world, out w.Y);
                triangle.C = ProjectVector(triangle.C, ref world, out w.Z);

                Vector3 normal = triangle.Normal;
                Vector3 lightIntensivity = Vector3.Zero;

                foreach (Light light in Lights)
                {
                    Vector3 intensivity = light.CalculateIntensivity(triangle, normal);
                    if (intensivity.X > 0)
                        lightIntensivity.X += intensivity.X;
                    if (intensivity.Y > 0)
                        lightIntensivity.Y += intensivity.Y;
                    if (intensivity.Z > 0)
                        lightIntensivity.Z += intensivity.Z;
                }

                triangle.Color = Light.CalculateColor(
                    triangle.Color, lightIntensivity);

                triangles[i] = triangle;
            }
        }

        /// <summary>
        /// Draw a specified polygons onto the screen.
        /// </summary>
        /// <param name="values">The polygons to draw.</param>
        /// <param name="worldViewProjection">Transformations applied to each polygon.</param>
        public virtual void Draw(ICollection<Triangle> values,
            Matrix world, Matrix view, Matrix projection)
        {
            if (!drawMode)
                throw new InvalidOperationException("Begin must be called.");

            int polygonsToDrawCount = values.Count;
            EnsureDrawBuffersSize(polygonsToDrawCount);
            values.CopyTo(trianglesToDraw, 0);
            
            int index = 0;
            Matrix transform = new Matrix();

            if (LightingEnabled)
            {
                const int PolygonCountPerTask = 256;

                int taskCount = polygonsToDrawCount / PolygonCountPerTask;
                Parallel.For(0, taskCount, taskIndex =>
                {
                    CalculateLights(
                        trianglesToDraw, taskIndex * PolygonCountPerTask,
                        PolygonCountPerTask, ref world);
                });

                int remainderIndex = taskCount * PolygonCountPerTask;
                int remainderLength = polygonsToDrawCount % PolygonCountPerTask;
                CalculateLights(trianglesToDraw, remainderIndex, remainderLength, ref world);

                Matrix.Multiply(ref view, ref projection, out transform);
            }
            else
            {
                transform = world * view * projection;
            }

            for (int i = 0; i < polygonsToDrawCount; i++)
            {
                Triangle polygon = trianglesToDraw[i];
                if (TryProjectTriangle(ref polygon, ref transform))
                {
                    trianglesToDraw[index] = polygon;
                    depthsToDraw[index] = (polygon.A.Z + polygon.B.Z + polygon.C.Z) / 3f;
                    index++;
                }
            }

            AddToDraw(trianglesToDraw, depthsToDraw, 0, index);
        }

        /// <summary>
        /// Disposes current object.
        /// </summary>
        /// <param name="disposing">True if object are not finilizing; false otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            disposed = true;
        }

        /// <summary>
        /// Disposes current object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
