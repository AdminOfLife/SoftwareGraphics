namespace WinformsExample
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem howDoIToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem cullModeToolStripMenuItem;
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useCursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.wireframeModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showEdgesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cullClockwiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cullCounterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scenePanel = new System.Windows.Forms.Panel();
            this.labelPolygons = new System.Windows.Forms.Label();
            this.labelPolygonsTitle = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            howDoIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cullModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scenePanel.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exit_Click);
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            howDoIToolStripMenuItem});
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // howDoIToolStripMenuItem
            // 
            howDoIToolStripMenuItem.Name = "howDoIToolStripMenuItem";
            howDoIToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            howDoIToolStripMenuItem.Text = "How Do I";
            howDoIToolStripMenuItem.Click += new System.EventHandler(this.howDoI_Click);
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useCursorToolStripMenuItem,
            this.toolStripSeparator1,
            this.wireframeModeToolStripMenuItem,
            this.showEdgesToolStripMenuItem,
            cullModeToolStripMenuItem});
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // useCursorToolStripMenuItem
            // 
            this.useCursorToolStripMenuItem.CheckOnClick = true;
            this.useCursorToolStripMenuItem.Name = "useCursorToolStripMenuItem";
            this.useCursorToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.useCursorToolStripMenuItem.Text = "Use Cursor";
            this.useCursorToolStripMenuItem.CheckedChanged += new System.EventHandler(this.useCursor_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(205, 6);
            // 
            // wireframeModeToolStripMenuItem
            // 
            this.wireframeModeToolStripMenuItem.CheckOnClick = true;
            this.wireframeModeToolStripMenuItem.Name = "wireframeModeToolStripMenuItem";
            this.wireframeModeToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.wireframeModeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.wireframeModeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.wireframeModeToolStripMenuItem.Text = "Wireframe Mode";
            this.wireframeModeToolStripMenuItem.Click += new System.EventHandler(this.wireframeMode_Click);
            // 
            // showEdgesToolStripMenuItem
            // 
            this.showEdgesToolStripMenuItem.CheckOnClick = true;
            this.showEdgesToolStripMenuItem.Name = "showEdgesToolStripMenuItem";
            this.showEdgesToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.showEdgesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.showEdgesToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.showEdgesToolStripMenuItem.Text = "Show Edges";
            this.showEdgesToolStripMenuItem.Click += new System.EventHandler(this.showEdges_Click);
            // 
            // cullModeToolStripMenuItem
            // 
            cullModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.cullClockwiseToolStripMenuItem,
            this.cullCounterToolStripMenuItem});
            cullModeToolStripMenuItem.Name = "cullModeToolStripMenuItem";
            cullModeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            cullModeToolStripMenuItem.Text = "Cull Mode";
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.noneToolStripMenuItem.Text = "None";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.cullModeChange_Click);
            // 
            // cullClockwiseToolStripMenuItem
            // 
            this.cullClockwiseToolStripMenuItem.Name = "cullClockwiseToolStripMenuItem";
            this.cullClockwiseToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.cullClockwiseToolStripMenuItem.Text = "Cull Clockwise Faces";
            this.cullClockwiseToolStripMenuItem.Click += new System.EventHandler(this.cullModeChange_Click);
            // 
            // cullCounterToolStripMenuItem
            // 
            this.cullCounterToolStripMenuItem.Checked = true;
            this.cullCounterToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cullCounterToolStripMenuItem.Name = "cullCounterToolStripMenuItem";
            this.cullCounterToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.cullCounterToolStripMenuItem.Text = "Cull Counter Clockwise Faces";
            this.cullCounterToolStripMenuItem.Click += new System.EventHandler(this.cullModeChange_Click);
            // 
            // scenePanel
            // 
            this.scenePanel.BackColor = System.Drawing.Color.White;
            this.scenePanel.Controls.Add(this.labelPolygons);
            this.scenePanel.Controls.Add(this.labelPolygonsTitle);
            this.scenePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scenePanel.Location = new System.Drawing.Point(0, 24);
            this.scenePanel.Name = "scenePanel";
            this.scenePanel.Size = new System.Drawing.Size(912, 522);
            this.scenePanel.TabIndex = 0;
            this.scenePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.scenePanel_MouseMove);
            this.scenePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.scenePanel_MouseDown);
            this.scenePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.scenePanel_MouseUp);
            // 
            // labelPolygons
            // 
            this.labelPolygons.AutoSize = true;
            this.labelPolygons.BackColor = System.Drawing.Color.Transparent;
            this.labelPolygons.Location = new System.Drawing.Point(117, 10);
            this.labelPolygons.Name = "labelPolygons";
            this.labelPolygons.Size = new System.Drawing.Size(43, 13);
            this.labelPolygons.TabIndex = 0;
            this.labelPolygons.Text = "<none>";
            // 
            // labelPolygonsTitle
            // 
            this.labelPolygonsTitle.AutoSize = true;
            this.labelPolygonsTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelPolygonsTitle.Location = new System.Drawing.Point(12, 10);
            this.labelPolygonsTitle.Name = "labelPolygonsTitle";
            this.labelPolygonsTitle.Size = new System.Drawing.Size(99, 13);
            this.labelPolygonsTitle.TabIndex = 0;
            this.labelPolygonsTitle.Text = "Polygons (drawing):";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fileToolStripMenuItem,
            optionsToolStripMenuItem,
            helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(912, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 546);
            this.Controls.Add(this.scenePanel);
            this.Controls.Add(this.mainMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinformsExample";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.scenePanel.ResumeLayout(false);
            this.scenePanel.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel scenePanel;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label labelPolygons;
        private System.Windows.Forms.Label labelPolygonsTitle;
        private System.Windows.Forms.ToolStripMenuItem useCursorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cullClockwiseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cullCounterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showEdgesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wireframeModeToolStripMenuItem;
    }
}

