using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace SoftwareGraphics.Devices
{
    /// <summary>
    /// The realization of the GraphicsDevice that uses Direct3D to output.
    /// </summary>
    public sealed class Direct3DGraphicsDevice : GraphicsDevice
    {
        const int StartBufferSize = 1024;

        Microsoft.Xna.Framework.Graphics.GraphicsDevice device;
        IntPtr renderWindowHandle;
        PresentationParameters presentationParams;

        Effect basicEffect;

        RasterizerState rasterizerState;
        FillMode fillMode = FillMode.Solid;

        VertexPositionColor[] vertices;
        DynamicVertexBuffer vertexBuffer;

        public override Viewport Viewport
        {
            get { return base.Viewport; }
            set
            {
                base.Viewport = value;
                UpdatePresentationParams();
                if (device != null)
                    device.Reset(presentationParams);
            }
        }

        public override bool IsWireframe
        {
            get { return fillMode == FillMode.WireFrame; }
            set
            {
                fillMode = value ? FillMode.WireFrame : FillMode.Solid;
                UpdateRasterizerState();
            }
        }

        public Direct3DGraphicsDevice(IntPtr renderWindowHandle, SoftwareGraphics.Viewport viewport)
            : base(viewport, StartBufferSize)
        {
            this.renderWindowHandle = renderWindowHandle;
            UpdatePresentationParams();

            device = new Microsoft.Xna.Framework.Graphics.GraphicsDevice(
                GraphicsAdapter.DefaultAdapter,
                GraphicsProfile.Reach,
                presentationParams);

            basicEffect = new BasicEffect(device)
            {
                VertexColorEnabled = true,
            };

            vertices = new VertexPositionColor[StartBufferSize * 3];
            vertexBuffer = CreateVertexBuffer();
            vertexBuffer.ContentLost += VertexBuffer_ContentLost;
            
            UpdateRasterizerState();
        }

        private void UpdatePresentationParams()
        {
            presentationParams = new PresentationParameters()
            {
                DeviceWindowHandle = renderWindowHandle,
                IsFullScreen = false,
                BackBufferWidth = Viewport.Width,
                BackBufferHeight = Viewport.Height,
                BackBufferFormat = SurfaceFormat.Color,
                DepthStencilFormat = DepthFormat.Depth16,
            };
        }

        private void UpdateRasterizerState()
        {
            rasterizerState = new RasterizerState()
            {
                FillMode = fillMode,
                CullMode = Microsoft.Xna.Framework.Graphics.CullMode.None,
            };
        }

        public override void Clear(System.Drawing.Color fillColor)
        {
            device.Clear(new Microsoft.Xna.Framework.Color(
                fillColor.R, fillColor.G, fillColor.B, fillColor.A));
        }

        public override void Begin()
        {
            base.Begin();
        }

        public override void End()
        {
            base.End();

            if (device.GraphicsDeviceStatus == GraphicsDeviceStatus.Lost)
                throw new DeviceLostException();

            if (device.GraphicsDeviceStatus == GraphicsDeviceStatus.NotReset)
                device.Reset();

            var previousState = device.RasterizerState;
            device.RasterizerState = rasterizerState;

            DrawTriangleBuffer();

            device.RasterizerState = previousState;
        }

        private DynamicVertexBuffer CreateVertexBuffer()
        {
            return new DynamicVertexBuffer(
                device, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
        }

        private void VertexBuffer_ContentLost(object sender, EventArgs e)
        {
            vertexBuffer.SetData(vertices);
        }

        private void ResizeBuffers()
        {
            int length = vertices.Length;
            checked
            {
                while (polygonCount * 3 > length)
                {
                    length *= 2;
                }
            }

            // check if we need to resize buffer
            if (length != vertices.Length)
            {
                Array.Resize(ref vertices, length);

                vertexBuffer.ContentLost -= VertexBuffer_ContentLost;
                vertexBuffer.Dispose();

                vertexBuffer = CreateVertexBuffer();
                vertexBuffer.ContentLost += VertexBuffer_ContentLost;
            }
        }

        private void DrawTriangleBuffer()
        {
            if (polygonCount * 3 > vertices.Length)
                ResizeBuffers();

            int vertexIndex = 0;
            for (int i = 0; i < polygonCount; i++)
            {
                Triangle polygon = triangleBuffer[i];
                var color = new Microsoft.Xna.Framework.Color(
                    polygon.Color.R,
                    polygon.Color.G,
                    polygon.Color.B);

                vertices[vertexIndex + 0] = new VertexPositionColor(
                    new Microsoft.Xna.Framework.Vector3(polygon.A.X, polygon.A.Y, polygon.A.Z), color);
                vertices[vertexIndex + 1] = new VertexPositionColor(
                    new Microsoft.Xna.Framework.Vector3(polygon.B.X, polygon.B.Y, polygon.B.Z), color);
                vertices[vertexIndex + 2] = new VertexPositionColor(
                    new Microsoft.Xna.Framework.Vector3(polygon.C.X, polygon.C.Y, polygon.C.Z), color);

                vertexIndex += 3;
            }

            vertexBuffer.SetData(vertices);
            device.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                if (polygonCount > 0)
                    device.DrawPrimitives(PrimitiveType.TriangleList, 0, polygonCount);
            }

            device.SetVertexBuffer(null);
            device.Present();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                vertexBuffer.Dispose();
                basicEffect.Dispose();
                device.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
