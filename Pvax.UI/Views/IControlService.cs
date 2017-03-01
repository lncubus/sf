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
	/// Provides access to the parent control (<see cref="ViewContainer"/>) for
	/// views that inserted to other views.
	/// </summary>
	public interface IControlService
	{
		/// <summary>
		/// Gets the <see cref="ViewContainer"/> that keeps the view.
		/// </summary>
		/// <value>
		/// The <see cref="ViewContainer"/> that keeps the view.
		/// </value>
		ViewContainer Control
		{
			get;
		}

		/// <summary>
		/// Invalidates a rectangular area area of the control.
		/// </summary>
		/// <param name="x">
		/// The x-coordinate of the upper-left corner of the rectangle.
		/// </param>
		/// <param name="y">
		/// The y-coordinate of the upper-left corner of the rectangle.
		/// </param>
		/// <param name="width">
		/// The width of the view.
		/// </param>
		/// <param name="height">
		/// The height of the view.
		/// </param>
		void InvalidateRectangle(int x, int y, int width, int height);
	}
}
