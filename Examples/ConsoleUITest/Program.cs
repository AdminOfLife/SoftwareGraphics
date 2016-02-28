using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SoftwareGraphics.Console.UI;

using Vector2 = GenericMathematics.Vector2<int>;

namespace ConsoleUITest
{
    static class Program
    {
        static void Main(string[] args)
        {
            Label label1 = new Label()
            {
                Name = "label1 (Red)",
                AutoSize = false,
                Position = new Vector2(2, 2),
                Size = new Vector2(10, 4),
                Background = new Brush(ConsoleColor.Red),
                Text = "Hello, ConsoleUI!",
                TextAlignment = TextAlignment.Left,
                TabStop = true,
                ZIndex = 10,
            };
            Label label2 = new Label()
            {
                Name = "label2 (Transparent)",
                AutoSize = true,
                Position = new Vector2(3, 3),
                Size = new Vector2(10, 4),
                Background = new Brush(),
                Text = "Hello, ConsoleUI!\r\nUo O_O ^_^ >_< (_!_)]\r\nEmpty",
                TextAlignment = TextAlignment.Center,
                TabStop = true,
                ZIndex = 20,
            };

            Panel panel1 = new Panel()
            {
                Name = "panel1",
                Position = new Vector2(4, 2),
                Background = new Brush('`', ConsoleColor.Cyan, ConsoleColor.DarkMagenta),
                Size = new Vector2(25, 12),
                Text = "I'm panel!",
                HasBorder = true,
                CaptionAlignment = TextAlignment.Center,
                TabStop = true,
            };
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label2);

            Panel panel2 = new Panel()
            {
                Name = "panel2",
                Position = new Vector2(3, 7),
                //Background = new Brush('~', ConsoleColor.Green, ConsoleColor.DarkGreen),
                Background = new Brush('~', ConsoleColor.Green, null),
                Size = new Vector2(40, 17),
                Text = "Outer panel",
                HasBorder = true,
                CaptionAlignment = TextAlignment.Right,
                TabStop = true,
                ZIndex = 10,
            };
            panel2.Controls.Add(panel1);

            Panel panel3 = new Panel()
            {
                Name = "panel3",
                Position = new Vector2(35, 4),
                Background = new Brush(':', ConsoleColor.Blue, ConsoleColor.DarkBlue),
                Size = new Vector2(30, 15),
                Text = "Another panel",
                HasBorder = true,
                CaptionAlignment = TextAlignment.Left,
                TabStop = true,
                ZIndex = 5,
            };
            panel3.Border.LeftTop = '*';
            panel3.Border.LeftBottom = '*';
            panel3.Border.RightTop = '*';
            panel3.Border.RightBottom = '*';
            panel3.Border.CharColor = ConsoleColor.Cyan;

            AddHandlers(panel2);
            AddHandlers(panel3);

            Application.Instance.Controls.Add(panel2);
            Application.Instance.Controls.Add(panel3);
            Application.Instance.Run();
        }

        private static void AddHandlers(Control control)
        {
            control.KeyPress += Control_KeyPress;
            control.GotFocus += Control_GotFocus;
            control.LostFocus += Control_LostFocus;

            PanelControl panel = control as PanelControl;
            if (panel != null)
            {
                foreach (Control child in panel.Controls)
                {
                    AddHandlers(child);
                }
            }
        }

        private static void Control_GotFocus(object sender, EventArgs e)
        {
            ((Control)sender).CharColor = ConsoleColor.Yellow;
        }

        private static void Control_LostFocus(object sender, EventArgs e)
        {
            ((Control)sender).CharColor = ConsoleColor.White;
        }

        private static void Control_KeyPress(object sender, KeyEventArgs e)
        {
            Control c = (Control)sender;
            if ((e.KeyInfo.Modifiers & ConsoleModifiers.Shift) != ConsoleModifiers.Shift)
            {
                switch (e.KeyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        c.Position -= Vector2.UnitY;
                        break;
                    case ConsoleKey.DownArrow:
                        c.Position += Vector2.UnitY;
                        break;
                    case ConsoleKey.LeftArrow:
                        c.Position -= Vector2.UnitX;
                        break;
                    case ConsoleKey.RightArrow:
                        c.Position += Vector2.UnitX;
                        break;
                }
            }
            else
            {
                switch (e.KeyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        c.Size -= Vector2.UnitY;
                        break;
                    case ConsoleKey.DownArrow:
                        c.Size += Vector2.UnitY;
                        break;
                    case ConsoleKey.LeftArrow:
                        c.Size -= Vector2.UnitX;
                        break;
                    case ConsoleKey.RightArrow:
                        c.Size += Vector2.UnitX;
                        break;
                }
            }
        }
    }
}
