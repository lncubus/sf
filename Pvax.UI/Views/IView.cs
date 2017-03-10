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
	/// Represents a light-weight view abstraction.
	/// </summary>
	/// <remarks>
	/// A view (an object that implements this interface) can be placed either
	/// to a <see cref="ViewContainer"/> or to <see cref="CompositeView"/>.
	/// </remarks>
	public interface IView
	{
		/// <summary>
		/// Gets or sets a parent object that provides support for
		/// <see cref="IControlService"/>.
		/// </summary>
		IControlService Parent
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the horizontal coordinate of the view relative to it's
		/// parent object.
		/// </summary>
		int Left
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the vertical coordinate of the view relative to it's
		/// parent object.
		/// </summary>
		int Top
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the width of the view.
		/// </summary>
		int Width
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the height of the view.
		/// </summary>
		int Height
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the foreground color of the view.
        /// </summary>
        Color ForeColor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the background color of the view.
        /// </summary>
        Color BackColor
        {
            get;
            set;
        }

        /// <summary>
        /// Enables or disables the view.
        /// </summary>
        bool Enabled
		{
			get;
			set;
		}

		/// <summary>
		/// Shows or hides the view.
		/// </summary>
		bool Visible
		{
			get;
			set;
		}

		/// <summary>
		/// Invalidates a rectangular area specified in the view's cooridinates.
		/// </summary>
		/// <param name="x">
		/// The x-coordinate of the upper-left corner of the rectangle in
		/// the view's coordinates.
		/// </param>
		/// <param name="y">
		/// The y-coordinate of the upper-left corner of the rectangle in
		/// the view's coordinates.
		/// </param>
		/// <param name="width">
		/// The width of the view.
		/// </param>
		/// <param name="height">
		/// The height of the view.
		/// </param>
		void Invalidate(int x, int y, int width, int height);

		/// <summary>
		/// Performs the view painting.
		/// </summary>
		/// <param name="graphics">An instance of the <see cref="Graphics"/>
		/// to paint on.</param>
		void Draw(Graphics graphics);

		/// <summary>
		/// Mouse down notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		/// <param name="buttons">
		/// A <see cref="MouseButtons"/> that describes the mouse buttons
		/// pressed.
		/// </param>
		void OnMouseDown(int posX, int posY, MouseButtons buttons);

		/// <summary>
		/// Mouse up notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		/// <param name="buttons">
		/// A <see cref="MouseButtons"/> that describes the mouse buttons
		/// pressed.
		/// </param>
		void OnMouseUp(int posX, int posY, MouseButtons buttons);

		/// <summary>
		/// Mouse entered into the view's bounds notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		void OnMouseEnter(int posX, int posY);

		/// <summary>
		/// Mouse left the view's bounds notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		void OnMouseLeave(int posX, int posY);

		/// <summary>
		/// Mouse is being moved in the view's bounds notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		void OnMouseHover(int posX, int posY);

		/// <summary>
		/// Checks if the mouse is in the view's bounds.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		/// <remarks>
		/// This method is for views that are non-rectangular.
		/// </remarks>
		bool HitTest(int posX, int posY);
	}
}
