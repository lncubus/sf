using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Pvax.UI;
using Pvax.UI.Views;

namespace Pvax.App.Views.TestBench
{
	/// <summary>
	/// Description of EllipseView.
	/// </summary>
	public class EllipseView: AbstractShapeView
	{
		Color color;

		public EllipseView(Color color, int x, int y, int width, int height):
			base(x, y, width, height)
		{
			this.color = color;
		}

		protected override void Draw(Graphics graphics)
		{
			Brush brush = DrawHelper.Instance.CreateSolidBrush((Tracking ? Color.Red : this.color));
			graphics.FillEllipse(brush, 0.0f, 0.0f, Width - 1, Height - 1);
		}

		protected override bool HitTest(int posX, int posY)
		{
			bool result;
			using(GraphicsPath path = new GraphicsPath())
			{
				path.AddEllipse(X, Y, Width - 1, Height - 1);
				result = path.IsVisible(posX, posY);
			}
			return result;
		}

		protected override void OnMouseUp(int posX, int posY, MouseButtons buttons)
		{
			if(buttons == MouseButtons.Left)
				this.Parent.Control.Views.MoveFront(this);
		}
	}
}
