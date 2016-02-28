using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SoftwareGraphics
{
    public static class ModelBuilder
    {
        public static void TransformPolygons(Triangle[] polygons, ref Matrix transform)
        {
            if (polygons == null)
                throw new ArgumentNullException("polygons");
            if (transform == null)
                throw new ArgumentNullException("transform");

            for (int i = 0; i < polygons.Length; i++)
            {
                var triangle = polygons[i];
                triangle.A = Vector3.Transform(triangle.A, ref transform);
                triangle.B = Vector3.Transform(triangle.B, ref transform);
                triangle.C = Vector3.Transform(triangle.C, ref transform);
                polygons[i] = triangle;
            }
        }

        public static Triangle[] CreateParallelogram(
            Vector3 start, Vector3 side1, Vector3 side2, Color color)
        {
            Vector3 p1 = start;
            Vector3 p2 = start + side1;
            Vector3 p3 = start + side2;
            Vector3 p4 = p2 + side2;

            return new Triangle[]
            {
                new Triangle(p1, p2, p3, color),
                new Triangle(p2, p4, p3, color),
            };
        }

        public static Triangle[] CreatePlane(int xSegmenets, int ySegments, IEnumerator<Color> colorer)
        {
            Vector3 segmentSize = new Vector3(2f / xSegmenets, 2f / ySegments, 0f);
            Vector3 xSegment = new Vector3(segmentSize.X, 0, 0);
            Vector3 ySegment = new Vector3(0, segmentSize.Y, 0);

            Triangle[] triangles = new Triangle[xSegmenets * ySegments * 2];
            int index = 0;

            for (int i = 0; i < xSegmenets; i++)
            {
                for (int j = 0; j < ySegments; j++)
                {
                    Vector3 p = new Vector3(i * segmentSize.X, j * segmentSize.Y, 0);
                    p.X -= 1f;
                    p.Y -= 1f;

                    colorer.MoveNext();
                    triangles[index++] = new Triangle(
                        p, p + ySegment, p + segmentSize, colorer.Current);
                    triangles[index++] = new Triangle(
                        p, p + segmentSize, p + xSegment, colorer.Current);
                }
            }

            return triangles;
        }

        public static Triangle[] CreateCircle(
            Vector3 center, Vector3 radius, Vector3 normal, int pointCount, Color color)
        {
            if (pointCount < 3)
                throw new ArgumentOutOfRangeException("pointCount", "pointCount must be >= 3.");

            float angle = 2 * (float)Math.PI / pointCount;
            Matrix rotation = MatrixHelper.CreateFromAxisAngle(normal, angle);

            Triangle[] pieces = new Triangle[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                Vector3 newRadius = Vector3.Transform(radius, ref rotation);
                pieces[i] = new Triangle(center, center + radius, center + newRadius, color);
                radius = newRadius;
            }

            return pieces;
        }

        public static Triangle[] СделатьКуб(float сторона, Color цвет)
        {
            float полСтороны = сторона / 2;
            var грани = new[]
            {
                CreateParallelogram(  // left
                    new Vector3(-полСтороны, -полСтороны, полСтороны),
                    new Vector3(0, 0, -сторона),                    
                    new Vector3(0, сторона, 0), 
                    цвет),
                CreateParallelogram(  // right
                    new Vector3(полСтороны, -полСтороны, полСтороны),
                    new Vector3(0, сторона, 0), 
                    new Vector3(0, 0, -сторона),
                    цвет),
                CreateParallelogram(  // top
                    new Vector3(-полСтороны, полСтороны, полСтороны),
                    new Vector3(0, 0, -сторона),
                    new Vector3(сторона, 0, 0), 
                    цвет),
                CreateParallelogram(  // bottom
                    new Vector3(-полСтороны, -полСтороны, полСтороны),
                    new Vector3(сторона, 0, 0), 
                    new Vector3(0, 0, -сторона),
                    цвет),
                CreateParallelogram(  // front
                    new Vector3(-полСтороны, -полСтороны, полСтороны),
                    new Vector3(0, сторона, 0), 
                    new Vector3(сторона, 0, 0),
                    цвет),
                CreateParallelogram(  // back
                    new Vector3(-полСтороны, -полСтороны, -полСтороны),
                    new Vector3(сторона, 0, 0),
                    new Vector3(0, сторона, 0), 
                    цвет),
            };
            return грани.SelectMany(грань => грань).ToArray();
        }

        public static Triangle[] CreateParallelepiped(
            Vector3 start, Vector3 side1, Vector3 side2, Vector3 side3, Color color)
        {
            Vector3 start1 = start + side1;
            Vector3 start2 = start + side2;
            Vector3 start3 = start + side3;

            var sides = new[]
            {
                CreateParallelogram(start, side1, side2, color),
                CreateParallelogram(start, side3, side1, color),
                CreateParallelogram(start, side2, side3, color),
                CreateParallelogram(start1, side3, side2, color),
                CreateParallelogram(start2, side1, side3, color),
                CreateParallelogram(start3, side2, side1, color),
            };

            return sides.SelectMany(side => side).ToArray();
        }

        public static Triangle[] CreateCube(float sideLength, Color color)
        {
            if (sideLength < 0)
                throw new ArgumentOutOfRangeException("sideLength", "sideLength must be >= 0.");

            float halfSide = sideLength * 0.5f;
            return CreateParallelepiped(
                new Vector3(-halfSide, halfSide, halfSide),
                new Vector3(sideLength, 0, 0),
                new Vector3(0, -sideLength, 0),
                new Vector3(0, 0, -sideLength),
                color);
        }

        public static Triangle[] CreatePyramid(
            Vector3 highestPoint, float height, Vector3 baseSide1, Vector3 baseSide2, Color color)
        {
            Vector3 up = Vector3.Normalize(Vector3.Cross(baseSide1, baseSide2)) * height;
            Vector3 baseCenter = highestPoint - up;
            Vector3 baseStart = baseCenter - (baseSide1 + baseSide2) / 2;

            Triangle[] pyramidBase = CreateParallelogram(
                baseStart,
                baseSide1,
                baseSide2,
                color);

            Vector3 basePoint1 = baseStart + baseSide1;
            Vector3 basePoint2 = baseStart + baseSide2;
            Vector3 baseEnd = basePoint1 + baseSide2;

            Triangle[] pyramidSides = new[]
            {
                new Triangle(highestPoint, basePoint1, baseStart, color),
                new Triangle(highestPoint, baseEnd, basePoint1, color),
                new Triangle(highestPoint, basePoint2, baseEnd, color),
                new Triangle(highestPoint, baseStart, basePoint2, color),
            };

            return pyramidBase.Concat(pyramidSides).ToArray();
        }

        public static Triangle[] CreateSphere(
            float radius, int layerCount, int layerPoints, Color color)
        {
            return CreateSphere(radius, layerCount, layerPoints,
                ColorHelper.GetStatic(color).GetEnumerator());
        }

        public static Triangle[] CreateSphere(
            float radius, int layerCount, int layerPoints, IEnumerator<Color> colorer)
        {
            if (layerCount < 1)
                throw new ArgumentOutOfRangeException("layerCount", "layerCount must be >= 1.");
            if (layerPoints < 3)
                throw new ArgumentOutOfRangeException("layerPoints", "layerPoints must be >= 3.");
            if (colorer == null)
                throw new ArgumentNullException("colorer");
            
            List<Triangle> triangles = new List<Triangle>();
            Vector3[] layer = new Vector3[layerPoints];

            Matrix horizontalRotate = MatrixHelper.CreateFromAxisAngle(
                Vector3.Right, (float)Math.PI / (layerCount + 1));
            Matrix roundRotate = MatrixHelper.CreateFromAxisAngle(
                Vector3.Up, (float)(2 * Math.PI) / layerPoints);

            Vector3 top = new Vector3(0, radius, 0);
            Vector3 bottom = -top;

            colorer.MoveNext();
            
            // building a top of sphere
            layer[0] = Vector3.Transform(top, ref horizontalRotate);
            for (int i = 1; i < layerPoints; i++)
            {
                Vector3 lastPoint = layer[i - 1];
                Vector3 newPoint = Vector3.Transform(lastPoint, ref roundRotate);
                layer[i] = newPoint;

                triangles.Add(new Triangle(top, lastPoint, newPoint, colorer.Current));
            }
            triangles.Add(new Triangle(top, layer[layerPoints - 1], layer[0], colorer.Current));

            // building a middle (largest part) of sphere
            Vector3 currentDown = layer[0];
            for (int i = 0; i < layerCount - 1; i++)
            {
                colorer.MoveNext();

                Vector3 firstUp = layer[0];
                currentDown = Vector3.Transform(currentDown, ref horizontalRotate);
                Vector3 lastDown = Vector3.Zero;

                for (int j = 0; j < layerPoints - 1; j++)
                {
                    lastDown = currentDown;
                    currentDown = Vector3.Transform(currentDown, ref roundRotate);

                    Vector3 lastUp = layer[j];
                    Vector3 currentUp = layer[j + 1];

                    triangles.Add(new Triangle(currentUp, lastUp, currentDown, colorer.Current));
                    triangles.Add(new Triangle(currentDown, lastUp, lastDown, colorer.Current));

                    layer[j] = lastDown;
                }

                lastDown = currentDown;
                currentDown = Vector3.Transform(currentDown, ref roundRotate);
                
                triangles.Add(new Triangle(firstUp, layer[layerPoints - 1], currentDown, colorer.Current));
                triangles.Add(new Triangle(currentDown, layer[layerPoints - 1], lastDown, colorer.Current));

                layer[layerPoints - 1] = lastDown;
            }

            colorer.MoveNext();

            // building a botom of sphere
            for (int i = 0; i < layerPoints - 1; i++)
            {
                triangles.Add(new Triangle(bottom, layer[i + 1], layer[i], colorer.Current));
            }
            triangles.Add(new Triangle(bottom, layer[0], layer[layerPoints - 1], colorer.Current));

            return triangles.ToArray();
        }

        public static Triangle[] CreateCone(
            Vector3 radius, Vector3 baseNormal, Vector3 up, int pointCount, Color color)
        {
            return CreateCone(radius, baseNormal, up, pointCount,
                ColorHelper.GetStatic(color).GetEnumerator());
        }

        public static Triangle[] CreateCone(
            Vector3 radius, Vector3 baseNormal, Vector3 up, int pointCount, IEnumerator<Color> colorer)
        {
            if (pointCount < 3)
                throw new ArgumentOutOfRangeException("pointCount", "pointCount must be >= 3.");
            if (colorer == null)
                throw new ArgumentNullException("colorer");

            Matrix rotation = MatrixHelper.CreateFromAxisAngle(
                Vector3.Normalize(baseNormal), 2 * (float)Math.PI / pointCount);

            Vector3 last = radius;
            Vector3 current = Vector3.Transform(radius, ref rotation);

            List<Triangle> polygons = new List<Triangle>();

            for (int i = 0; i < pointCount; i++)
            {
                colorer.MoveNext();

                polygons.Add(new Triangle(last, current, up, colorer.Current));
                polygons.Add(new Triangle(current, last, Vector3.Zero, colorer.Current));
                
                last = current;
                current = Vector3.Transform(current, ref rotation);
            }

            return polygons.ToArray();
        }

        public static Triangle[] CreateIcosahedron(Color color)
        {
            float fi = 1.6180339887f;

            Vector3[] p = new Vector3[]
            {
                // XZ plane
                new Vector3(fi, 0, 1),
                new Vector3(-fi, 0, 1),
                new Vector3(-fi, 0, -1),
                new Vector3(fi, 0, -1),
                // YX plane
                new Vector3(1, fi, 0),
                new Vector3(1, -fi, 0),
                new Vector3(-1, -fi, 0),
                new Vector3(-1, fi, 0),
                // ZY plane
                new Vector3(0, 1, fi),
                new Vector3(0, 1, -fi),
                new Vector3(0, -1, -fi),
                new Vector3(0, -1, fi),
            };

            Triangle[] result = new Triangle[]
            {
                // strongly top
                new Triangle(p[1], p[2], p[7], color),
                new Triangle(p[2], p[9], p[7], color),
                new Triangle(p[9], p[4], p[7], color),
                new Triangle(p[3], p[4], p[9], color),
                new Triangle(p[3], p[0], p[4], color),
                new Triangle(p[0], p[8], p[4], color),
                new Triangle(p[8], p[7], p[4], color),
                new Triangle(p[8], p[1], p[7], color),
                // between top and bottom
                new Triangle(p[0], p[11], p[8], color),
                new Triangle(p[11], p[1], p[8], color),
                new Triangle(p[2], p[10], p[9], color),
                new Triangle(p[9], p[10], p[3], color),
                // strongly bottom
                new Triangle(p[2], p[1], p[6], color),
                new Triangle(p[1], p[11], p[6], color),
                new Triangle(p[11], p[5], p[6], color),
                new Triangle(p[11], p[0], p[5], color),
                new Triangle(p[0], p[3], p[5], color),
                new Triangle(p[3], p[10], p[5], color),
                new Triangle(p[10], p[6], p[5], color),
                new Triangle(p[10], p[2], p[6], color),
            };

            return result;
        }

        public static Triangle[] CreateGeosphere(int detalizationLevel, IEnumerator<Color> colorer)
        {
            if (colorer == null)
                throw new ArgumentNullException("colorer");

            colorer.MoveNext();
            var icosahedron = CreateIcosahedron(colorer.Current);

            return CreateGeosphere(icosahedron, detalizationLevel, colorer);
        }

        public static Triangle[] CreateGeosphere(
            Triangle[] baseModel, int detalizationLevel, IEnumerator<Color> colorer)
        {
            if (baseModel == null)
                throw new ArgumentNullException("baseModel");
            if (detalizationLevel < 0)
                throw new ArgumentOutOfRangeException("detalizationLevel");
            if (colorer == null)
                throw new ArgumentNullException("colorer");

            Triangle[] current = baseModel;
            Triangle[] last = null;

            // project icosahedron on sphere
            for (int i = 0; i < current.Length; i++)
            {
                Triangle triangle = current[i];
                current[i] = new Triangle(
                    Vector3.Normalize(triangle.A),
                    Vector3.Normalize(triangle.B),
                    Vector3.Normalize(triangle.C),
                    triangle.Color);
            }

            for (int k = 0; k < detalizationLevel; k++)
            {
                last = current;
                current = new Triangle[last.Length * 4];
                
                colorer.MoveNext();

                // divide each triangle into 4 triangles
                // and project them on sphere
                int j = 0;
                for (int i = 0; i < last.Length; i++)
                {
                    Triangle triangle = last[i];
                    
                    var a = triangle.A;
                    var b = triangle.B;
                    var c = triangle.C;
                    var ab = Vector3.Normalize(a + b);
                    var ac = Vector3.Normalize(a + c);
                    var bc = Vector3.Normalize(b + c);

                    current[j++] = new Triangle(ab, bc, ac, triangle.Color);
                    current[j++] = new Triangle(a, ab, ac, colorer.Current);
                    current[j++] = new Triangle(b, bc, ab, colorer.Current);
                    current[j++] = new Triangle(c, ac, bc, colorer.Current);
                }
            }

            return current;
        }
    }
}
