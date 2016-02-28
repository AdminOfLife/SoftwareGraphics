using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

using Vector3Int = GenericMathematics.Vector3<int>;

namespace LifeGameExample
{
    public sealed class LifeCursor : ModelObject
    {
        Life3D life;
        Vector3Int location;

        public Vector3Int LocationOnField
        {
            get { return location; }
            set
            {
                if (value.X < 0 || value.X >= life.FieldWidth ||
                    value.Y < 0 || value.Y >= life.FieldHeight ||
                    value.Z < 0 || value.Z >= life.FieldDepth)
                {
                    throw new ArgumentOutOfRangeException("value", "value must be located in field.");
                }

                location = value;
                World = MatrixHelper.CreateTranslation(
                    -1f + (location.X + 0.5f) * life.CellSize.X,
                    1f - (location.Y + 0.5f) * life.CellSize.Y,
                    1f - (location.Z +0.5f) * life.CellSize.Z);
            }
        }

        public LifeCursor(Life3D lifeField)
        {
            life = lifeField;

            var cube = ModelBuilder.СделатьКуб(1f, Color.FromArgb(175, Color.Magenta));
            Matrix scale = MatrixHelper.CreateScale(life.CellSize);
            ModelBuilder.TransformPolygons(cube, ref scale);

            var colorer = ColorHelper.GetRainbow(Color.Red, 20f).GetEnumerator();
            for (int i = 0; i < 12; i += 2)
            {
                colorer.MoveNext();
                cube[i].Color = colorer.Current;
                cube[i + 1].Color = colorer.Current;
            }

            polygons.AddRange(cube);

            LocationOnField = new Vector3Int(0, 0, 0);
        }

        public void Poke(bool? newCellState)
        {
            life.PokeAt(LocationOnField.X, LocationOnField.Y, LocationOnField.Z, newCellState);
        }

        public override void Draw(Scene scene)
        {
            scene.Device.Draw(polygons, 
                this.World * life.World, scene.View, scene.Projection);
        }
    }
}
