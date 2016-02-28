using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vector2 = GenericMathematics.Vector2<int>;

namespace SoftwareGraphics.Console.UI
{
    public class Label : Control
    {
        static readonly Vector2 DefaultSize = new Vector2(7, 3);

        string[] lines;

        public bool AutoSize { get; set; }

        public override Vector2 Size
        {
            get
            {
                if (AutoSize)
                    return new Vector2(lines.Max(line => line.Length), lines.Length);
                else
                    return base.Size;
            }
            set
            {
                base.Size = value;
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                lines = base.Text.Split(
                    new[] { Environment.NewLine }, StringSplitOptions.None);
            }
        }

        public virtual TextAlignment TextAlignment { get; set; }

        public Label()
        {
            AutoSize = true;
            Size = DefaultSize;
            TabStop = false;
        }

        public override void Draw(ConsoleGraphics graphics)
        {
            if (Text.Length == 0)
                return;

            graphics.FillRectangle(Position, Size, Background);
            if (AutoSize)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    graphics.DrawLine(new Vector2(Position.X, Position.Y + i), lines[i],
                        CharColor, Background.BackColor);
                }
            }
            else
            {
                List<string> wrappedLines = new List<string>();
                var size = Size;

                foreach (string line in lines)
                {
                    string remainder = line;
                    while (remainder.Length > 0 && wrappedLines.Count < size.Y)
                    {
                        int cutLength = Math.Min(size.X, remainder.Length);
                        wrappedLines.Add(remainder.Substring(0, cutLength));
                        remainder = remainder.Remove(0, cutLength);
                    }

                    if (wrappedLines.Count >= size.Y)
                        break;
                }

                switch (TextAlignment)
                {
                    case TextAlignment.Left:
                        for (int i = 0; i < wrappedLines.Count; i++)
                        {
                            graphics.DrawLine(new Vector2(Position.X, Position.Y + i),
                                wrappedLines[i], CharColor, Background.BackColor);
                        }
                        break;
                    case TextAlignment.Right:
                        for (int i = 0; i < wrappedLines.Count; i++)
                        {
                            string line = wrappedLines[i];
                            graphics.DrawLine(
                                new Vector2(Position.X + size.X - line.Length, Position.Y + i),
                                line, CharColor, Background.BackColor);
                        }
                        break;
                    case TextAlignment.Center:
                        for (int i = 0; i < wrappedLines.Count; i++)
                        {
                            string line = wrappedLines[i];
                            graphics.DrawLine(
                                new Vector2(Position.X + (size.X - line.Length) / 2, Position.Y + i),
                                line, CharColor, Background.BackColor);
                        }
                        break;
                }
            }
        }
    }
}
