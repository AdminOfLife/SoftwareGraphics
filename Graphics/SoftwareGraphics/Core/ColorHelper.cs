using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SoftwareGraphics
{
    public static class ColorHelper
    {
        static readonly Dictionary<Color, ConsoleColor> colorMap =
            new Dictionary<Color, ConsoleColor>()
            {
                { Color.Black, ConsoleColor.Black },
                { Color.Blue, ConsoleColor.Blue },
                { Color.Cyan, ConsoleColor.Cyan },
                { Color.Gray, ConsoleColor.Gray },
                { Color.Green, ConsoleColor.Green },
                { Color.Magenta, ConsoleColor.Magenta },
                { Color.Red, ConsoleColor.Red },
                { Color.White, ConsoleColor.White },
                { Color.Yellow, ConsoleColor.Yellow },
                { Color.DarkBlue, ConsoleColor.DarkBlue },
                { Color.DarkCyan, ConsoleColor.DarkCyan },
                { Color.DarkGray, ConsoleColor.DarkGray },
                { Color.DarkGreen, ConsoleColor.DarkGreen },
                { Color.DarkMagenta, ConsoleColor.DarkMagenta },
                { Color.DarkRed, ConsoleColor.DarkRed },
                { Color.FromArgb(139, 139, 0), ConsoleColor.DarkYellow },
            };

        public static Color FromAhsb(int a, float h, float s, float b)
        {
            if (a < 0 || a > 255)
            {
                throw new ArgumentOutOfRangeException("a", a,
                  "Alpha must be in [0..255].");
            }
            if (h < 0f || h > 360f)
            {
                throw new ArgumentOutOfRangeException("h", h,
                  "Hue must be in [0..360].");
            }
            if (s < 0f || s > 1f)
            {
                throw new ArgumentOutOfRangeException("s", s,
                  "Saturation must be in [0..1].");
            }
            if (b < 0f || b > 1f)
            {
                throw new ArgumentOutOfRangeException("b", b,
                  "Brightness must be in [0..1].");
            }

            if (s == 0)
            {
                return Color.FromArgb(a, Convert.ToInt32(b * 255),
                  Convert.ToInt32(b * 255), Convert.ToInt32(b * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(a, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(a, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(a, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(a, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(a, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(a, iMax, iMid, iMin);
            }
        }

        public static ConsoleColor ToConsole(Color color)
        {
            int minDifference = int.MaxValue;
            ConsoleColor consoleColor = ConsoleColor.Black;

            foreach (var pair in colorMap)
            {
                int diff =
                    Math.Abs(pair.Key.R - color.R) +
                    Math.Abs(pair.Key.G - color.G) +
                    Math.Abs(pair.Key.B - color.B);

                if (diff < minDifference)
                {
                    minDifference = diff;
                    consoleColor = pair.Value;
                }
            }

            return consoleColor;
        }

        public static IEnumerable<Color> GetStatic(Color color)
        {
            while (true)
            {
                yield return color;
            }
        }

        public static IEnumerable<Color> GetRainbow(Color start, float hueStep)
        {
            Color color = start;

            while (true)
            {
                yield return color;

                float hue = color.GetHue() + hueStep;
                if (hue > 360f)
                    hue = 0;

                color = ColorHelper.FromAhsb(
                    color.A,
                    hue,
                    color.GetSaturation(),
                    color.GetBrightness());
            }
        }

        public static IEnumerable<Color> GetRandom(Random random)
        {
            if (random == null)
                throw new ArgumentNullException("random");

            while (true)
            {
                yield return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }
        }

        public static IEnumerable<Color> GetFixedColors(params Color[] values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length == 0)
                throw new ArgumentException("values must be not empty.", "values");

            int index = 0;
            while (true)
            {
                yield return values[index];
                index = (index + 1) % values.Length;
            }
        }
    }
}
