using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public static class MatrixHelper
    {
        public static Matrix Create2DRotation(float angle)
        {
            Matrix r = new Matrix();
            float sin = (float)Math.Sin(angle);

            r.M11 = r.M22 = (float)Math.Cos(angle);
            r.M21 = sin;
            r.M12 = -sin;

            return r;
        }

        public static Matrix CreateRotationX(float angle)
        {
            Matrix r = Matrix.Identity;

            r.M22 = r.M33 = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            r.M23 = sin;
            r.M32 = -sin;

            return r;
        }

        public static Matrix CreateRotationY(float angle)
        {
            Matrix r = Matrix.Identity;

            r.M11 = r.M33 = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            r.M13 = -sin;
            r.M31 = sin;

            return r;
        }

        public static Matrix CreateRotationZ(float angle)
        {
            Matrix r = Matrix.Identity;

            r.M11 = r.M22 = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            r.M12 = sin;
            r.M21 = -sin;

            return r;
        }

        public static Matrix CreateScale(float x, float y, float z)
        {
            Matrix r = Matrix.Identity;

            r.M11 = x;
            r.M22 = y;
            r.M33 = z;

            return r;
        }

        public static Matrix CreateScale(float value)
        {
            return CreateScale(value, value, value);
        }

        public static Matrix CreateScale(Vector3 value)
        {
            return CreateScale(value.X, value.Y, value.Z);
        }

        public static Matrix CreateTranslation(float x, float y, float z)
        {
            Matrix r = Matrix.Identity;

            r.M41 = x;
            r.M42 = y;
            r.M43 = z;

            return r;
        }

        public static Matrix CreateTranslation(Vector3 value)
        {
            return CreateTranslation(value.X, value.Y, value.Z);
        }

        public static Matrix CreateLookAt(Vector3 cameraPosition,
            Vector3 cameraTarget, Vector3 cameraUp)
        {
            Matrix r = Matrix.Identity;

            var look = Vector3.Normalize(cameraPosition - cameraTarget);
            var right = Vector3.Normalize(Vector3.Cross(cameraUp, look));
            var up = Vector3.Cross(look, right);

            r.M11 = right.X;
            r.M12 = up.X;
            r.M13 = look.X;

            r.M21 = right.Y;
            r.M22 = up.Y;
            r.M23 = look.Y;

            r.M31 = right.Z;
            r.M32 = up.Z;
            r.M33 = look.Z;

            r.M41 = -Vector3.Dot(right, cameraPosition);
            r.M42 = -Vector3.Dot(up, cameraPosition);
            r.M43 = -Vector3.Dot(look, cameraPosition);

            return r;
        }

        public static Matrix CreateFromAxisAngle(Vector3 axis, float angle)
        {
            axis = Vector3.Normalize(axis);

            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;

            float xx = x * x;
            float xy = x * y;
            float xz = x * z;
            float yy = y * y;
            float yz = y * z;
            float zz = z * z;

            float cos = (float)Math.Cos(angle);
            float cosm = 1.0f - cos;
            float sin = (float)Math.Sin(angle);

            Matrix r = new Matrix();

            r.M11 = cos + cosm * xx;
            r.M12 = cosm * xy - sin * z;
            r.M13 = cosm * xz + sin * y;

            r.M21 = cosm * xy + sin * z;
            r.M22 = cos + cosm * yy;
            r.M23 = cosm * yz - sin * x;

            r.M31 = cosm * xz - sin * y;
            r.M32 = cosm * yz + sin * x;
            r.M33 = cos + cosm * zz;

            r.M44 = 1.0f;

            return r;
        }

        public static Matrix CreatePerspective(
            float width, float height, float nearPlaneDistance, float farPlaneDistance)
        {
            if (nearPlaneDistance <= 0)
                throw new ArgumentOutOfRangeException("nearPlaneDistance");
            if (farPlaneDistance <= 0)
                throw new ArgumentOutOfRangeException("farPlaneDistance");
            if (nearPlaneDistance >= farPlaneDistance)
                throw new ArgumentException("nearPlaneDistance must be less than farPlaneDistance.");

            Matrix r = new Matrix();

            r.M11 = (2f * nearPlaneDistance) / width;
            r.M22 = (2f * nearPlaneDistance) / height;
            r.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            r.M34 = -1f;
            r.M43 = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);

            return r;
        }

        public static Matrix CreatePerspectiveFieldOfView(
            float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            if (fieldOfView <= 0 || fieldOfView >= Math.PI)
                throw new ArgumentOutOfRangeException("fieldOfView");
            if (nearPlaneDistance <= 0)
                throw new ArgumentOutOfRangeException("nearPlaneDistance");
            if (farPlaneDistance <= 0)
                throw new ArgumentOutOfRangeException("farPlaneDistance");
            if (nearPlaneDistance >= farPlaneDistance)
                throw new ArgumentException("nearPlaneDistance must be less than farPlaneDistance.");

            Matrix r = new Matrix();

            // 1 / tan(FOV/2)
            float yScale = (float)(1.0 / Math.Tan(fieldOfView / 2.0));
            float xScale = yScale / aspectRatio;

            r.M11 = xScale;
            r.M22 = yScale;
            r.M33 = farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            r.M34 = -1f;
            r.M43 = (nearPlaneDistance * farPlaneDistance) / (nearPlaneDistance - farPlaneDistance);

            return r;
        }

        public static Matrix CreateOrthographic(
            float width, float height, float zNearPlane, float zFarPlane)
        {
            Matrix r = new Matrix();

            float special = 1.0f / (zNearPlane - zFarPlane);

            r.M11 = 2.0f / width;
            r.M22 = 2.0f / height;
            r.M33 = special;
            r.M43 = zNearPlane * special;
            r.M44 = 1.0f;

            return r;
        }
    }
}
