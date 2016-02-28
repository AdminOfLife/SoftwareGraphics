using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoftwareGraphics;

using Vector2 = GenericMathematics.Vector2<int>;

namespace SoftwareGraphics.Console.UI
{
    public sealed class ConsoleGraphics
    {
        ConsoleBuffer consoleBuffer;
        Viewport clipRegion;
        Vector2 excludedMax;

        public Vector2 Offset { get; private set; }
        public Viewport ClipRegion
        {
            get { return clipRegion; }
            private set
            {
                clipRegion = value;
                excludedMax = new Vector2(
                    value.X + value.Width,
                    value.Y + value.Height);
            }
        }

        private ConsoleGraphics()
        {
        }

        internal ConsoleGraphics(ConsoleBuffer consoleBuffer)
        {
            this.consoleBuffer = consoleBuffer;

            ClipRegion = new Viewport(
                0, 0,
                consoleBuffer.Viewport.Width,
                consoleBuffer.Viewport.Height);
        }

        public ConsoleGraphics CreateRegion(Viewport region)
        {
            Vector2 offset = Offset + new Vector2(region.X, region.Y);

            // right and bottom are excluded values
            int left   = Math.Max(ClipRegion.X, Math.Min(excludedMax.X, offset.X));
            int top    = Math.Max(ClipRegion.Y, Math.Min(excludedMax.Y, offset.Y));
            int right  = Math.Max(ClipRegion.X, Math.Min(excludedMax.X, offset.X + region.Width));
            int bottom = Math.Max(ClipRegion.Y, Math.Min(excludedMax.Y, offset.Y + region.Height));

            ConsoleGraphics clipped = new ConsoleGraphics();
            clipped.consoleBuffer = this.consoleBuffer;
            clipped.Offset = offset;
            clipped.ClipRegion = new Viewport(left, top, right - left, bottom - top);

            return clipped;
        }

        private void Write(int x, int y, char value, ConsoleColor charColor, ConsoleColor? backColor)
        {
            int absX = Offset.X + x;
            int absY = Offset.Y + y;

            if (absX < ClipRegion.X || absX >= excludedMax.X ||
                absY < ClipRegion.Y || absY >= excludedMax.Y)
            {
                return;
            }

            if (backColor != null)
                consoleBuffer.Write(absX, absY, (char)value, charColor, backColor.Value);
            else if (value != ' ')
                consoleBuffer.Write(absX, absY, (char)value, charColor);
        }

        public void DrawRectangle(Vector2 position, Vector2 size, BorderPen pen)
        {
            if (size.X < 0 || size.Y < 0)
                throw new ArgumentException("size.X or Y can't be < 0.", "size");
            if (size.X == 0 || size.Y == 0)
                return;

            int maxX = position.X + size.X - 1;
            int maxY = position.Y + size.Y - 1;

            for (int x = position.X + 1; x < maxX; x++)
            {
                Write(x, position.Y, pen.HorizontalBorder, pen.CharColor, pen.BackColor);
                Write(x, maxY, pen.HorizontalBorder, pen.CharColor, pen.BackColor);
            }

            for (int y = position.Y + 1; y < maxY; y++)
            {
                Write(position.X, y, pen.VerticalBorder, pen.CharColor, pen.BackColor);
                Write(maxX, y, pen.VerticalBorder, pen.CharColor, pen.BackColor);
            }

            Write(position.X, position.Y, pen.LeftTop, pen.CharColor, pen.BackColor);
            Write(maxX, position.Y, pen.RightTop, pen.CharColor, pen.BackColor);
            Write(position.X, maxY, pen.LeftBottom, pen.CharColor, pen.BackColor);
            Write(maxX, maxY, pen.RightBottom, pen.CharColor, pen.BackColor);
        }

        public void DrawLine(Vector2 position, string value,
            ConsoleColor charColor, ConsoleColor? background)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            for (int i = 0; i < value.Length; i++)
            {
                Write(position.X + i, position.Y, value[i], charColor, background);
            }
        }

        public void FillRectangle(Vector2 position, Vector2 size, Brush brush)
        {
            int maxX = position.X + size.X - 1;
            int maxY = position.Y + size.Y - 1;

            for (int x = position.X; x <= maxX; x++)
            {
                for (int y = position.Y; y <= maxY; y++)
                {
                    Write(x, y, brush.FillChar, brush.CharColor, brush.BackColor);
                }
            }
        }
    }
}
