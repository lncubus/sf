using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pvax.App.StringBox
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnFillSB;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbLinesNumber;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private Pvax.App.StringBox.StringBox stringBox;
		private System.Windows.Forms.Button btnClearLB;
		private System.Windows.Forms.Button btnClearSB;
		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.Button btnFillLB;
		private System.Windows.Forms.ErrorProvider errorProvider;

		private int linesInBox = 100000;

		public MainForm()
		{
			InitializeComponent();
		}

		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.errorProvider = new System.Windows.Forms.ErrorProvider();
			this.btnFillLB = new System.Windows.Forms.Button();
			this.listBox = new System.Windows.Forms.ListBox();
			this.btnClearSB = new System.Windows.Forms.Button();
			this.btnClearLB = new System.Windows.Forms.Button();
			this.stringBox = new Pvax.App.StringBox.StringBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tbLinesNumber = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnFillSB = new System.Windows.Forms.Button();
			this.tabPage2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// btnFillLB
			// 
			this.btnFillLB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFillLB.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnFillLB.Location = new System.Drawing.Point(320, 344);
			this.btnFillLB.Name = "btnFillLB";
			this.btnFillLB.Size = new System.Drawing.Size(88, 24);
			this.btnFillLB.TabIndex = 4;
			this.btnFillLB.Text = "Fill";
			this.btnFillLB.Click += new System.EventHandler(this.BtnFillLBClick);
			// 
			// listBox
			// 
			this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox.IntegralHeight = false;
			this.listBox.Location = new System.Drawing.Point(0, 0);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(529, 334);
			this.listBox.TabIndex = 3;
			// 
			// btnClearSB
			// 
			this.btnClearSB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearSB.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnClearSB.Location = new System.Drawing.Point(424, 344);
			this.btnClearSB.Name = "btnClearSB";
			this.btnClearSB.Size = new System.Drawing.Size(88, 24);
			this.btnClearSB.TabIndex = 6;
			this.btnClearSB.Text = "Clear";
			this.btnClearSB.Click += new System.EventHandler(this.BtnClearSBClick);
			// 
			// btnClearLB
			// 
			this.btnClearLB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearLB.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnClearLB.Location = new System.Drawing.Point(424, 344);
			this.btnClearLB.Name = "btnClearLB";
			this.btnClearLB.Size = new System.Drawing.Size(88, 24);
			this.btnClearLB.TabIndex = 5;
			this.btnClearLB.Text = "Clear";
			this.btnClearLB.Click += new System.EventHandler(this.BtnClearLBClick);
			// 
			// stringBox
			// 
			this.stringBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.stringBox.AutoScrollMinSize = new System.Drawing.Size(525, 0);
			this.stringBox.BackColor = System.Drawing.SystemColors.Window;
			this.stringBox.Location = new System.Drawing.Point(0, 0);
			this.stringBox.Name = "stringBox";
			this.stringBox.Size = new System.Drawing.Size(529, 334);
			this.stringBox.TabIndex = 0;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.btnClearSB);
			this.tabPage2.Controls.Add(this.btnFillSB);
			this.tabPage2.Controls.Add(this.stringBox);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(529, 374);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "String box";
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnClearLB);
			this.tabPage1.Controls.Add(this.btnFillLB);
			this.tabPage1.Controls.Add(this.listBox);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(529, 374);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "List box";
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
						| System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Location = new System.Drawing.Point(0, 40);
			this.tabControl.Multiline = true;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(537, 400);
			this.tabControl.TabIndex = 3;
			// 
			// tbLinesNumber
			// 
			this.tbLinesNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbLinesNumber.Location = new System.Drawing.Point(160, 8);
			this.tbLinesNumber.Name = "tbLinesNumber";
			this.tbLinesNumber.Size = new System.Drawing.Size(352, 20);
			this.tbLinesNumber.TabIndex = 5;
			this.tbLinesNumber.Text = "100000";
			this.tbLinesNumber.TextChanged += new System.EventHandler(this.TbLinesNumberTextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 16);
			this.label1.TabIndex = 4;
			this.label1.Text = "Number of lines:";
			// 
			// btnFillSB
			// 
			this.btnFillSB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFillSB.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnFillSB.Location = new System.Drawing.Point(320, 344);
			this.btnFillSB.Name = "btnFillSB";
			this.btnFillSB.Size = new System.Drawing.Size(88, 24);
			this.btnFillSB.TabIndex = 5;
			this.btnFillSB.Text = "Fill";
			this.btnFillSB.Click += new System.EventHandler(this.BtnFillSBClick);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(536, 438);
			this.Controls.Add(this.tbLinesNumber);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tabControl);
			this.Name = "MainForm";
			this.Text = "StringBox control";
			this.tabPage2.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		void BtnFillLBClick(object sender, System.EventArgs e)
		{
			Cursor old = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			string[] texts = new string[linesInBox];
			for(int i = 1; i <= linesInBox; i++)
				texts[i - 1] = "Text line number " + i.ToString();
			listBox.Items.AddRange(texts);
			Cursor.Current = old;
		}

		void BtnFillSBClick(object sender, System.EventArgs e)
		{
			Cursor old = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			string[] texts = new string[linesInBox];
			StringBuilder builder = new StringBuilder(32);
			for(int i = 1; i <= linesInBox; i++)
			{
				builder.Append("Text line number ");
				builder.Append(i.ToString());
				texts[i - 1] = builder.ToString();
				builder.Length = 0;
			}
			stringBox.BeginUpdate();
			for(int i = 0; i < linesInBox; i++)
				stringBox.AddString(texts[i]);
			stringBox.EndUpdate();
			Cursor.Current = old;
		}

		void BtnClearLBClick(object sender, System.EventArgs e)
		{
			listBox.Items.Clear();
		}

		void BtnClearSBClick(object sender, System.EventArgs e)
		{
			stringBox.Views.Clear();
		}

		void TbLinesNumberTextChanged(object sender, System.EventArgs e)
		{
			int n = 0;
			try
			{
				n = Convert.ToInt32(this.tbLinesNumber.Text);
			}
			catch(FormatException)
			{
				this.errorProvider.SetError(this.tbLinesNumber, "Please, enter only non-negative numbers.");
				return;
			}
			catch(OverflowException)
			{
				this.errorProvider.SetError(this.tbLinesNumber, "The value you've entered is too big");
				return;
			}
			if(linesInBox < 0)
			{
				errorProvider.SetError(this.tbLinesNumber, "Please, enter only non-negative numbers.");
				return;
			}
			linesInBox = n;
			this.errorProvider.SetError(this.tbLinesNumber, String.Empty);
		}

	}
}
