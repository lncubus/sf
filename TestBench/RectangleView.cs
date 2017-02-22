using System;
using System.Drawing;

using Pvax.UI;
using Pvax.UI.Views;

namespace Pvax.App.Views.TestBench
{
	/// <summary>
	/// Description of RectangleView.
	/// </summary>
	public class RectangleView: AbstractShapeView
	{
		public RectangleView(int x, int y, int width, int height):
			base(x, y, width, height)
		{}

		protected override void Draw(Graphics graphics)
		{
			Pen pen = DrawHelper.Instance.CreateColorPen((Tracking ? Color.Red : Color.Black), 1.0f);
			graphics.DrawRectangle(pen, 0.0f, 0.0f, Width - 1, Height - 1);
		}
	}
}
