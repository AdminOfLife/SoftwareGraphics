using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace DirectGraphics
{
	public struct HyperLine
	{
		public Vector4 Start;
		public Vector4 End;

		public Color Color;

		public HyperLine(Vector4 start, Vector4 end, Color color)
		{
			Start = start;
			End = end;
			Color = color;
		}

		public override bool Equals(object obj)
		{
			var triangle = obj as HyperLine?;
			if (triangle == null)
				return false;

			return this.Equals(triangle.Value);
		}

		public bool Equals(HyperLine other)
		{
			return Start == other.Start && End == other.End && Color == other.Color;
		}

		public override int GetHashCode()
		{
			int prime = 31;
			return unchecked(Start.GetHashCode() * prime + End.GetHashCode());
		}

		public static bool operator ==(HyperLine line1, HyperLine line2)
		{
			return line1.Equals(line2);
		}

		public static bool operator !=(HyperLine line1, HyperLine line2)
		{
			return !(line1 == line2);
		}
	}
}
