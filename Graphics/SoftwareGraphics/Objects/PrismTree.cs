using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace SoftwareGraphics.Objects
{
    public class PrismTree : ModelObject
    {
        private struct Prism
        {
            public Vector3 Position;
            public Vector3 Base;
            public Vector3 Depth;
            public Vector3 Apex;
        }

        const float ColorHueStep = 4f;
        static readonly Color StartColor = Color.Yellow;

        Prism start;

        Color currentColor;
        List<Prism> currentLayer = new List<Prism>();

        public int LayerCount { get; private set; }

        public PrismTree(Vector3 baseSide, Vector3 depthSide, Vector3 height)
        {
            currentColor = StartColor;

            start = new Prism
            {
                Position = -baseSide / 2,
                Base = baseSide,
                Depth = depthSide,
                Apex = baseSide / 2 + height,
            };

            Vector3 down = Vector3.Cross(baseSide, Vector3.Normalize(depthSide));

            currentLayer.Add(start);
            polygons.AddRange(ModelBuilder.CreateParallelepiped(
                start.Position, baseSide, down, depthSide, currentColor));

            LayerCount = 1;
        }

        public void GenerateNextLayer()
        {
            float hue = currentColor.GetHue() + ColorHueStep;
            if (hue > 360f)
                hue = 0;

            currentColor = ColorHelper.FromAhsb(
                currentColor.A,
                hue,
                currentColor.GetSaturation(),
                currentColor.GetBrightness());

            List<Prism> newLayer = new List<Prism>(currentLayer.Count);

            foreach (Prism prism in currentLayer)
            {
                Vector3 oldUp = Vector3.Cross(
                    Vector3.Normalize(prism.Base), Vector3.Normalize(prism.Depth));
                Vector3 newUp = Vector3.Cross(
                    Vector3.Normalize(prism.Apex), Vector3.Normalize(prism.Depth));

                Matrix oldBasis = new Matrix(
                    prism.Base.X,  prism.Base.Y,  prism.Base.Z,  0,
                    prism.Depth.X, prism.Depth.Y, prism.Depth.Z, 0,
                    oldUp.X,       oldUp.Y,       oldUp.Z,       0,
                    0,             0,             0,             1);

                oldBasis = Matrix.Invert(oldBasis);

                Vector3 oldApexInOldBasis = Vector3.TransformNormal(prism.Apex, ref oldBasis);
                Vector3 newUpInOldBasis = Vector3.TransformNormal(newUp, ref oldBasis);

                Matrix transformation = new Matrix(
                    0f,  oldApexInOldBasis.X,  newUpInOldBasis.X, 0,
                    -1f, oldApexInOldBasis.Y,  newUpInOldBasis.Y, 0,
                    0f,  oldApexInOldBasis.Z,  newUpInOldBasis.Z, 0,
                    0,   0,                    0,                 1);

                transformation = Matrix.Transpose(transformation);
                transformation = Matrix.Invert(transformation);

                Matrix returnBasis = new Matrix(
                    -prism.Depth.X, prism.Apex.X, newUp.X, 0,
                    -prism.Depth.Y, prism.Apex.Y, newUp.Y, 0,
                    -prism.Depth.Z, prism.Apex.Z, newUp.Z, 0,
                    0,              0,            0,       1);

                returnBasis = Matrix.Transpose(returnBasis);
                Matrix resultTransform = oldBasis * transformation * returnBasis;

                Prism newPrism = new Prism
                {
                    Position = prism.Position + newUp * prism.Apex.Length,
                    Base = Vector3.TransformNormal(prism.Base, ref resultTransform),
                    Depth = Vector3.TransformNormal(prism.Depth, ref resultTransform),
                    Apex = Vector3.TransformNormal(prism.Apex, ref resultTransform),
                };

                newLayer.Add(newPrism);
                polygons.AddRange(ModelBuilder.CreateParallelepiped(
                    newPrism.Position,
                    newPrism.Apex,
                    prism.Position - newPrism.Position,
                    newPrism.Depth,
                    currentColor));
            }

            currentLayer = newLayer;
            LayerCount++;
        }
    }
}
