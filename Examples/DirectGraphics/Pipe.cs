using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace DirectGraphics
{
    public sealed class Pipe : ModelObject
    {
        int partPoints;
        IEnumerator<Color> colorer;

        Vector3[] lastPart;

        public Pipe(int partPoints, IEnumerator<Color> colorer)
        {
            if (partPoints < 3)
                throw new ArgumentOutOfRangeException("partPoints", "partPoints must be >= 3.");
            if (colorer == null)
                throw new ArgumentNullException("colorer");

            this.partPoints = partPoints;
            this.colorer = colorer;
        }

        public void AddPart(Vector3 position, Vector3 normal, Vector3 radius)
        {
            Vector3[] part = new Vector3[partPoints];
            Matrix rotation = MatrixHelper.CreateFromAxisAngle(
                normal, 2 * (float)Math.PI / partPoints);

            part[0] = radius;
            for (int i = 1; i < part.Length; i++)
            {
                part[i] = Vector3.Transform(part[i - 1], ref rotation);
            }

            for (int i = 0; i < part.Length; i++)
			{
                part[i] += position;
			}

            if (lastPart != null)
            {
                colorer.MoveNext();

                for (int i = 1; i < lastPart.Length; i++)
                {
                    polygons.Add(new Triangle(
                        lastPart[i - 1], lastPart[i], part[i - 1], colorer.Current));
                    polygons.Add(new Triangle(
                        lastPart[i], part[i], part[i - 1], colorer.Current));
                }

                polygons.Add(new Triangle(
                    lastPart[lastPart.Length - 1],
                    lastPart[0],
                    part[part.Length - 1],
                    colorer.Current));
                polygons.Add(new Triangle(
                    lastPart[0],
                    part[0],
                    part[part.Length - 1],
                    colorer.Current));
            }

            lastPart = part;
        }
    }
}
