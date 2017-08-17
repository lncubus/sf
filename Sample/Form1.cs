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
//        private Pvax.UI.Views.ButtonView buttonView1;
  //      private Pvax.UI.Views.ButtonView buttonView2;
	//	private Pvax.UI.Views.ImageView imageView1;
	//	private Pvax.UI.Views.IView panelView1;

        private Pvax.UI.Views.ButtonView ellipticButtonView1;
        private SpaceView spaceView;
      //  private TextBox textBox1;
        private Random random = new Random();
        Dictionary<IconView, Vector3> velocity = new Dictionary<IconView, Vector3>();
        List<IconView> icons;

		private double shift = 0;
        private double speed = 0.01;
        private double drift = 0.001;

        Vector3 RandomVector(double a = 1)
        {
            return new Vector3
            {
                X = a * (random.NextDouble() - 0.5),
                Y = a * (random.NextDouble() - 0.5),
                Z = a * (random.NextDouble() - 0.5),
            };
        }

        void MoveShips()
        {
            {
                spaceView.BeginUpdate();
                //foreach (IconView icon in icons)
                //{
                //    Vector3 v;
                //    if (!velocity.TryGetValue(icon, out v))
                //        continue;
                //    v += RandomVector(drift);
                //    if (icon.Left < 0 || icon.Top < 0 ||
                //        icon.Left + icon.Width > spaceView.ClientSize.Width ||
                //        icon.Top + icon.Height > spaceView.ClientSize.Height)
                //    {
                //        v = -v * 0.75;
                //    }
                //    velocity[icon] = v;
                //    icon.Vector += v;
                //}

                Vector3 position = Vector3.Zero;
                Vector3 up = Vector3.UnitY;
                shift += Math.PI / 180;
                Vector3 forward = new Vector3(Math.Sin(shift), 0, -Math.Cos(shift));
                Matrix4x4 world = Matrix4x4.CreateWorld(position, forward, up);
                //Tl.LogObjects(Tl.Reflect(world));
                spaceView.WorldMatrix = world;
                spaceView.EndUpdate();
            };

        }

        public Form1()
        {
            InitializeComponent();
            // 
            // viewContainer1
            // 
            this.spaceView = new SpaceView()
            {
                BackColor = System.Drawing.Color.Black,
                BorderStyle = System.Windows.Forms.BorderStyle.None,
                Dock = System.Windows.Forms.DockStyle.Fill,
                ForeColor = System.Drawing.Color.White,
                Location = new System.Drawing.Point(0, 0),
                Name = "space",
                Size = new System.Drawing.Size(292, 272),
                TabIndex = 0,
            };
            this.Controls.Add(spaceView);

        //    Font = new Font(Font.FontFamily, Font.Size * 1.5F);

		//	imageView1 = new Pvax.UI.Views.ImageView()
		//	{
		//		Image = Image.FromFile("sample.jpeg"),
		//		SizeMode = Pvax.UI.Views.ImageViewSizeMode.AutoSize,
		//	};
		//	viewContainer1.Views.Add(imageView1);

//            spaceView = new SpaceView(0, 0, 1200, 700);
            Color textColor = Color.FromArgb(192, Color.White);
            GraphicsPath teapot = new GraphicsPath();
            icons = new List<IconView>()
            {
                new IconView
                {
                    EdgeColor = Color.White,
                    BackColor = Color.FromArgb(128, Color.White),
                    HoverColor = Color.White,
                    ForeColor = textColor,
                    Symbol = Symbol.Pentagram,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
                    Name = "Я",
                },
                new IconView
                {
                    EdgeColor = Color.White,
                    BackColor = Color.FromArgb(128, Color.White),
                    HoverColor = Color.White,
                    ForeColor = textColor,
                    Symbol = Symbol.Star,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
                    Name = "Я",
                },
                new IconView
                {
                    EdgeColor = Color.Gold,
                    BackColor = Color.FromArgb(128, Color.Gold),
                    HoverColor = Color.Gold,
                    ForeColor = textColor,
                    Symbol = Symbol.Asterisk,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
                    Name = "Шериф",
                },               
                new IconView
                {
                    EdgeColor = Color.White,
                    BackColor = Color.FromArgb(128, Color.White),
                    HoverColor = Color.White,
                    ForeColor = textColor,
                    Symbol = Symbol.Ellipse,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
//                    W = 0.35355F,
//                    H = 0.35355F,
                    Name = "Планета",
                },
                new IconView
                {
                    EdgeColor = Color.White,
                    BackColor = Color.FromArgb(128, Color.Aqua),
                    HoverColor = Color.White,
                    ForeColor = textColor,
                    Symbol = Symbol.Cross,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
                    //W = 0.35355F,
                    //H = 0.35355F,
                    Name = "Ambulance",
                },
                new IconView
                {
                    EdgeColor = Color.FromArgb(0, 100, 176),
                    BackColor = Color.FromArgb(185, 0, 100, 176),
                    HoverColor = Color.FromArgb(0, 100, 176),
                    Symbol = Symbol.Rectangle,
                    ForeColor = textColor,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
//                    W = 0.5F,
//                    H = 0.278F,
                    Name = "Друг",
                },
                new IconView
                {
                    EdgeColor = Color.FromArgb(0, 255, 0),
                    BackColor = Color.FromArgb(174, 0, 255, 0),
                    HoverColor = Color.FromArgb(0, 255, 0),
                    ForeColor = textColor,
                    Symbol = Symbol.Rectangle,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
//                   W = 0.35355F,
 //                   H = 0.35355F,
                    Name = "Сосед",
                },
                new IconView
                {
                    EdgeColor = Color.FromArgb(170, 170, 0),
                    BackColor = Color.FromArgb(177, 170, 170, 0),
                    HoverColor = Color.FromArgb(170, 170, 0),
                    ForeColor = textColor,
                    Symbol = Symbol.Quatrefoil,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
//                    W = 0.5F,
//                    H = 0.5F,
                    Name = "Хер с горы",
                },
                new IconView
                {
                    EdgeColor = Color.FromArgb(255, 0, 0),
                    BackColor = Color.FromArgb(180, 255, 0, 0),
                    HoverColor = Color.FromArgb(255, 0, 0),
                    Symbol = Symbol.Diamond,
                    ForeColor = textColor,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1, 1),
//                    W = 0.5F,
//                    H = 0.5F,
                    Name = "Враг",
                },
                new IconView
                {
                    EdgeColor = Color.ForestGreen,
                    BackColor = Color.LimeGreen,
                    HoverColor = Color.DarkGreen,
                    Symbol = Symbol.Custom,
                    ForeColor = textColor,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1.5F, 1.5F),
                    //W = 0.75F,
                    //H = 0.75F,
                    Name = "☕",
               },
                new IconView
                {
                    EdgeColor = Color.Red,
                    BackColor = Color.Red,
                    HoverColor = Color.DarkRed,
                    Symbol = Symbol.Custom,
                    ForeColor = textColor,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1.5F, 0.5F),
                    Name = "BANG!",
                },
                new IconView
                {
                    EdgeColor = Color.Blue,
                    BackColor = Color.SeaGreen,
                    HoverColor = Color.LightSeaGreen,
                    Symbol = Symbol.Custom,
                    ForeColor = Color.White,
                    CustomSymbol = teapot,
                    Vector = RandomVector(4),
                    IconSize = new SizeF(1.6F, 1),
                },
            };

            //teapot.AddEllipse(new RectangleF(0, 0, 1F, 1F));
            float[][] teapot_points = new []
            {
                new[]
                {
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
                new[]
                {
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
                new[]
                {
                    387.41F, 394.25F,
                    390.15F, 396.99F, 391.06F, 400.64F, 391.06F, 403.38F,
                    391.06F, 427.10F, 327.18F, 439.88F, 264.21F, 439.88F,
                    201.25F, 439.88F, 137.37F, 427.10F, 137.37F, 403.38F,
                    137.37F, 399.73F, 138.28F, 396.99F, 141.02F, 393.34F,
                    176.61F, 411.59F, 221.32F, 420.72F, 265.13F, 420.72F,
                    308.93F, 420.72F, 353.64F, 411.59F, 387.41F, 394.25F,
                },
                new[]
                {
                    295.24F, 97.67F,
                    295.24F, 97.67F, 239.57F, 97.67F, 239.57F, 97.67F,
                    239.57F, 78.51F, 252.35F, 72.12F, 267.86F, 72.12F,
                    283.38F, 72.12F, 296.15F, 78.51F, 295.24F, 97.67F,
                },
            };
                  

            teapot.AddBeziers(teapot_points);

            foreach (char c in "♛")
            {
                IconView i = new IconView
                {
                        EdgeColor = Color.Chocolate,
                        BackColor = Color.Gold,
                        HoverColor = Color.Chocolate,
                        Symbol = Symbol.Custom,
                        ForeColor = textColor,
                        Vector = RandomVector(4),
                        IconSize = new SizeF(1, 1),
                        Name = new string(c, 1),
                };
                icons.Add(i);
            }

            foreach (IconView icon in icons)
            {
                velocity.Add(icon, RandomVector(0.01));
            }

            timer.Tick += (object sender, EventArgs e) => MoveShips();
            timer.Enabled = true;

            ellipticButtonView1 = new Pvax.UI.Views.ButtonView(155, 15, 150, 50)
            {
                Text = "ОГОНЬ",
                ForeColor = Color.LightYellow,
                BackColor = Color.Firebrick,
                HoverColor = Color.Red,
            };
            ellipticButtonView1.Click += (object sender, EventArgs e) =>
            {
                    //spaceView.DeviceScale = new PointF(spaceView.DeviceScale.X*1.03F, spaceView.DeviceScale.Y*1.03F);
                    //Vector3 axis = Vector3.Normalize(new Vectors.Vector3(1, 1, 1));
                    //spaceView.WorldRotation *= new Vectors.Quaternion(axis, 0);
             };
            LabelIconView zero = new LabelIconView
            {
                Vector = Vectors.Vector3.Zero,
				EdgeColor = Color.White,
				BackColor = Color.LightGray,
				HoverColor = Color.White,
				ForeColor = textColor,
                IconSize = new SizeF(0.5F, 0.5F),
                Text = "0",
            };
            icons.Add(zero);
            foreach (char c in "xyz")
            {
                Vectors.Vector3 axis;
                switch (c)
                {
                    case 'x': axis = Vectors.Vector3.UnitX; break;
                    case 'y': axis = Vectors.Vector3.UnitY; break;
                    case 'z': axis = Vectors.Vector3.UnitZ; break;
                    default:
                        throw new NotImplementedException("another dimension");
                }
                for (int z = 1; z < 4; z++)
                {
                    LabelIconView icon = new LabelIconView
                    {
                        Vector = z * axis,
                        EdgeColor = Color.White,
                        BackColor = Color.LightGray,
                        HoverColor = Color.White,
                        ForeColor = textColor,
                        Text = c + "=" + z,// "0",
                        IconSize = new SizeF(1.0F, 0.5F),
                    };
                    icons.Add(icon);
                }
            }
            // 
            // buttonView1
            // 
//            this.buttonView1 = new Pvax.UI.Views.ButtonView(15, 15, 100, 25);
//            this.buttonView1.Text = "Test button";
//			this.buttonView1.ForeColor = Color.AliceBlue;
//			this.buttonView1.BackColor = Color.FromArgb(16, Color.Cyan);
//			this.buttonView1.HoverColor = Color.FromArgb(128, Color.Cyan);
//            //this.buttonView1.Click = "Test button";
//            this.viewContainer1.Views.Add(this.buttonView1);
//
//            this.buttonView2 = new Pvax.UI.Views.ButtonView(15, 115, 100, 25);
//            this.buttonView2.Text = "Test button";
//            this.buttonView2.ForeColor = Color.Yellow;
//            this.buttonView2.BackColor = Color.Green;
////            this.buttonView2.OwnerDraw = true;
//            //this.buttonView1.Click = "Test button";
//            this.viewContainer1.Views.Add(this.buttonView2);
//
            spaceView.Views.AddRange(icons);
            spaceView.Views.Add(ellipticButtonView1);

            spaceView.SizeChanged += (object sender, EventArgs e) =>
                {
                    spaceView.DeviceOrigin = new Point
                    {
                        X = spaceView.ClientSize.Width/2,
                        Y = spaceView.ClientSize.Height/2
                    };
                };

			//this.viewContainer1.BackColor = Color.Black;

//			textBox1 = new TextBox
//			{
//				BackColor = Color.Black,
//				ForeColor = Color.SpringGreen,
//				Left = 115,
//				Top = 130,
//				
//			};
//			this.viewContainer1.Controls.Add(textBox1);
//			this.viewContainer1.Views.Add(panelView1);
//
//			buttonView2.Click += 
//				(s, e) => textBox1.BorderStyle = (BorderStyle)(3 - textBox1.BorderStyle);
           this.WindowState = FormWindowState.Maximized;
        }
    }
}
