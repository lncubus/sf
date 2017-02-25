using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample
{
    public partial class Form1 : Form
    {
//        private Pvax.UI.Views.ButtonView buttonView1;
  //      private Pvax.UI.Views.ButtonView buttonView2;
	//	private Pvax.UI.Views.ImageView imageView1;
	//	private Pvax.UI.Views.IView panelView1;

	//	private EllipticButtonView ellipticButtonView1;
        private SpaceView spaceView;
      //  private TextBox textBox1;
        private Timer timer;
        private Random random = new Random();
        Dictionary<SpaceView.Icon, PointF> velocity = new Dictionary<SpaceView.Icon, PointF>();

        public Form1()
        {
            InitializeComponent();

        //    Font = new Font(Font.FontFamily, Font.Size * 1.5F);

		//	imageView1 = new Pvax.UI.Views.ImageView()
		//	{
		//		Image = Image.FromFile("sample.jpeg"),
		//		SizeMode = Pvax.UI.Views.ImageViewSizeMode.AutoSize,
		//	};
		//	viewContainer1.Views.Add(imageView1);

            spaceView = new SpaceView(0, 0, 1200, 700);
            var icons = spaceView.Icons;
            Color textColor = Color.FromArgb(192, Color.White);
            icons.AddRange(new []
                {
                    new SpaceView.Icon
                    {
                        EdgeColor = Color.White,
                        FillColor = Color.FromArgb(128, Color.White),
                        HoverColor = Color.White,
                        TextColor = textColor,
                        Symbol = SpaceView.Symbol.Ellipse,
                        X = 1,
                        Y = 1,
                        W = 0.35355F,
                        H = 0.35355F,
                        //Text = "Планета",
                    },
                    new SpaceView.Icon
                    {
                        EdgeColor = Color.White,
                        FillColor = Color.FromArgb(128, Color.Aqua),
                        HoverColor = Color.White,
                        TextColor = textColor,
                        Symbol = SpaceView.Symbol.Cross,
                        X = 2,
                        Y = 2,
                        W = 0.35355F,
                        H = 0.35355F,
                        //Text = "Ambulance",
                    },
                    new SpaceView.Icon
                    {
                        EdgeColor = Color.FromArgb(0, 100, 176),
                        FillColor = Color.FromArgb(185, 0, 100, 176),
                        HoverColor = Color.FromArgb(0, 100, 176),
                        Symbol = SpaceView.Symbol.Rectangle,
                        TextColor = textColor,
                        X = 1F,
                        Y = 1F,
                        W = 0.5F,
                        H = 0.278F,
                        //Text = "Друг",
                    },
                    new SpaceView.Icon
                    {
                        EdgeColor = Color.FromArgb(0, 255, 0),
                        FillColor = Color.FromArgb(174, 0, 255, 0),
                        HoverColor = Color.FromArgb(0, 255, 0),
                        TextColor = textColor,
                        Symbol = SpaceView.Symbol.Rectangle,
                        X = 1,
                        Y = 1,
                        W = 0.35355F,
                        H = 0.35355F,
                        //Text = "Сосед",
                    },
                    new SpaceView.Icon
                    {
                        EdgeColor = Color.FromArgb(170, 170, 0),
                        FillColor = Color.FromArgb(177, 170, 170, 0),
                        HoverColor = Color.FromArgb(170, 170, 0),
                        TextColor = textColor,
                        Symbol = SpaceView.Symbol.Quatrefoil,
                        X = 1,
                        Y = 1,
                        W = 0.5F,
                        H = 0.5F,
                        //Text = "Хер с горы",
                    },
                    new SpaceView.Icon
                    {
                        EdgeColor = Color.FromArgb(255, 0, 0),
                        FillColor = Color.FromArgb(180, 255, 0, 0),
                        HoverColor = Color.FromArgb(255, 0, 0),
                        Symbol = SpaceView.Symbol.Diamond,
                        TextColor = textColor,
                        X = 1F,
                        Y = 1F,
                        W = 0.5F,
                        H = 0.5F,
                        //Text = "Враг",
                    },
                });

            viewContainer1.Views.Add(spaceView);

            timer = new Timer(this.components)
            {
                Interval = 100,
                Enabled = true,
            };

            timer.Tick += (object sender, EventArgs e) => 
                {
                    foreach (SpaceView.Icon icon in spaceView.Icons)
                    {
                        PointF v;
                        if (!velocity.TryGetValue(icon, out v))
                        {
                            v = new PointF
                                {
                                    X = (float)(random.NextDouble() - 0.5)*0.01F,
                                    Y = (float)(random.NextDouble() - 0.5)*0.01F,
                                };
                        }
                        else
                        {
                            v.X += (float)(random.NextDouble() - 0.5)*0.01F;
                            v.Y += (float)(random.NextDouble() - 0.5)*0.01F;
                        }
                        if (icon.X < 0)
                            v.X = Math.Abs(v.X);
                        if (icon.Y < 0)
                            v.Y = Math.Abs(v.Y);
                        if (icon.X > spaceView.Width/96)
                            v.X = -Math.Abs(v.X);
                        if (icon.Y > spaceView.Height/96)
                            v.Y = -Math.Abs(v.Y);
                        velocity[icon] = v;
                            
                        icon.X += v.X;
                        icon.Y += v.Y;
                    }
                    spaceView.Invalidate();
                };
            
//			panelView1 = new Pvax.UI.Views.PanelView /*ButtonView*/(80, 20, 100, 150)
//			{
//				//OwnerDraw = true,
//				BorderStyle = BorderStyle.Fixed3D,
//				BackColor = Color.FromArgb(128, Color.DarkSeaGreen),
//				ForeColor = Color.FromArgb(128, Color.Fuchsia),
//			};

//            ellipticButtonView1 = new EllipticButtonView(155, 15, 150, 50)
//            {
//                Text = "ОГОНЬ",
//                ForeColor = Color.LightYellow,
//                BackColor = Color.Firebrick,
//                HoverColor = Color.Red,
//            };

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
//            viewContainer1.Views.Add(ellipticButtonView1);

			this.viewContainer1.BackColor = Color.Black;

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
