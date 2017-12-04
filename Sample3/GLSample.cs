using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using GLGDIPlus;

namespace Sample
{
	public partial class GLSample : GLControl
	{
		// ============================================================
		bool mIsLoaded = false;

		GLGraphics g = new GLGraphics();
		// ============================================================
		public GLSample()
		{
			InitializeComponent();
		}
        // ============================================================

        protected override void OnPaint(PaintEventArgs e)
		{
			if (this.DesignMode)	// в режиме дизайна просто закрашиваем контрол цветом
			{
				e.Graphics.Clear(this.BackColor);
				e.Graphics.Flush();
				return;
			}

			if (!mIsLoaded)		// если OpenGL контекст еще не создан
				return;

			MakeCurrent();

			g.Reset();
            g.Resize(ClientRectangle.Width, ClientRectangle.Height);
            //g.FillRectangle(Color.Gray, (Width - 60) / 2, (Height - 40) / 2, 60, 40);
            g.DrawRectangle(Color.ForestGreen, 0, 0, ClientSize.Width, ClientSize.Height);
            g.DrawRectangle(Color.Red, 1, 1, ClientSize.Width-1, ClientSize.Height-1);
            g.DrawLine(Color.ForestGreen, 0, 0, ClientSize.Width, ClientSize.Height);
            //g.DrawLine(Color.Red, 0, 0, Width, Height);
            g.DrawLine(Color.ForestGreen, ClientSize.Width, 0, 0, ClientSize.Height);
            //g.DrawLine(Color.Red, Width, 0, 0, Height);
            g.FillRectangle(Color.Red, 8, 8, 8, 8);
            g.FillRectangle(Color.Red, ClientSize.Width - 18, ClientSize.Height - 18, 8, 8);

            SwapBuffers();
		}
		// ============================================================
		private void GLSample_Load(object sender, EventArgs e)
		{
			if (this.DesignMode)
				return;

            mIsLoaded = true;	// OpenGL контекст уже должен быть создан

			g.Init();
			g.Reset();
			g.Clear();
			g.SetClearColor(BackColor);
		}

		private void GLSample_Resize(object sender, EventArgs e)
		{
			if (this.DesignMode)
			{
				return;
			}

            Invalidate();
		}
		// ============================================================
	}
}
