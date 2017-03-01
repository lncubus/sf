using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Pvax.UI;
using Pvax.UI.Views;

namespace Pvax.App.StringBox
{
	/// <summary>
	/// Description of StringBox.
	/// </summary>
	public class StringBox: ViewContainer
	{
		/// <summary>
		/// Description of StringView.
		/// </summary>
		public class StringView: AbstractView
		{
			private StringBox parent;

			private int y;

			private int height;

			private string text;

			public StringView(StringBox parent, string text)
			{
				this.parent = parent;
				this.text = text;
			}

			public string Text
			{
				get
				{
					return text;
				}
				set
				{
					text = value;
				}
			}

			public override int X
			{
				get
				{
					return 0;
				}

				set
				{}
			}

			public override int Y
			{
				get
				{
					return y;
				}

				set
				{
					y = value;
				}
			}

			public override int Width
			{
				get
				{
					return parent.GetStringWidth();
				}

				set
				{}
			}

			public override int Height
			{
				get
				{
					return height;
				}

				set
				{
					height = value;
				}
			}

			protected override void Draw(Graphics graphics)
			{
				Rectangle rect = new Rectangle(0, 0, Width, Height);
				Brush backgroundBrush;
				Brush foregroundBrush;
				if(Selected)
				{
					backgroundBrush = SystemBrushes.Highlight;
					foregroundBrush = SystemBrushes.HighlightText;
				}
				else
				{
					backgroundBrush = SystemBrushes.Window;
					foregroundBrush = SystemBrushes.WindowText;
				}
				graphics.FillRectangle(backgroundBrush, rect);
				graphics.DrawString(text, parent.Font, foregroundBrush, 1, 1, StringFormat.GenericTypographic);
			}

			protected override void OnMouseUp(int posX, int posY, MouseButtons buttons)
			{
				parent.SetSelectedView(this);
			}

			public override string ToString()
			{
				return text;
			}
		}

		private int selectedIndex = -1;

		public StringBox()
		{
			this.BackColor = SystemColors.Window;
		}

		public void AddString(string text)
		{
			StringView sv = new StringView(this, text);
			sv.Height = this.Font.Height + 2;
			sv.Y = 0;
			int index = base.Views.Count - 1;
			if(index >= 0)
				sv.Y += base.Views[index].Y + base.Views[index].Height;
			Views.Add(sv);
		}

		public void RemoveString(string text)
		{
			for(int i = 0; i < Views.Count; i++)
			{
				StringView view = (StringView)Views[i];
				if(text.Equals(view.Text))
				{
					Views.Remove(view);
					return;
				}
			}
		}

		private int GetStringWidth()
		{
			return ClientSize.Width;
		}

		private int GetStringHeight()
		{
			return (Font.Height + 2);
		}

		protected override void CalcExtent()
		{
			if(Updating)
				return;
			int width = ClientSize.Width;
			int height = Views.Count * GetStringHeight();
			if(height > ClientSize.Height)
				width -= SystemInformation.VerticalScrollBarWidth;
			AutoScrollMinSize = new Size(width, height);
		}

		protected override IView HitTest(int posX, int posY)
		{
			if(Views.Count <= 0)
				return null;
			int index = posY / GetStringHeight();
			if(index < Views.Count)
				return Views[index];
			return null;
		}

		[Browsable(false)]
		public int SelectedIndex
		{
			get
			{
				return selectedIndex;
			}
		}

		private void SetSelectedView(StringView selectedView)
		{
			if(selectedIndex >= 0)
			{
				StringView view = Views[selectedIndex] as StringView;
				view.Selected = false;
			}
			int index = Views.IndexOf(selectedView);
			if(index >= 0)
			{
				StringView view = Views[index] as StringView;
				view.Selected = true;
			}
			selectedIndex = index;
			if(selectedIndex < -1)
			{
				selectedIndex = -1;
				return;
			}
			// Visibility
			Rectangle client = new Rectangle(-AutoScrollPosition.X, -AutoScrollPosition.Y, ClientRectangle.Width, ClientRectangle.Height);
			int height = GetStringHeight();
			int dy = selectedIndex * height;
			Point origin = client.Location;
			if(dy < client.Y)
				origin.Y -= client.Y - dy;
			if(dy + height > client.Y + client.Height)
				origin.Y += (dy + height) - (client.Y + client.Height);
			if(origin.Y != client.Y)
				AutoScrollPosition = origin;
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			switch(keyData)
			{
				case Keys.Up:
					if(0 == selectedIndex)
						break;
					SetSelectedView((StringView)Views[selectedIndex - 1]);
					break;
				case Keys.Down:
					if(Views.Count - 1 == selectedIndex)
						break;
					SetSelectedView((StringView)Views[selectedIndex + 1]);
					break;
				default:
					return base.ProcessDialogKey(keyData);
			}
			return true;
		}
	}
}
