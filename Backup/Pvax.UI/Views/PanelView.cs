#region License
/*  Pvax.UI.Views
    Copyright (c) 2005, 2006, Alexey A. Popov
    All rights reserved.

    Redistribution and use in source and binary forms, with or without modification, are
    permitted provided that the following conditions are met:

    - Redistributions of source code must retain the above copyright notice, this list
      of conditions and the following disclaimer.

    - Redistributions in binary form must reproduce the above copyright notice, this list
      of conditions and the following disclaimer in the documentation and/or other materials
      provided with the distribution.

    - Neither the name of the Alexey A. Popov nor the names of its contributors may be used to
      endorse or promote products derived from this software without specific prior written
      permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS *AS IS* AND ANY EXPRESS
    OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
    AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
    CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
    DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
    DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER
    IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
    OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pvax.UI.Views
{
	/// <summary>
	/// Represents a panes-style composite view.
	/// </summary>
	[Serializable]
	public class PanelView: CompositeView
	{
		BorderStyle borderStyle;

		/// <summary>
		/// Initializes a new instance of the <see cref="PanelView"/>.
		/// </summary>
		/// <param name="x">The horizontal coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="y">The vertical coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="width">The width of the view.</param>
		/// <param name="height">The height of the view.</param>
		public PanelView(int x, int y, int width, int height):
			base(x, y, width, height)
		{}

		/// <summary>
		/// Indicates the border style for the control.
		/// </summary>
		/// <value>
		/// One of the <see cref="BorderStyle"/> values. The default is
		/// <c>BorderStyle.None</c>.
		/// </value>
		public BorderStyle BorderStyle
		{
			get
			{
				return borderStyle;
			}

			set
			{
				if(borderStyle != value)
				{
					borderStyle = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Paints the <see cref="PanelView"/> including it's subviews and
		/// borders.
		/// </summary>
		/// <param name="graphics">An instance of the <see cref="Graphics"/>
		/// object to paint on.</param>
		/// <remarks>
		/// A view can be non-rectangular.
		/// </remarks>
		protected override void Draw(Graphics graphics)
		{
			base.Draw(graphics);
			switch(borderStyle)
			{
				case BorderStyle.None:
					break;
				case BorderStyle.FixedSingle:
					graphics.DrawRectangle(SystemPens.GrayText, 0, 0, Width - 1, Height - 1);
					break;
				case BorderStyle.Fixed3D:
					graphics.DrawLine(SystemPens.ControlDark, 0, Height - 1, 0, 0);
					graphics.DrawLine(SystemPens.ControlDark, 0, 0, Width - 1, 0);
					graphics.DrawLine(SystemPens.ControlLightLight, Width - 1, 0, Width - 1, Height - 1);
					graphics.DrawLine(SystemPens.ControlLightLight, Width - 1, Height - 1, 0, Height - 1);
					break;
			}
		}
	}
}
