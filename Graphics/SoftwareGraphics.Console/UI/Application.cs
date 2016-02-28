using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Console.UI
{
    public sealed class Application
    {
        ConsoleBuffer screenBuffer;
        ConsoleGraphics screenGraphics;
        Brush background;

        Control focusedControl;

        private static Application instance = new Application();
        public static Application Instance
        {
            get { return instance; }
        }

        public Brush Background
        {
            get { return background; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                background = value;
            }
        }

        public bool CursorVisible
        {
            get { return System.Console.CursorVisible; }
            set { System.Console.CursorVisible = value; }
        }

        public ControlCollection Controls { get; private set; }

        public Control FocusedControl
        {
            get { return focusedControl; }
        }

        private Application()
        {
            screenBuffer = new ConsoleBuffer(
                SoftwareGraphics.Devices.ConsoleGraphicsDevice.GetConsoleViewport());
            screenGraphics = new ConsoleGraphics(screenBuffer);

            background = new Brush(' ', ConsoleColor.White, ConsoleColor.Black);

            Controls = new ControlCollection(
                PreviewControlAdd, ControlAdded,
                PreviewControlRemove, ControlRemoved);

            CursorVisible = false;
            System.Console.CancelKeyPress += Console_CancelKeyPress;
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void PreviewControlAdd(Control control)
        {
        }

        private void PreviewControlRemove(Control control)
        {
        }

        private void ControlAdded(Control control)
        {
            if (focusedControl == null)
                SetFocusOn(control);
        }

        private void ControlRemoved(Control control)
        {
            if (focusedControl == control)
            {
                focusedControl.OnLostFocus();
                if (Controls.Any())
                {
                    focusedControl = Controls.First();
                    focusedControl.OnGotFocus();
                }
                else
                {
                    focusedControl = null;
                }
            }
        }

        public void SetFocusOn(Control target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (focusedControl != null)
                focusedControl.OnLostFocus();

            CursorVisible = false;

            focusedControl = target;
            focusedControl.OnGotFocus();
        }

        public void MoveFocusNext()
        {
            if (focusedControl == null)
                return;

            Control lastFocused = focusedControl;
            do
            {
                PanelControl panel = focusedControl as PanelControl;
                if (panel != null && panel.Controls.Any())
                {
                    focusedControl = panel.Controls.First();
                }
                else
                {
                    Control child;
                    while (true)
                    {
                        do
                        {
                            child = focusedControl;
                            focusedControl = focusedControl.Parent;
                        }
                        while (focusedControl != null &&
                            !(focusedControl is PanelControl));

                        panel = focusedControl as PanelControl;
                        if (panel == null)
                            break;

                        if (!panel.Controls.Any() ||
                            child == panel.Controls.Last())
                        {
                            focusedControl = panel;
                        }
                        else
                        {
                            int index = panel.Controls.IndexOf(child);
                            focusedControl = panel.Controls[index + 1];
                            
                            break;
                        }
                    }

                    if (focusedControl == null)
                    {
                        if (child == this.Controls.Last())
                            focusedControl = this.Controls.First();
                        else
                            focusedControl = this.Controls[this.Controls.IndexOf(child) + 1];
                    }
                }
            }
            while (!focusedControl.TabStop &&
                focusedControl != lastFocused);

            if (focusedControl != lastFocused)
            {
                lastFocused.OnLostFocus();
                focusedControl.OnGotFocus();
            }
        }

        public void MoveFocusPrevious()
        {
            if (focusedControl == null)
                return;

            Control lastFocused = focusedControl;
            do
            {
                PanelControl panel = null;
                bool traverseDown = true;

                while (true)
                {
                    Control child;
                    do
                    {
                        child = focusedControl;
                        focusedControl = focusedControl.Parent;
                    }
                    while (focusedControl != null &&
                        !focusedControl.TabStop &&
                        !(focusedControl is PanelControl));

                    panel = focusedControl as PanelControl;
                    if (panel == null)
                    {
                        if (child == this.Controls.First())
                            focusedControl = this.Controls.Last();
                        else
                            focusedControl = this.Controls[this.Controls.IndexOf(child) - 1];

                        break;
                    }

                    if (child == panel.Controls.First())
                    {
                        focusedControl = panel;

                        if (panel.TabStop)
                        {
                            traverseDown = false;
                            break;
                        }
                    }
                    else
                    {
                        int index = panel.Controls.IndexOf(child);
                        focusedControl = panel.Controls[index - 1];

                        break;
                    }
                }

                panel = focusedControl as PanelControl;
                while (traverseDown && panel != null && panel.Controls.Any())
                {
                    focusedControl = panel.Controls.Last();
                    panel = focusedControl as PanelControl;
                }
            }
            while (!focusedControl.TabStop &&
                focusedControl != lastFocused);

            if (focusedControl != lastFocused)
            {
                lastFocused.OnLostFocus();
                focusedControl.OnGotFocus();
            }
        }

        public void Run()
        {
            while (true)
            {
                if (background.BackColor != null)
                {
                    screenBuffer.Clear(background.FillChar,
                        background.CharColor, background.BackColor.Value);
                }
                else
                {
                    screenBuffer.Clear(background.FillChar,
                        background.CharColor, ConsoleColor.Black);
                }

                var controlsToDraw = Controls.OrderBy(c => c.ZIndex);
                foreach (Control control in controlsToDraw)
                {
                    if (control.Visible)
                        control.Draw(screenGraphics);
                }

                screenBuffer.Flush();

                var keyInfo = System.Console.ReadKey(true);
                if (focusedControl != null)
                    focusedControl.DispatchKeyPress(keyInfo);
            }
        }
    }
}
