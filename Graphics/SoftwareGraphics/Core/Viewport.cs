using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    /// <summary>
    /// A defined region of the screen with depth specified.
    /// </summary>
    public sealed class Viewport : ICloneable
    {
        /// <summary>
        /// X-coordinate (from left to right) on the screen.
        /// </summary>
        public int X { get; private set; }
        /// <summary>
        /// Y-coordinate (from top to bottom) on the screen.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// A width of this region of the screen.
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// A height of this region of the screen.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the aspect ratio for this region.
        /// </summary>
        public float AspectRatio
        {
            get
            {
                if (Width != 0 && Height != 0)
                {
                    return (float)Width / (float)Height;
                }

                return 0f;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Viewport.
        /// </summary>
        /// <param name="x">X-coordinate (from left to right) on the screen.</param>
        /// <param name="y">Y-coordinate (from top to bottom) on the screen.</param>
        /// <param name="width">A width of this region of the screen.</param>
        /// <param name="height">A height of this region of the screen.</param>
        public Viewport(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates an exact copy of this viewport.
        /// </summary>
        /// <returns>The viewport this method creates, casted as an object.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Projects a vector onto a screen plane, where X and Y are [-1, +1],
        /// setting Z-coordinate between [0, 1] if it fits between culling planes.
        /// </summary>
        /// <param name="source">Vector3 to project.</param>
        /// <param name="worldViewProjection">Transformations applied to vector.</param>
        /// <returns></returns>
        public Vector3 Project(Vector3 source, 
            Matrix worldViewProjection)
        {
            var vector = Vector3.Transform(source, ref worldViewProjection);

            float a =
                source.X * worldViewProjection.M14 +
                source.Y * worldViewProjection.M24 +
                source.Z * worldViewProjection.M34 +
                worldViewProjection.M44;

            if (!WithinEpsilon(a, 1f))
            {
                vector /= a;
            }

            return vector;
        }

        /// <summary>
        /// Translates a vector from 3D space into screen space; setting Z-coordinate
        /// unchanged (usually [0, 1] with typical projection matrices).
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Vector3 TranslateToScreenSize(Vector3 source)
        {
            source.X = (source.X + 1f) * 0.5f * Width + this.X;
            source.Y = (-source.Y + 1f) * 0.5f * Height + this.Y;

            return source;
        }

        private bool WithinEpsilon(float a, float b)
        {
            float diff = a - b;
            return -float.Epsilon <= diff && diff <= float.Epsilon;
        }
    }
}
