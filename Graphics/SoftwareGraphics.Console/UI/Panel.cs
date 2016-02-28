using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoftwareGraphics;

using Vector2 = GenericMathematics.Vector2<int>;

namespace SoftwareGraphics.Console.UI
{
    public class Panel : PanelControl
    {
        static readonly Vector2 DefaultSize = new Vector2(10, 8);

        BorderPen border;

        public bool HasBorder { get; set; }

        public BorderPen Border
        {
            get { return border; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                border = value;
            }
        }

        public TextAlignment CaptionAlignment { get; set; }

        public Panel()
        {
            Size = DefaultSize;
            Border = new BorderPen()
            {
                BackColor = null,
                CharColor = ConsoleColor.Gray,
                HorizontalBorder = '-',
                VerticalBorder = '|',
                LeftTop = 'o',
                LeftBottom = 'o',
                RightTop = 'o',
                RightBottom = 'o',
            };
        }

        public override void Draw(ConsoleGraphics graphics)
        {
            graphics.FillRectangle(Position, Size, Background);
            ConsoleGraphics innerGraphics;

            if (HasBorder)
            {
                graphics.DrawRectangle(Position, Size, Border);

                int maxLength = Size.X - 2;
                int captionLength = Math.Max(0, Math.Min(maxLength, Text.Length));
                if (captionLength > 0 && Size.Y > 0)
                {
                    string caption = null;

                    Vector2 start = new Vector2(Position.X + 1, Position.Y);
                    switch (CaptionAlignment)
                    {
                        case TextAlignment.Left:
                            caption = Text.Substring(0, captionLength);
                            break;
                        case TextAlignment.Right:
                            start.X += maxLength - captionLength;
                            caption = Text.Substring(Text.Length - captionLength, captionLength);
                            break;
                        case TextAlignment.Center:
                            start.X += (maxLength - captionLength) / 2;
                            int diff = Text.Length - captionLength;
                            caption = Text.Substring(diff / 2, captionLength);
                            break;
                    }

                    graphics.DrawLine(start, caption, CharColor, Border.BackColor);
                }

                innerGraphics = graphics.CreateRegion(
                    new Viewport(Position.X + 1, Position.Y + 1, Size.X - 2, Size.Y - 2));
            }
            else
            {
                innerGraphics = graphics.CreateRegion(
                    new Viewport(Position.X, Position.Y, Size.X, Size.Y));
            }

            var controls = Controls.OrderBy(c => c.ZIndex);
            foreach (Control control in controls)
            {
                control.Draw(innerGraphics);
            }
        }
    }
}
