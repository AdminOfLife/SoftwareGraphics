using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Objects
{
    public class BaseObject
    {
        public Vector3 Position
        {
            get { return new Vector3(World.M41, World.M42, World.M43); }
            set
            {
                World.M41 = value.X;
                World.M42 = value.Y;
                World.M43 = value.Z;
            }
        }

        public Matrix World = Matrix.Identity;

        public Vector3 Right = Vector3.Right;
        public Vector3 Up = Vector3.Up;
        public Vector3 Look = Vector3.Forward;        

        public BaseObject()
        {
        }

        public void Rotate(float x, float y, float z)
        {
            RotateAround(this.Position, x, y, z);
        }

        public void RotateAround(Vector3 point, float x, float y, float z)
        {
            Position -= point;

            Matrix yaw = MatrixHelper.CreateFromAxisAngle(Vector3.UnitY, y);
            Look = Vector3.Transform(Look, yaw);
            Right = Vector3.Transform(Right, yaw);
            Up = Vector3.Transform(Up, yaw);

            Matrix pitch = MatrixHelper.CreateFromAxisAngle(Vector3.UnitX, x);
            Up = Vector3.Transform(Up, pitch);
            Look = Vector3.Transform(Look, pitch);
            Right = Vector3.Transform(Right, pitch);

            Matrix roll = MatrixHelper.CreateFromAxisAngle(Vector3.UnitZ, z);
            Up = Vector3.Transform(Up, roll);
            Right = Vector3.Transform(Right, roll);
            Look = Vector3.Transform(Look, roll);

            World = World * yaw * pitch * roll;

            Position += point;
        }

        public void RelativeRotate(float x, float y, float z)
        {
            Vector3 translation = Position;
            Position = Vector3.Zero;

            Matrix yaw = MatrixHelper.CreateFromAxisAngle(Up, y);
            Look = Vector3.Transform(Look, yaw);
            Right = Vector3.Transform(Right, yaw);

            Matrix pitch = MatrixHelper.CreateFromAxisAngle(Right, x);
            Look = Vector3.Transform(Look, pitch);
            Up = Vector3.Transform(Up, pitch);

            Matrix roll = MatrixHelper.CreateFromAxisAngle(Look, z);
            Right = Vector3.Transform(Right, roll);
            Up = Vector3.Transform(Up, roll);

            World = World * yaw * pitch * roll;

            Look = Vector3.Normalize(Look);
            Up = Vector3.Normalize(Up);
            Right = Vector3.Normalize(Right);

            Position = translation;
        }

        public void MoveBy(Vector3 value)
        {
            Vector3 position = Position;
            position += value;
            
            Position = position;
        }

        public void RelativeMoveBy(Vector3 value)
        {
            Vector3 position = Position;
            position += Look * -value.Z;
            position += Up * value.Y;
            position += Right * value.X;

            Position = position;
        }
    }
}
