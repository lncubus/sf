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

namespace Pvax.UI.Views
{
	/// <summary>
	/// Represents a text label.
	/// </summary>
	[Serializable]
	public class LabelView: View
	{
		string text;

		/// <summary>
		/// Initializes a new instance of the <see cref="LabelView"/>.
		/// </summary>
		public LabelView():
			this(String.Empty, 0, 0, 0, 0)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="LabelView"/> with 
		/// <paramref name="text"/>.
		/// </summary>
		/// <param name="text">The text to display.</param>
		public LabelView(string text):
			this(text, 0, 0, 0, 0)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="LabelView"/> with 
		/// <paramref name="text"/> and boundaries.
		/// </summary>
		/// <param name="text">The text to display.</param>
		/// <param name="x">The horizontal coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="y">The vertical coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="width">The width of the view.</param>
		/// <param name="height">The height of the view.</param>
		public LabelView(string text, int x, int y, int width, int height):
			base(x, y, width, height)
		{
			this.text = text;
		}

		/// <summary>
		/// Gets or sets the text associated with this view.
		/// </summary>
		/// <value>
		/// The text associated with this view.
		/// </value>
		public string Text
		{
			get
			{
				return text;
			}

			set
			{
				if(text != value)
				{
					text = value;
					Invalidate();
				}
			}
		}

		private ContentAlignment textAlign;

		/// <summary>
		/// Gets or sets the alignment of text in the label.
		/// </summary>
		/// <value>
		/// One of the <see cref="ContentAlignment"/> values. The default is 
		/// <see cref="ContentAlignment.TopLeft"/>.
		/// </value>
		public ContentAlignment TextAlign
		{
			get
			{
				return textAlign;
			}

			set
			{
				if(textAlign != value)
				{
					textAlign = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Paint the label.
		/// </summary>
		/// <param name="graphics">An instance of the <see cref="Graphics"/>
		/// object to paint on.</param>
		protected override void Draw(Graphics graphics)
		{
			Brush brush = DrawHelper.Instance.CreateSolidBrush(ForeColor);
			StringFormat format = DrawHelper.Instance.CreateTypographicStringFormat(textAlign);
			graphics.DrawString(text, Parent.Control.Font, brush, new RectangleF(0, 0, Width - 1, Height - 1), format);
		}
	}
}
