namespace WinformsExample
{
    partial class HelpForm
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
            this.listViewHelp = new System.Windows.Forms.ListView();
            this.columnHeaderKey = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAction = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // listViewHelp
            // 
            this.listViewHelp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderKey,
            this.columnHeaderAction});
            this.listViewHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewHelp.FullRowSelect = true;
            this.listViewHelp.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewHelp.Location = new System.Drawing.Point(0, 0);
            this.listViewHelp.Name = "listViewHelp";
            this.listViewHelp.Size = new System.Drawing.Size(611, 339);
            this.listViewHelp.TabIndex = 0;
            this.listViewHelp.UseCompatibleStateImageBehavior = false;
            this.listViewHelp.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderKey
            // 
            this.columnHeaderKey.Text = "Key";
            this.columnHeaderKey.Width = 180;
            // 
            // columnHeaderAction
            // 
            this.columnHeaderAction.Text = "Action";
            this.columnHeaderAction.Width = 400;
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 339);
            this.Controls.Add(this.listViewHelp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "HelpForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "How Do I";
            this.Load += new System.EventHandler(this.HelpForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewHelp;
        private System.Windows.Forms.ColumnHeader columnHeaderKey;
        private System.Windows.Forms.ColumnHeader columnHeaderAction;
    }
}