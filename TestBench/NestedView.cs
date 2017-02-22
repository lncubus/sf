using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Pvax.UI.Views;

namespace Pvax.App.Views.TestBench
{
	/// <summary>
	/// Description of NestedView.
	/// </summary>
	public class NestedView: CompositeView
	{
		string text;

		public NestedView(string text, int x, int y, int width, int height):
			base(x, y, width, height)
		{
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

		protected override void Draw(Graphics graphics)
		{
			base.Draw(graphics);
			Font font = this.Control.Font;
			graphics.DrawString(text, font, Brushes.Black, 1, 1);
			graphics.DrawRectangle(Tracking ? Pens.Red : Pens.Black, 0, 0, Width - 1, Height - 1);
		}

		protected override void OnMouseEnter(int x, int y)
		{
			base.OnMouseEnter(x, y);
			Tracking = true;
		}

		protected override void OnMouseLeave(int x, int y)
		{
			base.OnMouseLeave(x, y);
			Tracking = false;
		}

		protected override void OnMouseHover(int posX, int posY)
		{
			base.OnMouseHover(posX, posY);
			Trace.WriteLine(String.Format("{0}: {1}, {2}", text, posX, posY));
		}
	}
}
