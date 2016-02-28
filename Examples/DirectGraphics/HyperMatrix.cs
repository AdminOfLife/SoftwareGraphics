using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GenericMathematics;

using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace DirectGraphics
{
	public static class HyperMatrix
	{
		public static Matrix<float> CreateIdentity()
		{
			return Matrix<float>.Identity(5);
		}

		public static Matrix<float> CreateTranslation(Vector4 value)
		{
			var m = CreateIdentity();

			m[4, 0] = value.X;
			m[4, 1] = value.Y;
			m[4, 2] = value.Z;
			m[4, 3] = value.W;

			return m;
		}

		private static Matrix<float> CreateRotationAt(int row1, int row2, float angle)
		{
			var m = CreateIdentity();

			float sin = (float)Math.Sin(angle);
			float cos = (float)Math.Cos(angle);

			m[row1, row1] = m[row2, row2] = cos;
			m[row1, row2] = sin;
			m[row2, row1] = -sin;

			return m;
		}

		public static Matrix<float> CreateRotationXY(float angle)
		{
			return CreateRotationAt(2, 3, angle);
		}

		public static Matrix<float> CreateRotationXZ(float angle)
		{
			return CreateRotationAt(1, 3, angle);
		}

		public static Matrix<float> CreateRotationXW(float angle)
		{
			return CreateRotationAt(1, 2, angle);
		}

		public static Matrix<float> CreateRotationYZ(float angle)
		{
			return CreateRotationAt(0, 3, angle);
		}

		public static Matrix<float> CreateRotationYW(float angle)
		{
			return CreateRotationAt(0, 2, angle);
		}

		public static Matrix<float> CreateRotationZW(float angle)
		{
			return CreateRotationAt(0, 2, angle);
		}

		public static Matrix<float> CreateScale(Vector4 value)
		{
			var m = new Matrix<float>(5);

			m[0, 0] = value.X;
			m[1, 1] = value.Y;
			m[2, 2] = value.Z;
			m[3, 3] = value.W;
			m[4, 4] = 1f;

			return m;
		}

		public static Matrix<float> CreateOrthographic()
		{
			var m = CreateIdentity();

			m[3, 3] = 0;

			return m;
		}

		public static Matrix<float> CreatePerspectiveFieldOfView(
			float fieldOfView, float nearPlaneDistance, float farPlaneDistance)
		{
			if (fieldOfView <= 0 || fieldOfView >= Math.PI)
				throw new ArgumentOutOfRangeException("fieldOfView");
			if (nearPlaneDistance <= 0)
				throw new ArgumentOutOfRangeException("nearPlaneDistance");
			if (farPlaneDistance <= 0)
				throw new ArgumentOutOfRangeException("farPlaneDistance");
			if (nearPlaneDistance >= farPlaneDistance)
				throw new ArgumentException("nearPlaneDistance must be less than farPlaneDistance.");

			var r = new Matrix<float>(5);

			// 1 / tan(FOV/2)
			float scale = (float)(1.0 / Math.Tan(fieldOfView / 2.0));

			r[0, 0] = scale;
			r[1, 1] = scale;
			r[2, 2] = scale;
			r[3, 3] = farPlaneDistance / (farPlaneDistance - nearPlaneDistance);
			r[3, 4] = 1f;
			r[4, 3] = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);

			return r;
		}
	}
}
