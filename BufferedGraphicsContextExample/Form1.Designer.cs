namespace BufferedGraphicsContextExample
{
    partial class Form1
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
            this.doubleBufferedControl1 = new BufferedGraphicsContextExample.DoubleBufferedControl();
            this.SuspendLayout();
            // 
            // doubleBufferedControl1
            // 
            this.doubleBufferedControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doubleBufferedControl1.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferedControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.doubleBufferedControl1.Name = "doubleBufferedControl1";
            this.doubleBufferedControl1.Size = new System.Drawing.Size(379, 322);
            this.doubleBufferedControl1.TabIndex = 0;
			this.doubleBufferedControl1.BackColor = System.Drawing.Color.Black;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 322);
            this.Controls.Add(this.doubleBufferedControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedControl doubleBufferedControl1;
    }
}

