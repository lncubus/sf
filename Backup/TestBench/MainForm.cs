using System;
using System.Drawing;
using System.Windows.Forms;

using Pvax.UI.Views;

namespace Pvax.App.Views.TestBench
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabPage tpRectangles;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.StatusBar statusBar;
		private Pvax.UI.Views.ViewContainer vcComposites;
		private System.Windows.Forms.TabPage tpNesting;
		private Pvax.UI.Views.ViewContainer vcNesting;
		private System.Windows.Forms.TabPage tpNonRect;
		private Pvax.UI.Views.ViewContainer vcNonRect;
		private Pvax.UI.Views.ViewContainer vcRectangles;
		private System.Windows.Forms.TabPage tpComposites;
		
		const int width = 120;
		
		const int height = 40;
		
		const int shiftX = 20;
		
		const int shiftY = 20;
		
		ButtonView buttonOnControl;
		
		ButtonView buttonOnView;
		
		public MainForm()
		{
			InitializeComponent();
			
			{
				int x = 0, y = 0;
				for(int i = 0; i < 20; i++)
				{
					RectangleView view = new RectangleView(x, y, width, height);
					x += shiftX;
					y += shiftY;
					if(6 == i)
						view.Enabled = false;
					vcRectangles.Views.Add(view);
				}
			}
			
			//--------------------------------------------------------
			{
				NestedView level1 = new NestedView("level1", 100, 30, 420, 300);
				vcNesting.Views.Add(level1);
				
				NestedView level2 = new NestedView("level2", 20, 20, 380, 260);
				level1.Views.Add(level2);
				
				NestedView level3 = new NestedView("level3", 20, 20, 340, 220);
				level2.Views.Add(level3);
				
				NestedView level4 = new NestedView("level4", 20, 20, 300, 180);
				level3.Views.Add(level4);
				
				NestedView level5 = new NestedView("level5", 20, 20, 260, 140);
				level4.Views.Add(level5);
				
				NestedView level6 = new NestedView("level6", 20, 20, 220, 100);
				level5.Views.Add(level6);
				
				NestedView level7 = new NestedView("level7", 20, 20, 180, 60);
				level6.Views.Add(level7);
			}
			//--------------------------------------------------------
			{
				LabelView lv = new LabelView("Buttons test", 10, 10, 80, 26);
				lv.TextAlign = ContentAlignment.MiddleCenter;
				vcComposites.Views.Add(lv);
				
				buttonOnControl = new ButtonView(10, 40, 80, 26);
				buttonOnControl.Text = "Button";
				buttonOnControl.Click += new EventHandler(ClickControl);
				vcComposites.Views.Add(buttonOnControl);
				
				ButtonView bv;
				
				bv = new ButtonView(110, 40, 80, 26);
				vcComposites.Views.Add(bv);
				bv.Text = "Enable";
				bv.Click += new EventHandler(EnableControl);
				
				bv = new ButtonView(210, 40, 80, 26);
				vcComposites.Views.Add(bv);
				bv.Text = "Visible";
				bv.Click += new EventHandler(VisibleControl);
				
				PanelView cv = new PanelView(10, 80, 310, 80);
				cv.BorderStyle = BorderStyle.Fixed3D;
				vcComposites.Views.Add(cv);
				
				lv = new LabelView("Buttons test", 10, 10, 80, 26);
				lv.TextAlign = ContentAlignment.MiddleCenter;
				cv.Views.Add(lv);
				
				buttonOnView = new ButtonView(10, 40, 80, 26);
				buttonOnView.Text = "Button";
				buttonOnView.Click += new EventHandler(ClickView);
				cv.Views.Add(buttonOnView);
				
				bv = new ButtonView(110, 40, 80, 26);
				bv.Text = "Enable";
				bv.Click += new EventHandler(EnableView);
				cv.Views.Add(bv);
				
				bv = new ButtonView(210, 40, 80, 26);
				bv.Text = "Visible";
				bv.Click += new EventHandler(VisibleView);
				cv.Views.Add(bv);
			}
			//--------------------------------------------------------
			{
				Color[] colors = new Color[] {Color.AliceBlue, Color.DodgerBlue, Color.LightYellow, Color.Fuchsia, Color.Tan, Color.PaleVioletRed};
				int colorIndex = 0;
				int x = 0, y = 0;
				for(int i = 0; i < 20; i++)
				{
					EllipseView view = new EllipseView(colors[colorIndex], x, y, width, height);
					colorIndex++;
					if(colorIndex == colors.Length)
						colorIndex = 0;
					x += shiftX;
					y += shiftY;
					if(6 == i)
						view.Enabled = false;
					vcNonRect.Views.Add(view);
				}
			}
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tpComposites = new System.Windows.Forms.TabPage();
			this.vcRectangles = new Pvax.UI.Views.ViewContainer();
			this.vcNonRect = new Pvax.UI.Views.ViewContainer();
			this.tpNonRect = new System.Windows.Forms.TabPage();
			this.vcNesting = new Pvax.UI.Views.ViewContainer();
			this.tpNesting = new System.Windows.Forms.TabPage();
			this.vcComposites = new Pvax.UI.Views.ViewContainer();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tpRectangles = new System.Windows.Forms.TabPage();
			this.tpComposites.SuspendLayout();
			this.tpNonRect.SuspendLayout();
			this.tpNesting.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tpRectangles.SuspendLayout();
			this.SuspendLayout();
			// 
			// tpComposites
			// 
			this.tpComposites.Controls.Add(this.vcComposites);
			this.tpComposites.Location = new System.Drawing.Point(4, 22);
			this.tpComposites.Name = "tpComposites";
			this.tpComposites.Size = new System.Drawing.Size(376, 278);
			this.tpComposites.TabIndex = 2;
			this.tpComposites.Text = "Views && Composites";
			// 
			// vcRectangles
			// 
			this.vcRectangles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vcRectangles.Location = new System.Drawing.Point(0, 0);
			this.vcRectangles.Name = "vcRectangles";
			this.vcRectangles.Size = new System.Drawing.Size(320, 238);
			this.vcRectangles.TabIndex = 0;
			// 
			// vcNonRect
			// 
			this.vcNonRect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vcNonRect.Location = new System.Drawing.Point(0, 0);
			this.vcNonRect.Name = "vcNonRect";
			this.vcNonRect.Size = new System.Drawing.Size(320, 238);
			this.vcNonRect.TabIndex = 0;
			// 
			// tpNonRect
			// 
			this.tpNonRect.Controls.Add(this.vcNonRect);
			this.tpNonRect.Location = new System.Drawing.Point(4, 22);
			this.tpNonRect.Name = "tpNonRect";
			this.tpNonRect.Size = new System.Drawing.Size(320, 238);
			this.tpNonRect.TabIndex = 3;
			this.tpNonRect.Text = "Non-rectangular";
			this.tpNonRect.ToolTipText = "Non-rectangular views";
			// 
			// vcNesting
			// 
			this.vcNesting.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vcNesting.Location = new System.Drawing.Point(0, 0);
			this.vcNesting.Name = "vcNesting";
			this.vcNesting.Size = new System.Drawing.Size(320, 238);
			this.vcNesting.TabIndex = 0;
			// 
			// tpNesting
			// 
			this.tpNesting.Controls.Add(this.vcNesting);
			this.tpNesting.Location = new System.Drawing.Point(4, 22);
			this.tpNesting.Name = "tpNesting";
			this.tpNesting.Size = new System.Drawing.Size(320, 238);
			this.tpNesting.TabIndex = 1;
			this.tpNesting.Text = "Nesting";
			// 
			// vcComposites
			// 
			this.vcComposites.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vcComposites.Location = new System.Drawing.Point(0, 0);
			this.vcComposites.Name = "vcComposites";
			this.vcComposites.Size = new System.Drawing.Size(376, 278);
			this.vcComposites.TabIndex = 0;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 304);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(384, 22);
			this.statusBar.TabIndex = 0;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tpComposites);
			this.tabControl.Controls.Add(this.tpRectangles);
			this.tabControl.Controls.Add(this.tpNesting);
			this.tabControl.Controls.Add(this.tpNonRect);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(384, 304);
			this.tabControl.TabIndex = 1;
			// 
			// tpRectangles
			// 
			this.tpRectangles.Controls.Add(this.vcRectangles);
			this.tpRectangles.Location = new System.Drawing.Point(4, 22);
			this.tpRectangles.Name = "tpRectangles";
			this.tpRectangles.Size = new System.Drawing.Size(320, 238);
			this.tpRectangles.TabIndex = 0;
			this.tpRectangles.Text = "Rectangles";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 326);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.statusBar);
			this.Name = "MainForm";
			this.Text = "Views test";
			this.tpComposites.ResumeLayout(false);
			this.tpNonRect.ResumeLayout(false);
			this.tpNesting.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.tpRectangles.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion
		
		private void ClickControl(object sender, EventArgs e)
		{
			MessageBox.Show("Button view on the control clicked!", "Event");
		}
		
		private void EnableControl(object sender, EventArgs e)
		{
			buttonOnControl.Enabled = !buttonOnControl.Enabled;
		}
		
		private void VisibleControl(object sender, EventArgs e)
		{
			buttonOnControl.Visible = !buttonOnControl.Visible;
		}
		
		private void ClickView(object sender, EventArgs e)
		{
			MessageBox.Show("Button view on the composite view clicked!", "Event");
		}
		
		private void EnableView(object sender, EventArgs e)
		{
			buttonOnView.Enabled = !buttonOnView.Enabled;
		}
		
		private void VisibleView(object sender, EventArgs e)
		{
			buttonOnView.Visible = !buttonOnView.Visible;
		}
	}
}
