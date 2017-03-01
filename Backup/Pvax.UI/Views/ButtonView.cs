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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pvax.UI.Views
{
	/// <summary>
	/// Represents a light-weight button control.
	/// </summary>
	[Serializable]
	public class ButtonView: View
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonView"/>.
		/// </summary>
		/// <param name="x">The horizontal coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="y">The vertical coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="width">The width of the view.</param>
		/// <param name="height">The height of the view.</param>
		public ButtonView(int x, int y, int width, int height):
			base(x, y, width, height)
		{
			textAlign = ContentAlignment.MiddleCenter;
		}

		private string text;

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
		/// Occurs when the control is clicked.
		/// </summary>
		public event EventHandler Click;

		/// <summary>
		/// Raises the <see cref="Click"/> event.
		/// </summary>
		/// <param name="e">
		/// An <see cref="EventArgs"/> that contains the event data.
		/// </param>
		protected virtual void OnClick(EventArgs e)
		{
			EventHandler handler = Click;
			if(null != handler)
				handler(this, e);
		}

		/// <summary>
		/// Paints the button in the proper state.
		/// </summary>
		/// <param name="graphics">An instance of the <see cref="Graphics"/>
		/// object to paint on.</param>
		protected override void Draw(Graphics graphics)
		{
			ButtonState state = ButtonState.Normal;
			if(Active)
				state = ButtonState.Pushed;
			ControlPaint.DrawButton(graphics, new Rectangle(0, 0, Width, Height), state);
			StringFormat format = DrawHelper.Instance.CloneTypographicStringFormat();
			switch(textAlign)
			{
				case ContentAlignment.BottomCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.BottomLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.BottomRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Far;
					break;
				case ContentAlignment.MiddleCenter:
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.MiddleRight:
					format.Alignment = StringAlignment.Far;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.TopCenter:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Center;
					break;
				case ContentAlignment.TopLeft:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Near;
					break;
				case ContentAlignment.TopRight:
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Far;
					break;
			}

			if(!Enabled)
			{
				ControlPaint.DrawStringDisabled(graphics, text, Parent.Control.Font, SystemColors.ControlLight, new RectangleF(0, 0, Width - 1, Height - 1), format);
				return;
			}

			Rectangle rect = new Rectangle(2, 2, Width - 5, Height - 5);
			if(Active)
				rect.Offset(1, 1);
			graphics.DrawString(text, Parent.Control.Font, DrawHelper.Instance.CreateSolidBrush(Parent.Control.ForeColor), rect, format);
		}

		/// <summary>
		/// Handles mouse when it enters the button and changes the view's state
		/// appropriately.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		protected override void OnMouseEnter(int posX, int posY)
		{
			Tracking = true;
			if(Pressed)
				Active = true;
		}

		/// <summary>
		/// Handles mouse when it leaves the button and changes the view's state
		/// appropriately.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		protected override void OnMouseLeave(int posX, int posY)
		{
			Tracking = false;
			Active = false;
		}

		/// <summary>
		/// Handles mouse down.
		/// </summary>
		/// <param name="posX">
		/// The horizontal position of the mouse.
		/// </param>
		/// <param name="posY">
		/// The vertical position of the mouse.
		/// </param>
		/// <param name="buttons">
		/// A <see cref="MouseButtons"/> that describes the mouse buttons
		/// pressed.
		/// </param>
		protected override void OnMouseDown(int posX, int posY, MouseButtons buttons)
		{
			Pressed = true;
			Active = true;
		}

		/// <summary>
		/// Handles mouse up and, optionally, raises <see cref="Click"/> event.
		/// </summary>
		/// <param name="posX">
		/// The horizontal position of the mouse.
		/// </param>
		/// <param name="posY">
		/// The vertical position of the mouse.
		/// </param>
		/// <param name="buttons">
		/// A <see cref="MouseButtons"/> that describes the mouse buttons
		/// pressed.
		/// </param>
		/// <event cref="Click">Can be raised by this method.</event>
		protected override void OnMouseUp(int posX, int posY, MouseButtons buttons)
		{
			Pressed = false;
			Active = false;
			OnClick(EventArgs.Empty);
		}
	}
}
