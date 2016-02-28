using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public sealed class Camera
    {
        public Vector3 Position = Vector3.Zero;

        public Vector3 Look = Vector3.Forward;
        public Vector3 Up = Vector3.Up;
        public Vector3 Right = Vector3.Right;

        public Matrix View = Matrix.Identity;

        public Camera(Vector3 position)
        {
            Position = position;
            UpdateView();
        }

        private void UpdateView()
        {
            Look = Vector3.Normalize(Look);
            Up = Vector3.Normalize(Up);
            Right = Vector3.Normalize(Right);

            Vector3 target = Position + Look;
            View = MatrixHelper.CreateLookAt(Position, target, Up);
        }

        public void Rotate(float x, float y, float z)
        {
            Matrix rotation;

            rotation = MatrixHelper.CreateFromAxisAngle(Vector3.Up, y);
            Look = Vector3.Transform(Look, ref rotation);
            Right = Vector3.Transform(Right, ref rotation);
            Up = Vector3.Transform(Up, ref rotation);

            rotation = MatrixHelper.CreateFromAxisAngle(Right, x);
            Up = Vector3.Transform(Up, ref rotation);
            Look = Vector3.Transform(Look, ref rotation);

            Matrix roll = MatrixHelper.CreateFromAxisAngle(Look, z);
            Up = Vector3.Transform(Up, ref rotation);
            Right = Vector3.Transform(Right, ref rotation);

            UpdateView();
        }

        public void RotateInXZAround(Vector3 target, float angle)
        {
            Matrix rotation = MatrixHelper.CreateFromAxisAngle(Vector3.Up, angle);

            Vector3 diff = Position - target;
            Vector3 newDiff = Vector3.Transform(diff, ref rotation);
            Position = target + newDiff;

            Look = Vector3.Transform(Look, ref rotation);
            Right = Vector3.Transform(Right, ref rotation);
            Up = Vector3.Transform(Up, ref rotation);

            UpdateView();
        }

        public void RotateInYZAround(Vector3 target, float angle)
        {
            Matrix rotation = MatrixHelper.CreateFromAxisAngle(Right, angle);

            Vector3 diff = Position - target;
            Vector3 newDiff = Vector3.Transform(diff, ref rotation);
            Position = target + newDiff;
            
            Look = Vector3.Transform(Look, ref rotation);
            Right = Vector3.Transform(Right, ref rotation);
            Up = Vector3.Transform(Up, ref rotation);

            UpdateView();
        }

        public void MoveTo(Vector3 position)
        {
            this.Position = position;
            UpdateView();
        }

        public void MoveBy(Vector3 value)
        {
            Position += value;
            UpdateView();
        }

        public void RelativeMoveBy(Vector3 value)
        {
            Position += Look * -value.Z;
            Position += Up * value.Y;
            Position += Right * value.X;
            UpdateView();
        }

        public void Zoom(float value)
        {
            Position += Look * value;
            UpdateView();
        }
    }
}
