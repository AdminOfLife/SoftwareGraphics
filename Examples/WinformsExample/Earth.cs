using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace WinformsExample
{
    public class Earth : ModelObject
    {
        int mapWidth;
        int mapHeight;

        float[,] heights;
        Bitmap colors;

        float sizeUnit;
        float heightUnit;

        public Earth(Bitmap heightMap, Bitmap colorMap, float sizeUnit, float heightUnit)
        {
            if (heightMap == null)
                throw new ArgumentNullException("heightMap");
            if (colorMap == null)
                throw new ArgumentNullException("colorMap");
            if (colorMap.Width != heightMap.Width + 1)
                throw new ArgumentException("colorMap.Width must be heightMap.Width + 1.", "colorMap");
            if (colorMap.Height != heightMap.Height + 1)
                throw new ArgumentException("colorMap.Height must be heightMap.Height + 1.", "colorMap");
            if (sizeUnit <= 0)
                throw new ArgumentOutOfRangeException("sizeUnit");
            if (heightUnit <= 0)
                throw new ArgumentOutOfRangeException("heightUnit");

            this.sizeUnit = sizeUnit;
            this.heightUnit = heightUnit;

            colors = (Bitmap)colorMap.Clone();

            LoadMap(heightMap);
            BuildMap();
        }

        private void LoadMap(Bitmap heightMap)
        {
            mapWidth = heightMap.Width + 2;
            mapHeight = heightMap.Height + 2;

            heights = new float[mapWidth, mapHeight];

            for (int i = 0; i < heightMap.Width; i++)
            {
                for (int j = 0; j < heightMap.Height; j++)
                {
                    Color heightPixel = heightMap.GetPixel(i, j);
                    heights[i + 1, j + 1] = heightPixel.B * heightUnit;
                }
            }
        }

        private void BuildMap()
        {
            for (int i = 0; i < mapWidth - 1; i++)
            {
                for (int j = 0; j < mapHeight - 1; j++)
                {
                    Vector3 upLeft = new Vector3(i * sizeUnit, heights[i, j], j * sizeUnit);
                    Vector3 upRight = new Vector3(upLeft.X + sizeUnit, heights[i + 1, j], upLeft.Z);
                    Vector3 downLeft = new Vector3(upLeft.X, heights[i, j + 1], upLeft.Z + sizeUnit);
                    Vector3 downRight = new Vector3(upRight.X, heights[i + 1, j + 1], downLeft.Z);

                    Color color = colors.GetPixel(i, j);
                    polygons.Add(new Triangle(upLeft, upRight, downLeft, color));
                    polygons.Add(new Triangle(downLeft, upRight, downRight, color));
                }
            }

            Matrix translation = MatrixHelper.CreateTranslation(
                -((mapWidth - 1) / 2) * sizeUnit, 0, -((mapHeight - 1) / 2) * sizeUnit);

            Triangle[] triangles = polygons.ToArray();
            ModelBuilder.TransformPolygons(triangles, ref translation);
            polygons = triangles.ToList();
        }
    }
}
