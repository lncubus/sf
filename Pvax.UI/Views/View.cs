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
using System.Windows.Forms;

namespace Pvax.UI.Views
{
	/// <summary>
	/// Represents an abstract light-weight view.
	/// </summary>
	/// <remarks>
	/// This class implements the <see cref="IView"/> interface and provides
	/// facilities that help the developer to implement various views.
	/// </remarks>
	[Serializable]
	public abstract class View: AbstractView
	{
		private int x;

		private int y;

		private int width;

		private int height;

		/// <summary>
		/// Initializes a new instance of the <see cref="View"/> class with it's
		/// desired size and location.
		/// </summary>
		/// <param name="x">The horizontal coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="y">The vertical coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="width">The width of the view.</param>
		/// <param name="height">The height of the view.</param>
		protected View(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.height = height;
			this.width = width;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="View"/> class.
		/// </summary>
		protected View():
			this(0, 0, 0, 0)
		{}

		/// <summary>
		/// Gets or sets the horizontal coordinate of the view relative to it's
		/// parent object.
		/// </summary>
		public override int Left
		{
			get
			{
				return x;
			}

			set
			{
				if(x != value)
				{
					Invalidate();
					x = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the vertical coordinate of the view relative to it's
		/// parent object.
		/// </summary>
		public override int Top
		{
			get
			{
				return y;
			}

			set
			{
				if(y != value)
				{
					Invalidate();
					y = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the width of the view.
		/// </summary>
		public override int Width
		{
			get
			{
				return width;
			}

			set
			{
				if(width != value)
				{
					Invalidate();
					width = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the height of the view.
		/// </summary>
		public override int Height
		{
			get
			{
				return height;
			}

			set
			{
				if(height != value)
				{
					Invalidate();
					height = value;
					Invalidate();
				}
			}
		}
	}
}
