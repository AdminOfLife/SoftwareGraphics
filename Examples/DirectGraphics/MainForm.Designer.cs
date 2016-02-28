namespace DirectGraphics
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelGraphics = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelGraphics
            // 
            this.panelGraphics.BackColor = System.Drawing.Color.White;
            this.panelGraphics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraphics.Location = new System.Drawing.Point(0, 0);
            this.panelGraphics.Name = "panelGraphics";
            this.panelGraphics.Size = new System.Drawing.Size(676, 530);
            this.panelGraphics.TabIndex = 0;
            this.panelGraphics.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelGraphics_MouseMove);
            this.panelGraphics.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelGraphics_MouseDown);
            this.panelGraphics.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelGraphics_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 530);
            this.Controls.Add(this.panelGraphics);
            this.Name = "MainForm";
            this.Text = "Direct Graphics";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGraphics;
    }
}

