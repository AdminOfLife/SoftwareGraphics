using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vector2 = GenericMathematics.Vector2<int>;

namespace SoftwareGraphics.Console.UI
{
    public abstract class Control
    {
        string name = string.Empty;
        Vector2 size = Vector2.Zero;
        Brush background = new Brush();
        string text = string.Empty;

        public Control Parent { get; internal set; }
        public string Name
        {
            get { return name; }
            set
            {
                if (name == null)
                    throw new ArgumentNullException("value");

                name = value;
            }
        }

        public virtual Vector2 Position { get; set; }
        public virtual Vector2 Size
        {
            get { return size; }
            set
            {
                if (value.X < 0 || value.Y < 0)
                    throw new ArgumentException("value.X and Y can't be < 0.", "value");

                size = value;
            }
        }

        public virtual ConsoleColor CharColor { get; set; }
        public virtual Brush Background
        {
            get { return background; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                background = value;
            }
        }

        public virtual bool Enabled { get; set; }
        public virtual bool Visible { get; set; }

        public virtual bool TabStop { get; set; }
        public virtual int TabIndex { get; set; }

        public int ZIndex { get; set; }

        public virtual string Text
        {
            get { return text; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                text = value;
            }
        }

        public object Tag { get; set; }

        public event EventHandler GotFocus;
        public event EventHandler LostFocus;
        public event EventHandler<KeyEventArgs> KeyPress;

        public Control()
        {
            Position = Vector2.Zero;

            CharColor = ConsoleColor.White;

            Enabled = true;
            Visible = true;
            TabStop = true;
            TabIndex = 0;
            ZIndex = 0;
        }

        protected internal virtual void OnGotFocus()
        {
            var temp = System.Threading.Interlocked.CompareExchange(
                ref GotFocus, null, null);

            if (temp != null)
                temp(this, EventArgs.Empty);
        }

        protected internal virtual void OnLostFocus()
        {
            var temp = System.Threading.Interlocked.CompareExchange(
                ref LostFocus, null, null);

            if (temp != null)
                temp(this, EventArgs.Empty);
        }

        protected internal virtual void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            var temp = System.Threading.Interlocked.CompareExchange(
                ref KeyPress, null, null);

            if (temp != null)
                temp(this, new KeyEventArgs(keyInfo));
        }

        protected internal virtual void DispatchKeyPress(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Tab:
                    if (Application.Instance.FocusedControl == this)
                    {
                        if (keyInfo.Modifiers == (ConsoleModifiers)0)
                            Application.Instance.MoveFocusNext();
                        else if (keyInfo.Modifiers == ConsoleModifiers.Shift)
                            Application.Instance.MoveFocusPrevious();
                    }
                    break;
                default:
                    OnKeyPress(keyInfo);
                    break;
            }
        }

        public virtual void Draw(ConsoleGraphics graphics)
        {
        }

        public override string ToString()
        {
            string text = GetType().Name;
            if (Name != string.Empty)
                text = Name + ": " + text;

            return text;
        }
    }
}
