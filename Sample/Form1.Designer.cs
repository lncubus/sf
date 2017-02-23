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

            this.viewContainer1 = new Pvax.UI.Views.ViewContainer();
            this.SuspendLayout();
            // 
            // viewContainer1
            // 
            this.viewContainer1.BackColor = System.Drawing.Color.DimGray;
            this.viewContainer1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewContainer1.ForeColor = System.Drawing.Color.White;
            this.viewContainer1.Location = new System.Drawing.Point(0, 0);
            this.viewContainer1.Name = "viewContainer1";
            this.viewContainer1.Size = new System.Drawing.Size(292, 272);
            this.viewContainer1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 272);
            this.Controls.Add(this.viewContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Pvax.UI.Views.ViewContainer viewContainer1;
    }
}

