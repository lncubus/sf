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

using Vectors;

namespace Sample
{
	public partial class Form1 : Form
	{
		private Pvax.UI.Views.ButtonView rndRectButtonView1;
		private Pvax.UI.Views.ButtonView cutRectButtonView1;
		private SpaceView spaceView;
		//  private TextBox textBox1;
		private Random random = new Random ();
		Dictionary<IconView, Vector3> velocity = new Dictionary<IconView, Vector3> ();
		List<IconView> icons;

		public static readonly Stopwatch mainWatch = Stopwatch.StartNew ();
		public static readonly Stopwatch drawWatch = new Stopwatch ();

		Vector3 RandomVector (double a = 1)
		{
			return new Vector3 {
				X = a * (random.NextDouble () - 0.5),
				Y = a * (random.NextDouble () - 0.5),
				Z = a * (random.NextDouble () - 0.5),
			};
		}

		void MoveShips ()
		{
			{
				spaceView.BeginUpdate ();
				foreach (IconView icon in icons)
				{
					Vector3 v;
					if (!velocity.TryGetValue (icon, out v))
						continue;
//                    v += RandomVector(drift);
					if (icon.Left < 0 || icon.Top < 0 ||
					    icon.Left + icon.Width > spaceView.ClientSize.Width ||
					    icon.Top + icon.Height > spaceView.ClientSize.Height)
					{
						v = -v;
					}
					velocity [icon] = v;
					icon.Vector += v;
				}
				spaceView.EndUpdate ();
				Text = drawWatch.Elapsed.TotalSeconds.ToString ("N2") + " " + mainWatch.Elapsed.TotalSeconds.ToString ("N2") +
				" " + (drawWatch.Elapsed.TotalSeconds / mainWatch.Elapsed.TotalSeconds).ToString ("P");
			}
			;
		}

