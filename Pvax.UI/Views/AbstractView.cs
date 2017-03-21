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
using System.Diagnostics;


#endregion

using System;
using System.Drawing;
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
	public abstract class AbstractView: IView
	{
		/// <summary>
		/// Declares bits for the <see cref="View"/>'s internal state.
		/// </summary>
		[Flags]
		[Serializable]
		private enum StateBit
		{
			Enabled = 1,

			Visible = 2,

			Active = 4,

			Selected = 8,

			Tracking = 16,

			Pressed = 32,

			Focused = 64
		}

		private IControlService parent;

		/// <summary>
		/// Keeps the <see cref="View"/>'s state.
		/// </summary>
		private StateBit state;

        /// <summary>
        /// Foreground Color
        /// </summary>
        private Color foreColor = Color.Empty;

        /// <summary>
        /// Background Color
        /// </summary>
        private Color backColor = Color.Empty;

		/// <summary>
		/// Hovering Color.
		/// </summary>
		private Color hoverColor = Color.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="View"/> class with it's
        /// desired size and location.
        /// </summary>
        protected AbstractView()
		{
			this.parent = NullControlService.Instance;
			state = StateBit.Enabled | StateBit.Visible;
		}

		/// <summary>
		/// Gets or sets a parent object that provides support for
		/// <see cref="IControlService"/>.
		/// </summary>
		public virtual IControlService Parent
		{
			get
			{
				return parent;
			}

			set
			{
				if(null == value)
					parent = NullControlService.Instance;
				else
					parent = value;
			}
		}

        public virtual Font Font
        {
            get
            {
                if (Parent == null || Parent.Control == null)// || Parent.Control.
                    return null;
                return Parent.Control.Font;
            }
        }

		/// <summary>
		/// Gets or sets the horizontal coordinate of the view relative to it's
		/// parent object.
		/// </summary>
		public abstract int Left
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the vertical coordinate of the view relative to it's
		/// parent object.
		/// </summary>
		public abstract int Top
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the width of the view.
		/// </summary>
		public abstract int Width
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the height of the view.
		/// </summary>
		public abstract int Height
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the foreground color of the view.
        /// </summary>
        public virtual Color ForeColor
        {
            get
            {
                if (foreColor != Color.Empty)
                    return foreColor;
                if ((null == Parent.Control) || !Parent.Control.Created)
                    return SystemColors.ControlText;
                return Parent.Control.ForeColor;
            }
            set
            {
                if (foreColor != value)
                {
                    foreColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the background color of the view.
        /// </summary>
        public virtual Color BackColor
        {
            get
            {
                if (backColor != Color.Empty)
                    return backColor;
                if ((null == Parent.Control) || !Parent.Control.Created)
                    return SystemColors.Control;
                return Parent.Control.BackColor;
            }
            set
            {
                if (backColor != value)
                {
                    backColor = value;
                    Invalidate();
                }
            }
        }

		/// <summary>
		/// Gets or sets the hovered background color of the view.
		/// </summary>
		public virtual Color HoverColor
		{
			get
			{
				if (hoverColor != Color.Empty)
					return hoverColor;
				return BackColor;
			}
			set
			{
				if (hoverColor != value)
				{
					hoverColor = value;
					Invalidate();
				}
			}
		}

		/// <summary>
        /// Enables or disables the view.
        /// </summary>
        public bool Enabled
		{
			get
			{
				return StateBit.Enabled == (state & StateBit.Enabled);
			}

			set
			{
				bool oldValue = this.Enabled;
				if(oldValue == value)
					return;
				if(value)
					state |= StateBit.Enabled;
				else
					state &= ~(StateBit.Enabled);
				Invalidate();
			}
		}

		/// <summary>
		/// Shows or hides the view.
		/// </summary>
		public bool Visible
		{
			get
			{
				return StateBit.Visible == (state & StateBit.Visible);
			}

			set
			{
				bool oldValue = this.Visible;
				if(oldValue == value)
					return;
				if(value)
					state |= StateBit.Visible;
				else
					state &= ~(StateBit.Visible);
				Invalidate();
			}
		}

		/// <summary>
		/// Activates or deactivates the view.
		/// </summary>
		/// <value>
		/// <c>true</c> is the view is active and <c>flase</c> otherwise.
		/// </value>
		/// <remarks>
		/// This property is an extension of the <see cref="IView"/> interface.
		/// It's up to the developer to interprete this flag.
		/// </remarks>
		public bool Active
		{
			get
			{
				return StateBit.Active == (state & StateBit.Active);
			}

			set
			{
				bool oldValue = this.Active;
				if(oldValue == value)
					return;
				if(value)
					state |= StateBit.Active;
				else
					state &= ~(StateBit.Active);
				Invalidate();
			}
		}

		/// <summary>
		/// Selects or deselects the view.
		/// </summary>
		/// <value>
		/// <c>true</c> is the view is selected and <c>flase</c> otherwise.
		/// </value>
		/// <remarks>
		/// This property is an extension of the <see cref="IView"/> interface.
		/// It's up to the developer to interprete this flag.
		/// </remarks>
		public bool Selected
		{
			get
			{
				return StateBit.Selected == (state & StateBit.Selected);
			}

			set
			{
				bool oldValue = this.Selected;
				if(oldValue == value)
					return;
				if(value)
					state |= StateBit.Selected;
				else
					state &= ~(StateBit.Selected);
				Invalidate();
			}
		}

		/// <summary>
		/// Keeps the tracking state of the view
		/// </summary>
		/// <value>
		/// <c>true</c> is the view is in tracking mode (think of hyperlink
		/// being underlined when the mouse points at it) and <c>flase</c>
		/// otherwise.
		/// </value>
		/// <remarks>
		/// This property is an extension of the <see cref="IView"/> interface.
		/// It's up to the developer to interprete this flag.
		/// </remarks>
		/// <seealso cref="OnMouseEnter"/>
		/// <seealso cref="OnMouseHover"/>
		/// <seealso cref="OnMouseLeave"/>
		public bool Tracking
		{
			get
			{
				return StateBit.Tracking == (state & StateBit.Tracking);
			}

			set
			{
				bool oldValue = this.Tracking;
				if(oldValue == value)
					return;
				if(value)
					state |= StateBit.Tracking;
				else
					state &= ~(StateBit.Tracking);
				Invalidate();
			}
		}

		/// <summary>
		/// Presses or releases the view with mouse.
		/// </summary>
		/// <value>
		/// <c>true</c> is the view is pressed and <c>flase</c> otherwise.
		/// </value>
		/// <remarks>
		/// This property is an extension of the <see cref="IView"/> interface.
		/// It's up to the developer to interprete this flag.
		/// </remarks>
		public bool Pressed
		{
			get
			{
				return StateBit.Pressed == (state & StateBit.Pressed);
			}

			set
			{
				bool oldValue = this.Pressed;
				if(oldValue == value)
					return;
				if(value)
					state |= StateBit.Pressed;
				else
					state &= ~(StateBit.Pressed);
				Invalidate();
			}
		}

		/// <summary>
		/// Sets focus to or removes focus from the view with mouse.
		/// </summary>
		/// <value>
		/// <c>true</c> is the view has focus and <c>flase</c> otherwise.
		/// </value>
		/// <remarks>
		/// This property is an extension of the <see cref="IView"/> interface.
		/// It's up to the developer to interprete this flag.
		/// </remarks>
		public bool Focused
		{
			get
			{
				return StateBit.Focused == (state & StateBit.Focused);
			}

			set
			{
				bool oldValue = this.Focused;
				if(oldValue == value)
					return;
				if(value)
					state |= StateBit.Focused;
				else
					state &= ~(StateBit.Focused);
				Invalidate();
			}
		}

		/// <summary>
		/// Gets the view location - it's top left corner coordinates.
		/// </summary>
		public virtual Point Location
		{
			get
			{
				return new Point(Left, Top);
			}
		}

		/// <summary>
		/// Gets the view size.
		/// </summary>
		public virtual Size Size
		{
			get
			{
				return new Size(Width, Height);
			}
		}

		/// <summary>
		/// Gets the bounds of the view relative to it's parent object.
		/// </summary>
		public virtual Rectangle Bounds
		{
			get
			{
				return new Rectangle(0, 0, Width, Height);
			}
		}

		/// <summary>
		/// Invalidates the view's contents.
		/// </summary>
		public void Invalidate()
		{
			Invalidate(0, 0, Width, Height);
		}

		/// <summary>
		/// Invalidates a rectangular area specified in the view's cooridinates.
		/// </summary>
		/// <param name="x">
		/// The x-coordinate of the upper-left corner of the view.
		/// </param>
		/// <param name="y">
		/// The y-coordinate of the upper-left corner of the view.
		/// </param>
		/// <param name="width">
		/// The width of the view.
		/// </param>
		/// <param name="height">
		/// The height of the view.
		/// </param>
		public virtual void Invalidate(int x, int y, int width, int height)
		{
			if((null == Parent.Control) || !Parent.Control.Created)
				return;
			Parent.InvalidateRectangle(x + Left, y + Top, width, height);
		}

		/// <summary>
		/// Override this method in derived classes to paint the views.
		/// </summary>
		/// <param name="graphics">An instance of the <see cref="Graphics"/>
		/// object to paint on.</param>
		/// <remarks>
		/// A view can be non-rectangular.
		/// </remarks>
		protected abstract void Draw(Graphics graphics);

		void IView.Draw(Graphics graphics)
		{
			this.Draw(graphics);
		}

		/// <summary>
		/// Mouse down notification callback.
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
		protected virtual void OnMouseDown(int posX, int posY, MouseButtons buttons)
		{}

		void IView.OnMouseDown(int posX, int posY, MouseButtons buttons)
		{
			this.OnMouseDown(posX, posY, buttons);
		}

		/// <summary>
		/// Mouse up notification callback.
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
		protected virtual void OnMouseUp(int posX, int posY, MouseButtons buttons)
		{}

		void IView.OnMouseUp(int posX, int posY, MouseButtons buttons)
		{
			this.OnMouseUp(posX, posY, buttons);
		}

		/// <summary>
		/// Mouse entered into the view's bounds notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		protected virtual void OnMouseEnter(int posX, int posY)
		{}

		void IView.OnMouseEnter(int posX, int posY)
		{
			this.OnMouseEnter(posX, posY);
		}

		/// <summary>
		/// Mouse left the view's bounds notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		protected virtual void OnMouseLeave(int posX, int posY)
		{}

		void IView.OnMouseLeave(int posX, int posY)
		{
			this.OnMouseLeave(posX, posY);
		}

		/// <summary>
		/// Mouse is being moved in the view's bounds notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		protected virtual void OnMouseHover(int posX, int posY)
		{}

		void IView.OnMouseHover(int posX, int posY)
		{
			this.OnMouseHover(posX, posY);
		}

		/// <summary>
		/// Checks if the mouse is in the view's bounds.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		protected virtual bool HitTest(int posX, int posY)
		{
			if((Left <= posX) && (Left + Width > posX) && (Top <= posY) && (Top + Height > posY))
				return true;
			return false;
		}

		bool IView.HitTest(int posX, int posY)
		{
			return this.HitTest(posX, posY);
		}

        public string Name { get; set; }
	}
}
