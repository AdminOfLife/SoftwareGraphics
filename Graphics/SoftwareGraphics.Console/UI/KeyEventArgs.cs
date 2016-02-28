using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Console.UI
{
    [Serializable]
    public sealed class KeyEventArgs : EventArgs
    {
        public ConsoleKeyInfo KeyInfo { get; set; }

        public KeyEventArgs(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }
    }
}
