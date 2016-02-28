using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinformsExample
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            string[][] items = new[]
            {
                new[] { "Hold middle mouse buton", "Move camera in its XY plane" },
                new[] { "Hold right mouse button", "Rotate camera around zero point" },
                new[] { "Left, Right", "Move object by X axis" },
                new[] { "Up, Down", "Move object by Y axis" },
                new[] { "Q, E", "Move object by Z axis" },
                new[] { "<, >", "Rotate object around X axis" },
                new[] { "[, ]", "Rotate object around Y axis" },
                new[] { "PageUp, PageDown", "Rotate object around Z axis" },
                new[] { "G", "Generate part (layer) in Random System, Pythagoras Tree, etc" },
                new[] { "Ctrl + W", "Turn on/off wireframe draw mode (without polygon filling)" },
                new[] { "Ctrl + S", "Turn on/off showing edges of polygons" },
                new[] { "Shift + <move/rotate key>", "Move (rotate) by (around) object local axis" },
                new[] { "Ctrl + <move/rotate key>", "Move (rotate) another object" },
            };

            listViewHelp.Items.AddRange(
                (from item in items
                 select new ListViewItem(item)).ToArray());
        }
    }
}
