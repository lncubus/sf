using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Pvax.UI;
using Pvax.UI.Views;

namespace Sample
{
    public class PowerIndicatorView : Pvax.UI.Views.View
	{
		//private 

		public PowerIndicatorView ()
		{
		}

        /// <summary>
        /// Perfoms the view painting.
        /// </summary>
        /// <param name="graphics">An instance of the <see cref="Graphics"/>
        /// object to paint on.</param>
        /// <remarks>
        /// A view can be non-rectangular.
        /// </remarks>
        protected override void Draw(Graphics graphics)
        {
        }
	}
}

