using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Console.UI
{
    public sealed class Brush : ICloneable
    {
        public char FillChar { get; set; }

        public ConsoleColor CharColor { get; set; }
        public ConsoleColor? BackColor { get; set; }

        public Brush()
            : this(' ', ConsoleColor.White, null)
        {
        }

        public Brush(ConsoleColor backColor)
            : this(' ', ConsoleColor.White, backColor)
        {
        }

        public Brush(char fillChar, ConsoleColor charColor, ConsoleColor? backColor)
        {
            FillChar = fillChar;
            CharColor = charColor;
            BackColor = backColor;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
