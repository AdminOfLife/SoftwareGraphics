using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Console.UI
{
    public abstract class PanelControl : Control
    {
        public ControlCollection Controls { get; private set; }

        public PanelControl()
        {
            TabStop = false;

            Controls = new ControlCollection(
                PreviewControlAdd, ControlAdded,
                PreviewControlRemove, ControlRemoved);
        }

        protected virtual void PreviewControlAdd(Control control)
        {
        }

        protected virtual void PreviewControlRemove(Control control)
        {
        }

        protected virtual void ControlAdded(Control control)
        {
            control.Parent = this;
        }

        protected virtual void ControlRemoved(Control control)
        {
            if (Application.Instance.FocusedControl == control)
                Application.Instance.MoveFocusNext();

            control.Parent = null;
        }
    }
}
