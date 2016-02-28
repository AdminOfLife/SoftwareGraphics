using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SoftwareGraphics.Console.UI
{
    public sealed class ControlCollection : Collection<Control>
    {
        Action<Control> previewControlAdd;
        Action<Control> previewControlRemove;
        Action<Control> controlAdded;
        Action<Control> controlRemoved;

        internal ControlCollection(
            Action<Control> previewControlAdd, Action<Control> controlAdded,
            Action<Control> previewControlRemove, Action<Control> controlRemoved)
        {
            this.previewControlAdd = previewControlAdd;
            this.previewControlRemove = previewControlRemove;
            this.controlAdded = controlAdded;
            this.controlRemoved = controlRemoved;
        }

        protected override void InsertItem(int index, Control item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (Contains(item))
                throw new InvalidOperationException("Collection already contains this control.");

            previewControlAdd(item);
            base.InsertItem(index, item);
            controlAdded(item);
        }

        protected override void SetItem(int index, Control item)
        {
            throw new NotSupportedException();
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];

            previewControlRemove(item);
            base.RemoveItem(index);
            controlRemoved(item);
        }

        protected override void ClearItems()
        {
            while (Count > 0)
            {
                var item = this[0];

                previewControlRemove(item);
                RemoveAt(0);
                controlRemoved(item);
            }

            base.ClearItems();
        }
    }
}
