namespace LifeGameExample
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
            this.canvas = new SoftwareGraphics.Controls.Canvas3D();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Camera = null;
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.GraphicsDeviceType = SoftwareGraphics.Controls.DeviceType.Direct3D;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.MinimumSize = new System.Drawing.Size(1, 1);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(658, 557);
            this.canvas.TabIndex = 0;
            this.canvas.Draw += new System.EventHandler<SoftwareGraphics.Controls.CanvasDrawEventArgs>(this.canvas_Draw);
            this.canvas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.canvas_KeyDown);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 557);
            this.Controls.Add(this.canvas);
            this.Name = "MainForm";
            this.Text = "Life Game";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SoftwareGraphics.Controls.Canvas3D canvas;
    }
}

