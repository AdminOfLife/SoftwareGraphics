using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using SoftwareGraphics;
using SoftwareGraphics.Objects;

namespace LifeGameExample
{
	public sealed class Life : ModelObject
	{
		// if cell is empty
		static readonly bool[] birthRules = new[]
		{
			/* 0 */ false,
			/* 1 */ false,
			/* 2 */ false,
			/* 3 */ true,
			/* 4 */ false,
			/* 5 */ false,
			/* 6 */ false,
			/* 7 */ false,
			/* 8 */ false,
		};
		
		// if cell is not empty
		static readonly bool[] aliveRules = new[]
		{
			/* 0 */ false,
			/* 1 */ false,
			/* 2 */ true,
			/* 3 */ true,
			/* 4 */ false,
			/* 5 */ false,
			/* 6 */ false,
			/* 7 */ false,
			/* 8 */ false,
		};
		
		bool[,] field;
		Color cellColor;
		
		public int FieldWidth { get; private set; }
		public int FieldHeight { get; private set; }
		
		public Life(int fieldWidth, int fieldHeight, Color cellColor)
		{
			if (fieldWidth < 1)
				throw new ArgumentOutOfRangeException("fieldWidth", "fieldWidth must be >= 1.");
			if (fieldHeight < 1)
				throw new ArgumentOutOfRangeException("fieldHeight", "fieldHeight must be >= 1.");
			
			FieldWidth = fieldWidth;
			FieldHeight = fieldHeight;
			
			field = new bool[fieldWidth, fieldHeight];
			this.cellColor = cellColor;
		}
		
		public void GenerateView()
		{
			polygons.Clear();
			
			float cellWidth = 2f / FieldWidth;
			float cellHeight = 2f / FieldHeight;

            /*Triangle[] cell = ModelBuilder.CreateSphere(
                0.5f, 5, 10, ColorHelper.GetRainbow(cellColor, 10f).GetEnumerator());
            Triangle[] cellCopy = new Triangle[cell.Length];*/
			
			for (int i = 0; i < FieldWidth; i++)
			{
				for (int j = 0; j < FieldHeight; j++)
				{
					if (field[i, j])
					{
						float left = -1f + cellWidth * i;
						float top = 1f - cellHeight * j;

                        polygons.Add(new Triangle(
                            new Vector3(left, top, 0),
                            new Vector3(left + cellWidth, top - cellHeight, 0),
                            new Vector3(left, top - cellHeight, 0),
                            cellColor));
                        polygons.Add(new Triangle(
                            new Vector3(left, top, 0),
                            new Vector3(left + cellWidth, top, 0),
                            new Vector3(left + cellWidth, top - cellHeight, 0),
                            cellColor));
                        
                        //polygons.AddRange(ModelBuilder.CreateParallelepiped(
                        //    new Vector3(left, top, 0),
                        //    new Vector3(cellWidth, 0, 0),
                        //    new Vector3(0, -cellHeight, 0),
                        //    new Vector3(0, 0, -20f),
                        //    cellColor));

                        /*Array.Copy(cell, cellCopy, cell.Length);
                        ModelBuilder.TransformPolygons(cellCopy,
                            MatrixHelper.CreateScale(cellWidth, cellHeight, (cellWidth + cellHeight) / 2f) *
                            MatrixHelper.CreateTranslation(
                                left + cellWidth / 2f,
                                top - cellHeight / 2f,
                                0));
                        polygons.AddRange(cellCopy);*/
					}
				}
			}
		}
		
		public void PokeAt(int left, int top, bool? newCellState)
		{
			if (left < 0 || left >= FieldWidth)
				throw new ArgumentOutOfRangeException("left");
			if (top < 0 || top >= FieldHeight)
				throw new ArgumentOutOfRangeException("top");

            if (newCellState == null)
                field[left, top] = !field[left, top];
            else
                field[left, top] = newCellState.Value;
		}
		
		public void Clear()
		{
			Array.Clear(field, 0, field.Length);
		}
		
		public void NextGeneration()
		{
			bool[,] newGeneration = new bool[FieldWidth, FieldHeight];
			
			for (int i = 0; i < FieldWidth; i++)
			{
				for (int j = 0; j < FieldHeight; j++)
				{
					int neighbourCount = 0;
					
					int left = i == 0 ? 0 : -1;
					int top = j == 0 ? 0 : -1;
					int right = i == FieldWidth - 1 ? 0 : 1;
					int bottom = j == FieldHeight - 1 ? 0 : 1;
					
					for (int k = left; k <= right; k++)
					{
						for (int m = top; m <= bottom; m++)
						{
							if (k == 0 && m == 0)
								continue;
							
							if (field[i + k, j + m])
								neighbourCount++;
						}
					}
					
					if (field[i, j])
						newGeneration[i, j] = aliveRules[neighbourCount];
					else
						newGeneration[i, j] = birthRules[neighbourCount];
				}
			}
			
			field = newGeneration;
		}
	}
}