		public Form1 ()
		{
			Font = new Font (Font.FontFamily, 1.5F * Font.Size);
			InitializeComponent ();

			// 
			// viewContainer1
			// 
			this.spaceView = new SpaceView () {
				BackColor = System.Drawing.Color.Black,
				BorderStyle = System.Windows.Forms.BorderStyle.None,
				Dock = System.Windows.Forms.DockStyle.Fill,
				ForeColor = System.Drawing.Color.White,
				Location = new System.Drawing.Point (0, 0),
				Name = "space",
				Size = new System.Drawing.Size (292, 272),
				TabIndex = 0,
			};
			spaceView.MouseDown += spaceView_MouseDown;
			spaceView.MouseUp += spaceView_MouseUp;
			spaceView.MouseMove += spaceView_MouseMove;
			this.Controls.Add (spaceView);
			spaceView.BringToFront ();
			Color textColor = Color.White;
			GraphicsPath teapot = new GraphicsPath ();
			icons = new List<IconView> () {
				new IconView {
					EdgeColor = Color.White,
					BackColor = Color.LightGray,
					HoverColor = Color.White,
					ForeColor = textColor,
					Symbol = Symbol.Pentagram,
					Vector = RandomVector (4),
					IconSize = new SizeF (1, 1),
					Name = "Я",
				},
				new IconView {
					EdgeColor = Color.White,
					BackColor = Color.LightGray,
					HoverColor = Color.White,
					ForeColor = textColor,
					Symbol = Symbol.Star,
					Vector = RandomVector (4),
					IconSize = new SizeF (1, 1),
					Name = "Я",
				},
				new IconView {
					EdgeColor = Color.Gold,
					BackColor = Color.LightYellow,
					HoverColor = Color.Gold,
					ForeColor = textColor,
					Symbol = Symbol.Asterisk,
					Vector = RandomVector (4),
					IconSize = new SizeF (1, 1),
					Name = "Шериф",
				},               
				new IconView {
					EdgeColor = Color.White,
					BackColor = Color.LightGray,
					HoverColor = Color.White,
					ForeColor = textColor,
					Symbol = Symbol.Ellipse,
					Vector = RandomVector (4),
					IconSize = new SizeF (1, 1),
//                    W = 0.35355F,
//                    H = 0.35355F,
					Name = "Планета",
				},
				new IconView {
					EdgeColor = Color.White,
					BackColor = Color.Aqua,
					HoverColor = Color.White,
					ForeColor = textColor,
					Symbol = Symbol.Cross,
					Vector = RandomVector (4),
					IconSize = new SizeF (1, 1),
					//W = 0.35355F,
					//H = 0.35355F,
					Name = "Ambulance",
				},
				new IconView {
					EdgeColor = Color.ForestGreen,
					BackColor = Color.LimeGreen,
					HoverColor = Color.DarkGreen,
					Symbol = Symbol.Custom,
					ForeColor = textColor,
					Vector = RandomVector (4),
					IconSize = new SizeF (1.5F, 1.5F),
					//W = 0.75F,
					//H = 0.75F,
					Name = "кофе",
					Text = "☕",
				},
				new IconView {
					EdgeColor = Color.Red,
					BackColor = Color.Red,
					HoverColor = Color.DarkRed,
					Symbol = Symbol.Custom,
					ForeColor = textColor,
					Vector = RandomVector (4),
					IconSize = new SizeF (1.5F, 0.5F),
					Name = "BANG!",
					Text = "BANG!",
				},
				new IconView {
					EdgeColor = Color.Blue,
					BackColor = Color.SeaGreen,
					HoverColor = Color.LightSeaGreen,
					Symbol = Symbol.Custom,
					ForeColor = Color.White,
					CustomSymbol = teapot,
					Vector = RandomVector (4),
					IconSize = new SizeF (1.6F, 1),
					Name = "teapot",
				},
			};

			for (int i = 0; i < 16; i++)
			{
				IconView icon = null;
				int i4 = i % 4;
				int num = 1 + i / 4;
				switch (i4)
				{
				case 0:
					icon = new IconView
					{
						Symbol = Symbol.Diamond,
						EdgeColor = Color.Red,
						BackColor = Color.FromArgb (192, Color.Red),
						HoverColor = Color.Red,
						ForeColor = textColor,
						Vector = RandomVector (4),
						IconSize = new SizeF (1.41F, 1.41F),
						Name = "Враг" + num,
					};
					break;
				case 1:
					icon = new IconView
					{
						Symbol = Symbol.Rectangle,
						EdgeColor = Color.RoyalBlue,
						BackColor = Color.FromArgb (192, Color.RoyalBlue),
						HoverColor = Color.RoyalBlue,
						ForeColor = textColor,
						Vector = RandomVector (4),
						IconSize = new SizeF (1.25F, 0.8F),
						Name = "Друг" + num,
					};
					break;
				case 2:
					icon = new IconView
					{
						Symbol = Symbol.Rectangle,
						EdgeColor = Color.ForestGreen,
						BackColor = Color.FromArgb (192, Color.ForestGreen),
						HoverColor = Color.ForestGreen,
						ForeColor = textColor,
						Vector = RandomVector (4),
						IconSize = new SizeF (1.0F, 1.0F),
						Name = "Сосед" + num,
					};
					break;
				case 3:
					icon = new IconView
					{
						EdgeColor = Color.DarkOrange,
						BackColor = Color.FromArgb (192, Color.DarkOrange),
						HoverColor = Color.DarkOrange,
						ForeColor = textColor,
						Symbol = Symbol.Quatrefoil,
						Vector = RandomVector (4),
						IconSize = new SizeF (1.23F, 1.23F),
						//                    W = 0.5F,
						//                    H = 0.5F,
						Name = "Хер с горы" + num,
					};
					break;
				default:
					throw new IndexOutOfRangeException ();
				}
				icons.Add (icon);
			}

			//teapot.AddEllipse(new RectangleF(0, 0, 1F, 1F));
			float[][] teapot_points = new [] {
				new[] {
					506.04F, 148.77F,
					500.57F, 151.51F, 494.18F, 155.16F, 488.70F, 161.55F,
					472.28F, 179.80F, 456.76F, 237.29F, 453.11F, 243.68F,
					453.11F, 243.68F, 450.37F, 250.07F, 445.81F, 253.72F,
					447.64F, 263.76F, 449.46F, 273.79F, 449.46F, 284.75F,
					449.46F, 371.44F, 357.29F, 410.68F, 265.13F, 410.68F,
					173.87F, 410.68F,  80.79F, 371.44F,  81.70F, 284.75F,
					81.70F, 237.29F, 108.17F, 195.31F, 146.49F, 166.11F,
					245.05F, 173.41F, 272.43F, 156.07F, 272.43F, 156.07F,
					272.43F, 156.07F, 214.02F, 158.81F, 175.69F, 147.86F,
					175.69F, 147.86F, 175.69F, 147.86F, 175.69F, 147.86F,
					161.09F, 143.30F, 152.88F, 137.82F, 152.88F, 132.35F,
					152.88F, 117.75F, 204.90F, 105.88F, 268.78F, 105.88F,
					332.65F, 105.88F, 384.67F, 117.75F, 384.67F, 132.35F,
					384.67F, 138.74F, 375.55F, 144.21F, 360.03F, 148.77F,
					374.63F, 156.99F, 388.32F, 167.94F, 400.18F, 179.80F,
					423.00F, 179.80F, 423.91F, 181.63F, 431.21F, 172.50F,
					440.34F, 161.55F, 447.64F, 147.86F, 464.06F, 141.47F,
					475.93F, 136.91F, 496.00F, 138.74F, 506.95F, 140.56F,
					514.25F, 141.47F, 513.34F, 144.21F, 506.04F, 148.77F,
				},
				new[] {
					71.66F, 289.31F,
					72.58F, 308.47F, 78.05F, 325.81F, 86.26F, 341.32F,
					63.45F, 332.20F, 47.02F, 314.86F, 36.07F, 300.26F,
					4.13F, 257.37F, -5.90F, 199.88F,  3.22F, 171.59F,
					13.26F, 138.74F, 40.64F, 134.17F, 55.24F, 134.17F,
					72.58F, 134.17F, 91.74F, 140.56F, 107.25F, 150.60F,
					117.29F, 156.99F, 126.42F, 160.64F, 134.63F, 163.38F,
					120.03F, 175.24F, 108.17F, 188.01F,  99.04F, 202.62F,
					97.21F, 200.79F, 95.39F, 198.05F, 93.56F, 195.31F,
					89.91F, 188.93F, 85.35F, 182.54F, 79.88F, 177.98F,
					74.40F, 173.41F, 68.01F, 171.59F, 62.54F, 171.59F,
					51.59F, 171.59F, 41.55F, 179.80F, 37.90F, 191.66F,
					30.60F, 216.30F, 45.20F, 250.98F, 59.80F, 273.79F,
					63.45F, 279.27F, 67.10F, 284.75F, 71.66F, 289.31F,
				},
				new[] {
					387.41F, 394.25F,
					390.15F, 396.99F, 391.06F, 400.64F, 391.06F, 403.38F,
					391.06F, 427.10F, 327.18F, 439.88F, 264.21F, 439.88F,
					201.25F, 439.88F, 137.37F, 427.10F, 137.37F, 403.38F,
					137.37F, 399.73F, 138.28F, 396.99F, 141.02F, 393.34F,
					176.61F, 411.59F, 221.32F, 420.72F, 265.13F, 420.72F,
					308.93F, 420.72F, 353.64F, 411.59F, 387.41F, 394.25F,
				},
				new[] {
					295.24F, 97.67F,
					295.24F, 97.67F, 239.57F, 97.67F, 239.57F, 97.67F,
					239.57F, 78.51F, 252.35F, 72.12F, 267.86F, 72.12F,
					283.38F, 72.12F, 296.15F, 78.51F, 295.24F, 97.67F,
				},
			};
			teapot.AddBeziers (teapot_points);

			int n = 0;
			int buttonWidth = SpaceView.Dpi.X;
			int buttonSpace = buttonWidth / 8;
			int buttonShift = buttonWidth * 9 / 8;
			int buttonHeight = buttonWidth * 5 / 8;

			foreach (char c in "♚♛♜♝♞")
			{
				int x = buttonSpace + buttonShift * n++;
				IconView i = new IconView {
					EdgeColor = Color.Chocolate,
					BackColor = Color.Gold,
					HoverColor = Color.Chocolate,
					Symbol = Symbol.Custom,
					ForeColor = textColor,
					Vector = RandomVector (4),
					IconSize = new SizeF (1, 1),
					DisabledColor = Color.DarkGray,
					Name = "chessIcon",
					Text = new string (c, 1),
				};
				icons.Add (i);
				Func<int, int, int, GraphicsPath> shape;
				switch (n)
				{
					case 1:
						shape = CustomPaths.Ellipse;
						break;
					case 2:
						shape = CustomPaths.RoundRectangle;
						break;
					case 3:
						shape = CustomPaths.CutRectangle;
						break;
					case 4:
						shape = CustomPaths.Diamond;
						break;
					default:
						shape = null;
						break;
				}
				CustomButtonView b = new CustomButtonView (shape, x, buttonSpace, buttonWidth, buttonHeight, buttonSpace) {
					BackColor = Color.Black,
					HoverColor = Color.Navy,
					DisabledColor = Color.DarkGray,
					ForeColor = Color.White,
					Name = "chess",
					Text = new string (c, 1),
				};
				spaceView.Views.Add (b);
				b.Click += (object sender, EventArgs e) =>
				{
					var button = (CustomButtonView)sender;
					var chess = icons.Find (icon => icon.Text == button.Text);
					chess.Visible = !chess.Visible;
				};
			}

			foreach (string f in new[] { "1", "3", "2" })
			{
				int x = buttonSpace + buttonShift * n++;
				Func<int, int, int, GraphicsPath> shape = null;
				if (n % 2 == 0)
					shape = CustomPaths.RoundRectangle;
				CustomButtonView b = new CustomButtonView (shape, x, buttonSpace, buttonWidth, buttonHeight, buttonSpace) {
					BackColor = Color.FromArgb (39, 39, 39),
					HoverColor = Color.FromArgb (54, 54, 54),
					DisabledColor = Color.DarkGray,
					ForeColor = Color.White,
					Name = "action",
					Text = f,
				};
				spaceView.Views.Add (b);
				b.Click += (object sender, EventArgs e) =>
				{
				};
			}

			foreach (IconView icon in icons)
			{
				velocity.Add (icon, RandomVector (0.05));
			}

			timer.Tick += (object sender, EventArgs e) => MoveShips ();
			timer.Enabled = true;

			rndRectButtonView1 = new CustomButtonView (CustomPaths.RoundRectangle, 155, 205, 150, 50, 10) {
				Text = "ОГОНЬ",
				ForeColor = Color.LightYellow,
				BackColor = Color.Firebrick,
				HoverColor = Color.Red,
				DisabledColor = Color.Pink,
			};

			cutRectButtonView1 = new CustomButtonView (CustomPaths.CutRectangle, 155, 105, 150, 50, 10) {
				Text = "ОГОНЬ",
				ForeColor = Color.LightYellow,
				BackColor = Color.MediumBlue,
				HoverColor = Color.Blue,
				DisabledColor = Color.Cyan,
			};

			rndRectButtonView1.Click += (object sender, EventArgs e) =>
			{
				spaceView.HideNegative = !spaceView.HideNegative;
			};
			cutRectButtonView1.Click += (object sender, EventArgs e) =>
			{
				spaceView.PerspectiveProjection = !spaceView.PerspectiveProjection;
			};

			IconView zero = new IconView {
				Vector = Vectors.Vector3.Zero,
				EdgeColor = Color.White,
				BackColor = Color.LightGray,
				HoverColor = Color.White,
				ForeColor = textColor,
				IconSize = new SizeF (0.5F, 0.5F),
				Symbol = Symbol.Text,
				Name = "zero",
				Text = "0",
			};
			icons.Add (zero);

			spaceView.Views.AddRange (icons);
			spaceView.Views.Add (rndRectButtonView1);
			spaceView.Views.Add (cutRectButtonView1);

			//double w = SpaceView.Resolution.Width / SpaceView.Dpi.X;
			//double h = SpaceView.Resolution.Height / SpaceView.Dpi.Y;

			spaceView.WorldMatrix = Matrix4x4.CreatePerspective (1, 1, 0.5, 3);
				
			//CreateLookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);

			spaceView.SizeChanged += (object sender, EventArgs e) =>
			{
				spaceView.DeviceOrigin = new Point {
					X = spaceView.ClientSize.Width / 2,
					Y = spaceView.ClientSize.Height / 2
				};
			};

			this.WindowState = FormWindowState.Maximized;
		}

