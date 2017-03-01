using System;

using Pvax.UI.Views;

namespace Pvax.App.Views.TestBench
{
	/// <summary>
	/// Description of AbstractShapeView.
	/// </summary>
	public abstract class AbstractShapeView: View
	{
		public AbstractShapeView(int x, int y, int width, int height):
			base(x, y, width, height)
		{}

		protected override void OnMouseEnter(int x, int y)
		{
			Tracking = true;
		}

		protected override void OnMouseLeave(int x, int y)
		{
			Tracking = false;
		}
	}
}
