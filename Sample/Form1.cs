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
        private Pvax.UI.Views.ButtonView buttonView1;
        private Pvax.UI.Views.ButtonView buttonView2;
		private Pvax.UI.Views.ImageView imageView1;
		private Pvax.UI.Views.IView panelView1;

		private EllipticButtonView ellipticButtonView1;
        private TextBox textBox1;

        public Form1()
        {
            InitializeComponent();

			Font = new Font(Font.FontFamily, Font.Size * 1.5F);

			imageView1 = new Pvax.UI.Views.ImageView()
			{
				Image = Image.FromFile("sample.jpeg"),
				SizeMode = Pvax.UI.Views.ImageViewSizeMode.AutoSize,
			};

			viewContainer1.Views.Add(imageView1);

			panelView1 = new Pvax.UI.Views.PanelView /*ButtonView*/(80, 20, 100, 150)
			{
				//OwnerDraw = true,
				BorderStyle = BorderStyle.Fixed3D,
				BackColor = Color.FromArgb(128, Color.DarkSeaGreen),
				ForeColor = Color.FromArgb(128, Color.Fuchsia),
			};


            ellipticButtonView1 = new EllipticButtonView(155, 15, 150, 50)
            {
                Text = "ОГОНЬ",
                ForeColor = Color.LightYellow,
                BackColor = Color.Firebrick,
                HoverColor = Color.Red,
            };

            // 
            // buttonView1
            // 
            this.buttonView1 = new Pvax.UI.Views.ButtonView(15, 15, 100, 25);
            this.buttonView1.Text = "Test button";
			this.buttonView1.ForeColor = Color.AliceBlue;
			this.buttonView1.BackColor = Color.FromArgb(16, Color.Cyan);
			this.buttonView1.HoverColor = Color.FromArgb(128, Color.Cyan);
            //this.buttonView1.Click = "Test button";
            this.viewContainer1.Views.Add(this.buttonView1);

            this.buttonView2 = new Pvax.UI.Views.ButtonView(15, 115, 100, 25);
            this.buttonView2.Text = "Test button";
            this.buttonView2.ForeColor = Color.Yellow;
            this.buttonView2.BackColor = Color.Green;
//            this.buttonView2.OwnerDraw = true;
            //this.buttonView1.Click = "Test button";
            this.viewContainer1.Views.Add(this.buttonView2);

            viewContainer1.Views.Add(ellipticButtonView1);

			this.viewContainer1.BackColor = Color.Black;

			textBox1 = new TextBox
			{
				BackColor = Color.Black,
				ForeColor = Color.SpringGreen,
				Left = 115,
				Top = 130,
				
			};
			this.viewContainer1.Controls.Add(textBox1);
			this.viewContainer1.Views.Add(panelView1);

			buttonView2.Click += 
				(s, e) => textBox1.BorderStyle = (BorderStyle)(3 - textBox1.BorderStyle);

        }
    }
}
