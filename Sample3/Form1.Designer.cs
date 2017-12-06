namespace Sample
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
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.glSample1 = new Sample.GLSample();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 15;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // glSample1
            // 
            this.glSample1.BackColor = System.Drawing.Color.Black;
            this.glSample1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.glSample1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glSample1.Location = new System.Drawing.Point(0, 0);
            this.glSample1.Margin = new System.Windows.Forms.Padding(5);
            this.glSample1.Name = "glSample1";
            this.glSample1.Size = new System.Drawing.Size(389, 359);
            this.glSample1.TabIndex = 0;
            this.glSample1.VSync = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 359);
            this.Controls.Add(this.glSample1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Timer timer;
        private GLSample glSample1;

        #endregion

        // private Pvax.UI.Views.ViewContainer viewContainer1;
    }
}

