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
//        private Pvax.UI.Views.ButtonView buttonView1;
  //      private Pvax.UI.Views.ButtonView buttonView2;
	//	private Pvax.UI.Views.ImageView imageView1;
	//	private Pvax.UI.Views.IView panelView1;

        private Pvax.UI.Views.ButtonView ellipticButtonView1;
        private SpaceView spaceView;
      //  private TextBox textBox1;
        private Timer timer;
        private Random random = new Random();
        Dictionary<IconView, PointF> velocity = new Dictionary<IconView, PointF>();
        List<IconView> icons;

        void MoveShips(float w = 0)
        {
            {
                spaceView.BeginUpdate();
                foreach (IconView icon in icons)
                {
                    PointF v = velocity[icon];
                    v.X += (float)(random.NextDouble() - 0.5)*0.01F;
                    v.Y += (float)(random.NextDouble() - 0.5)*0.01F;
                    if (icon.Left < 0)
                        v.X = Math.Abs(v.X);
                    if (icon.Top < 0)
                        v.Y = Math.Abs(v.Y);
                    if (icon.Left + icon.Width > spaceView.ClientSize.Width)
                        v.X = -Math.Abs(v.X);
                    if (icon.Top + icon.Height > spaceView.ClientSize.Height)
                        v.Y = -Math.Abs(v.Y);
                    if (Math.Abs(w) > 0.1)
                    {
                        v = new PointF(v.X * w, v.Y * w);
                    }

                    velocity[icon] = v;
                    icon.X += v.X;
                    icon.Y += v.Y;
                }
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
                    X = (float)(random.NextDouble() - 0.5)*4F,
                    Y = (float)(random.NextDouble() - 0.5)*4F,
                    W = 1,
                    H = 1,
                    Name = "Я",
                },
                new IconView
                {
                    EdgeColor = Color.White,
                    BackColor = Color.FromArgb(128, Color.White),
                    HoverColor = Color.White,
                    ForeColor = textColor,
                    Symbol = Symbol.Star,
                    X = (float)(random.NextDouble() - 0.5)*4F,
                    Y = (float)(random.NextDouble() - 0.5)*4F,
                    W = 1,
                    H = 1,
                    Name = "Я",
                },
                new IconView
                {
                    EdgeColor = Color.Gold,
                    BackColor = Color.FromArgb(128, Color.Gold),
                    HoverColor = Color.Gold,
                    ForeColor = textColor,
                    Symbol = Symbol.Asterisk,
                    X = (float)(random.NextDouble() - 0.5)*4F,
                    Y = (float)(random.NextDouble() - 0.5)*4F,
                    W = 1,
                    H = 1,
                    Name = "Шериф",
                },               
                new IconView
                {
                    EdgeColor = Color.White,
                    BackColor = Color.FromArgb(128, Color.White),
                    HoverColor = Color.White,
                    ForeColor = textColor,
                    Symbol = Symbol.Ellipse,
                    X = (float)(random.NextDouble() - 0.5),
                    Y = (float)(random.NextDouble() - 0.5),
                    W = 0.35355F,
                    H = 0.35355F,
                    Name = "Планета",
                },
                new IconView
                {
                    EdgeColor = Color.White,
                    BackColor = Color.FromArgb(128, Color.Aqua),
                    HoverColor = Color.White,
                    ForeColor = textColor,
                    Symbol = Symbol.Cross,
                    X = (float)(random.NextDouble() - 0.5),
                    Y = (float)(random.NextDouble() - 0.5),
                    W = 0.35355F,
                    H = 0.35355F,
                    Name = "Ambulance",
                },
                new IconView
                {
                    EdgeColor = Color.FromArgb(0, 100, 176),
                    BackColor = Color.FromArgb(185, 0, 100, 176),
                    HoverColor = Color.FromArgb(0, 100, 176),
                    Symbol = Symbol.Rectangle,
                    ForeColor = textColor,
                    X = (float)(random.NextDouble() - 0.5),
                    Y = (float)(random.NextDouble() - 0.5),
                    W = 0.5F,
                    H = 0.278F,
                    Name = "Друг",
                },
                new IconView
                {
                    EdgeColor = Color.FromArgb(0, 255, 0),
                    BackColor = Color.FromArgb(174, 0, 255, 0),
                    HoverColor = Color.FromArgb(0, 255, 0),
                    ForeColor = textColor,
                    Symbol = Symbol.Rectangle,
                    X = (float)(random.NextDouble() - 0.5),
                    Y = (float)(random.NextDouble() - 0.5),
                    W = 0.35355F,
                    H = 0.35355F,
                    Name = "Сосед",
                },
                new IconView
                {
                    EdgeColor = Color.FromArgb(170, 170, 0),
                    BackColor = Color.FromArgb(177, 170, 170, 0),
                    HoverColor = Color.FromArgb(170, 170, 0),
                    ForeColor = textColor,
                    Symbol = Symbol.Quatrefoil,
                    X = (float)(random.NextDouble() - 0.5),
                    Y = (float)(random.NextDouble() - 0.5),
                    W = 0.5F,
                    H = 0.5F,
                    Name = "Хер с горы",
                },
                new IconView
                {
                    EdgeColor = Color.FromArgb(255, 0, 0),
                    BackColor = Color.FromArgb(180, 255, 0, 0),
                    HoverColor = Color.FromArgb(255, 0, 0),
                    Symbol = Symbol.Diamond,
                    ForeColor = textColor,
                    X = (float)(random.NextDouble() - 0.5),
                    Y = (float)(random.NextDouble() - 0.5),
                    W = 0.5F,
                    H = 0.5F,
                    Name = "Враг",
                },
                new IconView
                {
                        EdgeColor = Color.ForestGreen,
                        BackColor = Color.LimeGreen,
                        HoverColor = Color.DarkGreen,
                        Symbol = Symbol.Custom,
                        ForeColor = textColor,
                    X = (float)(random.NextDouble() - 0.5),
                    Y = (float)(random.NextDouble() - 0.5),
                    W = 0.75F,
                    H = 0.75F,
                        Name = "☕",
               },
                    new IconView
                    {
                        EdgeColor = Color.Red,
                        BackColor = Color.Red,
                        HoverColor = Color.DarkRed,
                        Symbol = Symbol.Custom,
                        ForeColor = textColor,
                        X = (float)(random.NextDouble() - 0.5),
                        Y = (float)(random.NextDouble() - 0.5),
                        W = 0.75F,
                        H = 0.25F,
                        Name = "BANG!",
                    },
                    new IconView
                    {
                        EdgeColor = Color.Blue,
                        BackColor = Color.SeaGreen,
                        HoverColor = Color.LightSeaGreen,
                        Symbol = Symbol.Custom,
                        ForeColor = Color.White,
                        X = (float)(random.NextDouble() - 0.5)*4F,
                        Y = (float)(random.NextDouble() - 0.5)*4F,
                        CustomSymbol = teapot,
                        W = 1F,
                        H = 1F,
                    },
            };

            teapot.AddEllipse(new RectangleF(0, 0, 1F, 1F));
            teapot.AddBezier(-0.5F, 0, 0, 0, 0, 1, 0, 0.5F);

            foreach (char c in "☠☣♚♛♜♝♞♟")
            {
                IconView i = new IconView
                {
                        EdgeColor = Color.Chocolate,
                        BackColor = Color.Gold,
                        HoverColor = Color.Chocolate,
                        Symbol = Symbol.Custom,
                        ForeColor = textColor,
                        X = (float)(random.NextDouble() - 0.5)*4F,
                        Y = (float)(random.NextDouble() - 0.5)*4F,
                        W = 0.5F,
                        H = 0.5F,
                        Name = new string(c, 1),
                };
                icons.Add(i);
            }

            foreach (IconView icon in icons)
            {
                velocity.Add(icon, new PointF
                    {
                        X = (float)(random.NextDouble() - 0.5) * 0.01F,
                        Y = (float)(random.NextDouble() - 0.5) * 0.01F,
                    });
            }

            timer = new Timer(this.components)
            {
                Interval = 100,
                Enabled = true,
            };

            timer.Tick += (object sender, EventArgs e) => MoveShips();


            ellipticButtonView1 = new Pvax.UI.Views.ButtonView(155, 15, 150, 50)
            {
                Text = "ОГОНЬ",
                ForeColor = Color.LightYellow,
                BackColor = Color.Firebrick,
                HoverColor = Color.Red,
            };
            ellipticButtonView1.Click += (object sender, EventArgs e) => MoveShips(-1.25F);
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
                    spaceView.WorldOrigin = new PointF
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
