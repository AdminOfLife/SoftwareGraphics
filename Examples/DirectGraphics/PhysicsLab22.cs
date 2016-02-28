using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace DirectGraphics
{
    public sealed class PhysicsLab22 : ModelObject
    {
        const float StepX = 5f;
        const float StepZ = 1f;
        const float PartRadius = 0.1f;

        List<Pipe> series;
        List<Pipe> cuttings;

        List<Pipe> currentPipes;

        public PhysicsLab22()
        {
            double[][] data = new double[][]
            {
                /*       0           20          40          60          80         100         120         140           160  */
                new[] { 2.40, 2.45, 2.45, 2.50, 2.50, 2.55, 2.60, 2.70, 2.75, 2.80, 2.90, 3.15, 3.20, 2.80, 1.00, -0.50, -0.75 },
                new[] { 2.40, 2.45, 2.45, 2.45, 2.45, 2.45, 2.40, 2.40, 2.35, 2.30, 2.25, 2.10, 1.90, 1.50, 1.00, 0.70, 0.25 },
                new[] { 2.40, 2.45, 2.45, 2.45, 2.45, 2.40, 2.35, 2.35, 2.30, 2.20, 2.05, 1.90, 1.70, 1.35, 1.00, 0.80, 0.50 },
                new[] { 2.40, 2.45, 2.45, 2.45, 2.45, 2.45, 2.40, 2.40, 2.35, 2.30, 2.25, 2.10, 1.90, 1.50, 1.00, 0.70, 0.25 },
                new[] { 2.40, 2.45, 2.45, 2.45, 2.50, 2.55, 2.60, 2.70, 2.75, 2.80, 2.90, 3.15, 3.20, 2.75, 1.00, -0.60, -0.75 },
            };

            double[] zCoords = new[] { 4.8, 7.3, 9.8, 11.3, 13.8 };

            Color[] colors = new[]
            {
                Color.Red, Color.LightGreen, Color.DarkBlue, Color.Magenta, Color.Cyan
            };

            series = BuildPipes(data, zCoords, ColorHelper.GetFixedColors(colors).GetEnumerator());

            double[][] data1 = new double[data[0].Length][];
            for (int i = 0; i < data1.Length; i++)
            {
                data1[i] = new double[data.Length];

                for (int j = 0; j < data.Length; j++)
                {
                    data1[i][j] = data[j][i];
                }
            }

            double[] zCoords1 = GenericMathematics.Math<double>.Default
                .EnumerateFrom(0)
                .Select(d => d * StepZ)
                .Take(data1.Length)
                .ToArray();

            cuttings = BuildPipes(data1, zCoords1, ColorHelper.GetFixedColors(colors).GetEnumerator());

            foreach (Pipe pipe in series)
            {
                pipe.World *= MatrixHelper.CreateTranslation(
                    -(float)data[0].Length * StepX / 2, 0, (float)(zCoords.First() + zCoords.Last()) / 2);

                pipe.World *= MatrixHelper.CreateScale(4.0f, 40.0f, 20.0f);
            }

            foreach (Pipe pipe in cuttings)
            {
                pipe.World *= MatrixHelper.CreateTranslation(
                    -(float)data1[0].Length * StepX / 2 + 2.5f, 0, (float)(zCoords1.Last()) / 2);

                pipe.World *= MatrixHelper.CreateScale(9.0f, 40.0f, 18.0f);

                pipe.World *= MatrixHelper.CreateRotationY((float)-Math.PI / 2);
            }

            World *= MatrixHelper.CreateScale(1.5f);

            currentPipes = series;
        }

        private List<Pipe> BuildPipes(double[][] data, double[] zCoords, IEnumerator<Color> colorer)
        {
            List<Pipe> pipes = new List<Pipe>();

            for (int i = 0; i < data.Length; i++)
            {
                colorer.MoveNext();

                Pipe pipe = new Pipe(16, ColorHelper.GetStatic(colorer.Current).GetEnumerator());
                double[] pipeData = data[i];

                float z = (float)zCoords[i];

                Vector3 lastPoint = new Vector3(0, (float)pipeData[0], -z);
                pipe.AddPart(lastPoint, Vector3.UnitX, new Vector3(0, 0, PartRadius));

                for (int j = 1; j < pipeData.Length; j++)
                {
                    float value = (float)pipeData[j];

                    Vector3 point = new Vector3(j * StepX, value, -z);
                    Vector3 normal = Vector3.Normalize(point - lastPoint);

                    pipe.AddPart(point, normal, new Vector3(0, 0, PartRadius));
                }

                pipes.Add(pipe);
            }

            return pipes;
        }

        public void SwitchSeries()
        {
            if (currentPipes == series)
                currentPipes = cuttings;
            else
                currentPipes = series;
        }

        public override void Draw(Scene scene)
        {
            foreach (Pipe pipe in currentPipes)
            {
                pipe.Draw(scene, World);
            }
        }

        public override void Draw(Scene scene, Matrix parentWorld)
        {
            var matrix = World * parentWorld;

            foreach (Pipe pipe in currentPipes)
            {
                pipe.Draw(scene, matrix);
            }
        }
    }
}
