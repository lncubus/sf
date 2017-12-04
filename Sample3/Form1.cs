using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Sample
{
	public partial class Form1 : Form
	{
		private Random random = new Random ();

        //UserControl spaceView;

		public Form1 ()
		{
			Font = new Font (Font.FontFamily, 1.5F * Font.Size);
			InitializeComponent ();


            //spaceView.MouseDown += spaceView_MouseDown;
            //spaceView.MouseUp += spaceView_MouseUp;
            //spaceView.MouseMove += spaceView_MouseMove;

//            this.Controls.Add (spaceView);
	//		spaceView.BringToFront ();

			//this.WindowState = FormWindowState.Maximized;
		}

		bool rotating;
		Point origin;
//		Matrix4x4 world;

		private void spaceView_MouseDown (object sender, MouseEventArgs e)
		{
			//if (e.Button != MouseButtons.Left)
			//	return;
			////origin = spaceView.PointToScreen (e.Location);
			////world = spaceView.WorldMatrix;
			//rotating = true;
			////spaceView.Cursor = Cursors.NoMove2D;
		}

		private void spaceView_MouseMove (object sender, MouseEventArgs e)
		{
			//if (e.Button != MouseButtons.Left || !rotating)
			//{
			//	spaceView.Cursor = Cursors.Default;
			//	return;
			//}
			//Point current = spaceView.PointToScreen (e.Location);
			//double height = Math.Min (SpaceView.Resolution.Width, SpaceView.Resolution.Height);
			//Vector3 axis = new Vector3 {
			//	X = (origin.Y - current.Y) / height,
			//	Y = (origin.X - current.X) / height,
			//	Z = 0,
			//};
			//double angle = axis.Length ();
			//if (angle * height < 3)
			//	return;
			//axis /= angle;
			//var rotation = Matrix4x4.CreateFromAxisAngle (axis, Math.PI * angle);
			//spaceView.WorldMatrix = rotation * world;
		}

		private void spaceView_MouseUp (object sender, MouseEventArgs e)
		{
			//rotating = false;
			//spaceView.Cursor = Cursors.Default;
		}
    }
}