		void RandomMove (object sender, EventArgs e)
		{
			var v = (Pvax.UI.Views.IView)sender;
			v.Left = random.Next (spaceView.ClientSize.Width - v.Width);
			v.Top = random.Next (spaceView.ClientSize.Height - v.Height);
			//v.Enabled = false;
			//spaceView.DeviceScale = new PointF(spaceView.DeviceScale.X*1.03F, spaceView.DeviceScale.Y*1.03F);
			//Vector3 axis = Vector3.Normalize(new Vectors.Vector3(1, 1, 1));
			//spaceView.WorldRotation *= new Vectors.Quaternion(axis, 0);
		}

		bool rotating;
		Point origin;
		Matrix4x4 world;

		private void spaceView_MouseDown (object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			origin = spaceView.PointToScreen (e.Location);
			world = spaceView.WorldMatrix;
			rotating = true;
			spaceView.Cursor = Cursors.NoMove2D;
		}

		private void spaceView_MouseMove (object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left || !rotating)
			{
				spaceView.Cursor = Cursors.Default;
				return;
			}
			Point current = spaceView.PointToScreen (e.Location);
			double height = Math.Min (SpaceView.Resolution.Width, SpaceView.Resolution.Height);
			Vector3 axis = new Vector3 {
				X = (current.Y - origin.Y) / height,
				Y = (current.X - origin.X) / height,
				Z = 0,
			};
			double angle = axis.Length ();
			if (angle * height < 3)
				return;
			axis /= angle;
			var rotation = Matrix4x4.CreateFromAxisAngle (axis, Math.PI * angle);
			spaceView.WorldMatrix = rotation * world;
		}

		private void spaceView_MouseUp (object sender, MouseEventArgs e)
		{
			rotating = false;
			spaceView.Cursor = Cursors.Default;
		}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Tl.Log(IconView.Report());
        }
    }
}
