namespace Canvas3DTest
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
            System.Windows.Forms.Label labelModeTitle;
            System.Windows.Forms.Label labelLightTitle;
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelMode = new System.Windows.Forms.Label();
            this.labelLight = new System.Windows.Forms.Label();
            this.canvas = new SoftwareGraphics.Controls.Canvas3D();
            labelModeTitle = new System.Windows.Forms.Label();
            labelLightTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelModeTitle
            // 
            labelModeTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            labelModeTitle.AutoSize = true;
            labelModeTitle.BackColor = System.Drawing.Color.White;
            labelModeTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            labelModeTitle.ForeColor = System.Drawing.Color.Black;
            labelModeTitle.Location = new System.Drawing.Point(3, 0);
            labelModeTitle.Name = "labelModeTitle";
            labelModeTitle.Size = new System.Drawing.Size(97, 24);
            labelModeTitle.TabIndex = 1;
            labelModeTitle.Text = "Mode (m):";
            // 
            // labelLightTitle
            // 
            labelLightTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            labelLightTitle.AutoSize = true;
            labelLightTitle.BackColor = System.Drawing.Color.White;
            labelLightTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            labelLightTitle.ForeColor = System.Drawing.Color.Black;
            labelLightTitle.Location = new System.Drawing.Point(3, 26);
            labelLightTitle.Name = "labelLightTitle";
            labelLightTitle.Size = new System.Drawing.Size(76, 24);
            labelLightTitle.TabIndex = 1;
            labelLightTitle.Text = "Light (l):";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(labelModeTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelMode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(labelLightTitle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLight, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(193, 53);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // labelMode
            // 
            this.labelMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelMode.AutoSize = true;
            this.labelMode.BackColor = System.Drawing.Color.White;
            this.labelMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMode.ForeColor = System.Drawing.Color.Black;
            this.labelMode.Location = new System.Drawing.Point(119, 0);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(57, 24);
            this.labelMode.TabIndex = 1;
            this.labelMode.Text = "None";
            // 
            // labelLight
            // 
            this.labelLight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelLight.AutoSize = true;
            this.labelLight.BackColor = System.Drawing.Color.White;
            this.labelLight.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelLight.ForeColor = System.Drawing.Color.Black;
            this.labelLight.Location = new System.Drawing.Point(123, 26);
            this.labelLight.Name = "labelLight";
            this.labelLight.Size = new System.Drawing.Size(50, 24);
            this.labelLight.TabIndex = 1;
            this.labelLight.Text = "True";
            // 
            // canvas
            // 
            this.canvas.Camera = null;
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.GraphicsDeviceType = SoftwareGraphics.Controls.DeviceType.Gdi;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.MinimumSize = new System.Drawing.Size(1, 1);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(680, 498);
            this.canvas.TabIndex = 3;
            this.canvas.Draw += new System.EventHandler<SoftwareGraphics.Controls.CanvasDrawEventArgs>(this.canvas_Draw);
            this.canvas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.canvas_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 498);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.canvas);
            this.Name = "MainForm";
            this.Text = "Canvas3D Test";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelMode;
        private SoftwareGraphics.Controls.Canvas3D canvas;
        private System.Windows.Forms.Label labelLight;

    }
}

