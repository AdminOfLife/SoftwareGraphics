using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Console.UI
{
    public sealed class BorderPen : ICloneable
    {
        public char HorizontalBorder { get; set; }
        public char VerticalBorder { get; set; }
        public char LeftTop { get; set; }
        public char LeftBottom { get; set; }
        public char RightTop { get; set; }
        public char RightBottom { get; set; }

        public ConsoleColor CharColor { get; set; }
        public ConsoleColor? BackColor { get; set; }

        public BorderPen()
            : this(' ')
        {
        }

        public BorderPen(char defaultCharacter)
        {
            HorizontalBorder = VerticalBorder = defaultCharacter;
            LeftTop = LeftBottom = defaultCharacter;
            RightTop = RightBottom = defaultCharacter;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
