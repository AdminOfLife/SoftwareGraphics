using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace LifeGameExample
{
	public sealed class Life3D : ModelObject
	{
		// if cell is empty
		static readonly bool[] birthRules = new[]
		{
			/*  0 */ false,
			/*  1 */ false,
			/*  2 */ false,
			/*  3 */ true,
			/*  4 */ true,
			/*  5 */ false,
			/*  6 */ false,
			/*  7 */ false,
			/*  8 */ false,
			/*  9 */ false,
			/* 10 */ false,
			/* 11 */ false,
			/* 12 */ false,
			/* 13 */ false,
			/* 14 */ false,
			/* 15 */ false,
			/* 16 */ false,
			/* 17 */ false,
			/* 18 */ false,
			/* 19 */ false,
			/* 20 */ false,
			/* 21 */ false,
			/* 22 */ false,
			/* 23 */ false,
			/* 24 */ false,
			/* 25 */ false,
			/* 26 */ false,
		};
		
		// if cell is not empty
		static readonly bool[] aliveRules = new[]
		{
			/*  0 */ false,
			/*  1 */ false,
			/*  2 */ true,
			/*  3 */ true,
			/*  4 */ false,
			/*  5 */ false,
			/*  6 */ false,
			/*  7 */ false,
			/*  8 */ false,
			/*  9 */ false,
			/* 10 */ false,
			/* 11 */ false,
			/* 12 */ false,
			/* 13 */ false,
			/* 14 */ false,
			/* 15 */ false,
			/* 16 */ false,
			/* 17 */ false,
			/* 18 */ false,
			/* 19 */ false,
			/* 20 */ false,
			/* 21 */ false,
			/* 22 */ false,
			/* 23 */ false,
			/* 24 */ false,
			/* 25 */ false,
			/* 26 */ false,
		};
		
		bool[,,] field;
		Color cellColor;
		
		public int FieldWidth { get; private set; }
		public int FieldHeight { get; private set; }
		public int FieldDepth { get; private set; }

        public Vector3 CellSize { get; private set; }
		
		public Life3D(int fieldWidth, int fieldHeight, int fieldDepth, Color cellColor)
		{
			if (fieldWidth < 1)
				throw new ArgumentOutOfRangeException("fieldWidth", "fieldWidth must be >= 1.");
			if (fieldHeight < 1)
				throw new ArgumentOutOfRangeException("fieldHeight", "fieldHeight must be >= 1.");
			if (fieldDepth < 1)
				throw new ArgumentOutOfRangeException("fieldDepth", "fieldDepth must be >= 1.");
			
			FieldWidth = fieldWidth;
			FieldHeight = fieldHeight;
			FieldDepth = fieldDepth;

            CellSize = new Vector3(
                2f / FieldWidth,
                2f / FieldHeight,
                2f / FieldDepth);
			
			field = new bool[fieldWidth, fieldHeight, fieldDepth];
			this.cellColor = cellColor;
		}
		
		public void GenerateView()
		{
			polygons.Clear();

            //Triangle[] cell = ModelBuilder.CreateSphere(
            //    0.5f, 3, 5, ColorHelper.GetRainbow(cellColor, 10f).GetEnumerator());
            Triangle[] cell = ModelBuilder.CreateParallelepiped(
                new Vector3(-0.5f, 0.5f, 0.5f),
                new Vector3(1, 0, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, 0, -1),
                cellColor);
            var colorer = ColorHelper.GetRainbow(cellColor, 10f).GetEnumerator();
            for (int i = 0; i < 12; i += 2)
            {
                colorer.MoveNext();
                cell[i].Color = colorer.Current;
                cell[i + 1].Color = colorer.Current;
            }
            Triangle[] cellCopy = new Triangle[cell.Length];
			
			for (int i = 0; i < FieldWidth; i++)
			{
				for (int j = 0; j < FieldHeight; j++)
				{
					for (int k = 0; k < FieldDepth; k++)
					{
						if (field[i, j, k])
						{
							float left = -1f + CellSize.X * i;
							float top = 1f - CellSize.Y * j;
							float depth = 1f - CellSize.Z * k;
	                        
	                        //polygons.AddRange(ModelBuilder.CreateParallelepiped(
	                        //    new Vector3(left, top, 0),
	                        //    new Vector3(cellWidth, 0, 0),
	                        //    new Vector3(0, -cellHeight, 0),
	                        //    new Vector3(0, 0, -20f),
	                        //    cellColor));
	
	                        Array.Copy(cell, cellCopy, cell.Length);
                            Matrix cellTransform =
                                MatrixHelper.CreateScale(CellSize) *
	                            MatrixHelper.CreateTranslation(
	                                left + CellSize.X / 2f,
	                                top - CellSize.Y / 2f,
	                                depth - CellSize.Z / 2f);

	                        ModelBuilder.TransformPolygons(cellCopy, ref cellTransform);
	                        polygons.AddRange(cellCopy);
						}
					}
				}
			}
		}
		
		public void PokeAt(int left, int top, int depth, bool? newCellState)
		{
			if (left < 0 || left >= FieldWidth)
				throw new ArgumentOutOfRangeException("left");
			if (top < 0 || top >= FieldHeight)
				throw new ArgumentOutOfRangeException("top");
			if (depth < 0 || depth >= FieldDepth)
				throw new ArgumentOutOfRangeException("depth");

            bool newState = newCellState ?? !field[left, top, depth];
            field[left, top, depth] = newState;
		}
		
		public void Clear()
		{
			Array.Clear(field, 0, field.Length);
		}
		
		public void NextGeneration()
		{
			bool[,,] newGeneration = new bool[FieldWidth, FieldHeight, FieldDepth];
			
			for (int i = 0; i < FieldWidth; i++)
			{
				for (int j = 0; j < FieldHeight; j++)
				{
					for (int k = 0; k < FieldDepth; k++)
					{
						int neighbourCount = 0;
						
						int left = i == 0 ? 0 : -1;
						int top = j == 0 ? 0 : -1;
						int near = k == 0 ? 0 : -1;
						
						int right = i == FieldWidth - 1 ? 0 : 1;
						int bottom = j == FieldHeight - 1 ? 0 : 1;
						int far = k == FieldDepth - 1 ? 0 : 1;
						
						for (int m = left; m <= right; m++)
						{
							for (int n = top; n <= bottom; n++)
							{
								for (int p = near; p <= far; p++)
								{
									if (m == 0 && n == 0 && p == 0)
										continue;
									
									if (field[i + m, j + n, k + p])
										neighbourCount++;
								}
							}
						}
						
						if (field[i, j, k])
							newGeneration[i, j, k] = aliveRules[neighbourCount];
						else
							newGeneration[i, j, k] = birthRules[neighbourCount];
					}
				}
			}
			
			field = newGeneration;
		}
	}
}
