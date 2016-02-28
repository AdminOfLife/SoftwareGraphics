using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using GenericMathematics;

using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace DirectGraphics
{
	public sealed class HyperGraphicsDevice
	{
		const int StartBufferSize = 128;

		Scene scene;

		HyperLine[] privitiveBuffer;
		float[] depthBuffer;

		int primitiveCount = 0;
		bool drawMode = false;

		public HyperGraphicsDevice(Scene scene3d)
		{
			scene = scene3d;

			privitiveBuffer = new HyperLine[StartBufferSize];
			depthBuffer = new float[StartBufferSize];
		}

		public void Clear(Color fillColor)
		{
			scene.Device.Clear(fillColor);
		}

		public void Begin()
		{
			if (drawMode)
				throw new InvalidOperationException("Begin already called.");

			drawMode = true;
			primitiveCount = 0;
		}

		public void End()
		{
			if (!drawMode)
				throw new InvalidOperationException("Begin must be called before End.");

			drawMode = false;
			DrawBuffer();
		}

		private void DrawBuffer()
		{
			Array.Sort(depthBuffer, privitiveBuffer, 0, primitiveCount);

			scene.Device.Begin();

			var triangles3d = privitiveBuffer
				.Where(polygon =>
					polygon.Start.W >= 0 && polygon.Start.W <= 1 &&
					polygon.End.W >= 0 && polygon.End.W <= 1)
				.Select(polygon =>
					new Triangle(
						new Vector3(polygon.Start.X, polygon.Start.Y, polygon.Start.Z),
						new Vector3(polygon.End.X, polygon.End.Y, polygon.End.Z),
						new Vector3(polygon.End.X, polygon.End.Y, polygon.End.Z),
						polygon.Color))
				.Take(primitiveCount)
				.ToArray();
			
			scene.Device.Draw(triangles3d, Matrix.Identity, scene.View, scene.Projection);

			scene.Device.End();
		}

		private void AddToDraw(HyperLine projectedTriangle, float depth)
		{
			if (primitiveCount == privitiveBuffer.Length)
			{
				int newBufferSize = primitiveCount * 2;
				Array.Resize(ref privitiveBuffer, newBufferSize);
				Array.Resize(ref depthBuffer, newBufferSize);
			}

			privitiveBuffer[primitiveCount] = projectedTriangle;
			depthBuffer[primitiveCount] = depth;
			primitiveCount++;
		}

		private Vector4 ProjectVector(Vector4 v, Matrix<float> transform, out float uComponent)
		{
			// matrix applied to right side (row-vector)
			float x =
				v.X * transform.Elements[0] +
				v.Y * transform.Elements[5] +
				v.Z * transform.Elements[10] +
				v.W * transform.Elements[15] +
				transform.Elements[20];
			float y =
				v.X * transform.Elements[1] +
				v.Y * transform.Elements[6] +
				v.Z * transform.Elements[11] +
				v.W * transform.Elements[16] +
				transform.Elements[21];
			float z =
				v.X * transform.Elements[2] +
				v.Y * transform.Elements[7] +
				v.Z * transform.Elements[12] +
				v.W * transform.Elements[17] +
				transform.Elements[22];
			float w =
				v.X * transform.Elements[3] +
				v.Y * transform.Elements[8] +
				v.Z * transform.Elements[13] +
				v.W * transform.Elements[18] +
				transform.Elements[23];
			uComponent =
				v.X * transform.Elements[4] +
				v.Y * transform.Elements[9] +
				v.Z * transform.Elements[14] +
				v.W * transform.Elements[19] +
				transform.Elements[24];

			return new Vector4(x, y, z, w);
		}

		private bool TryProjectPrimitive(ref HyperLine hyperLine, Matrix<float> worldViewProjection)
		{
			Vector4 u = new Vector4();

			var pStart = ProjectVector(hyperLine.Start, worldViewProjection, out u.X);
			var pEnd = ProjectVector(hyperLine.End, worldViewProjection, out u.Y);

			const float epsilon = float.Epsilon;
			if (u.X < epsilon || u.Y < epsilon)
			{
				return false;
			}

			pStart /= u.X;
			pEnd /= u.Y;

			hyperLine = new HyperLine(pStart, pEnd, hyperLine.Color);
			return true;
		}

		public void Draw(HyperLine value, Matrix<float> worldViewProjection)
		{
			if (!drawMode)
				throw new InvalidOperationException("Begin must be called.");

			HyperLine triangle = value;
			if (TryProjectPrimitive(ref triangle, worldViewProjection))
			{
				float depth = (triangle.Start.W + triangle.End.W) / 2f;
				AddToDraw(triangle, depth);
			}
		}
	}
}
