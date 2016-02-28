using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using SoftwareGraphics;

namespace SoftwareGraphics.Console
{
    internal class ConsoleBuffer
    {
        const int StdOutputHandle = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        private struct Coord
        {
            public short X;
            public short Y;

            public Coord(short x, short y)
            {
                X = x;
                Y = y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        private struct CharUnion
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct CharInfo
        {
            [FieldOffset(0)]
            public CharUnion Char;
            [FieldOffset(2)]
            public short Attributes;

            public ConsoleColor CharColor
            {
                get { return (ConsoleColor)((ushort)Attributes & 0x000F); }
            }

            public ConsoleColor BackColor
            {
                get { return (ConsoleColor)((ushort)Attributes >> 4); }
            }

            public CharInfo(char value, ConsoleColor charColor, ConsoleColor backColor)
            {
                Char = new CharUnion { UnicodeChar = value };
                Attributes = (short)((ushort)charColor + ((ushort)backColor << 4));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        IntPtr consoleOutHandle;
        CharInfo[] buffer;
        Coord bufferSize;
        Coord bufferCoord;
        SmallRect writeRegion;

        private CharInfo this[int x, int y]
        {
            get { return buffer[x + y * bufferSize.X]; }
            set { buffer[x + y * bufferSize.X] = value; }
        }

        public Viewport Viewport { get; private set; }

        public ConsoleBuffer(Viewport viewport)
        {
            Viewport = viewport;
            consoleOutHandle = GetStdHandle(StdOutputHandle);
            
            buffer = new CharInfo[viewport.Width * viewport.Height];
            bufferSize = new Coord((short)viewport.Width, (short)viewport.Height);
            bufferCoord = new Coord(0, 0);
            
            writeRegion = new SmallRect
            {
                Left = (short)viewport.X,
                Top = (short)viewport.Y,
                Right = (short)(viewport.X + viewport.Width),
                Bottom = (short)(viewport.Y + viewport.Height),
            };
        }

        public void Clear(char fillChar, ConsoleColor charColor, ConsoleColor backColor)
        {
            CharInfo filler = new CharInfo(fillChar, charColor, backColor);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = filler;
            }
        }

        public void Write(int x, int y, char value, ConsoleColor charColor)
        {
            var info = this[x, y];
            ConsoleColor backColor = info.BackColor;
            this[x, y] = new CharInfo(value, charColor, backColor);
        }

        public void Write(int x, int y, ConsoleColor backColor)
        {
            var info = this[x, y];
            this[x, y] = new CharInfo(info.Char.UnicodeChar, info.CharColor, backColor);
        }

        public void Write(int x, int y, char value, ConsoleColor charColor, ConsoleColor backColor)
        {
            buffer[x + y * bufferSize.X] = new CharInfo(value, charColor, backColor);
        }

        public void Flush()
        {
            SmallRect region = writeRegion;

            WriteConsoleOutput(
                consoleOutHandle,
                buffer,
                bufferSize,
                bufferCoord,
                ref region);
        }
    }
}