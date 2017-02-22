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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Pvax.UI;

namespace Pvax.UI.Views
{
	/// <summary>
	/// Represents a light-weight view that hosts other light-weight views.
	/// </summary>
	[Serializable]
	public class CompositeView: View, IControlService
	{
		#region ViewsCollection private class
		[Serializable]
		private class ViewsCollection: ViewCollection
		{
			private CompositeView owner;

			public ViewsCollection(CompositeView owner)
			{
				this.owner = owner;
			}

			public override void Add(IView view)
			{
				base.Add(view);
				view.Parent = owner;
			}

			public override void AddRange(IEnumerable <IView> views)
			{
				base.AddRange(views);
				foreach(IView view in views)
					view.Parent = owner;
			}

			public override void Insert(int index, IView view)
			{
				base.Insert(index, view);
				view.Parent = owner;
			}

			public override bool Remove(IView view)
			{
				bool result = base.Remove(view);
                if (result)
                    view.Parent = NullControlService.Instance;
                return result;
			}

			public override void RemoveAt(int index)
			{
				IView view = base[index];
				if(null == view)
					return;
				base.RemoveAt(index);
				view.Parent = NullControlService.Instance;
			}
		}
		#endregion

		private ViewCollection views;

		private IView capturingView;

		private IView currentTrackingView;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeView"/> class.
		/// </summary>
		/// <param name="x">The horizontal coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="y">The vertical coordinate of the view relative to
		/// it's parent object.</param>
		/// <param name="width">The width of the view.</param>
		/// <param name="height">The height of the view.</param>
		public CompositeView(int x, int y, int width, int height):
			base(x, y, width, height)
		{
			this.views = new ViewsCollection(this);
		}

		#region IControlService interface implementation
		/// <summary>
		/// Gets the <see cref="ViewContainer"/> that keeps the view.
		/// </summary>
		/// <value>
		/// The <see cref="ViewContainer"/> that keeps the view.
		/// </value>
		public virtual ViewContainer Control
		{
			get
			{
				return Parent.Control;
			}
		}

		/// <summary>
		/// Invalidates a rectangular area area of the view.
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
		public virtual void InvalidateRectangle(int x, int y, int width, int height)
		{
//			rectangle.Offset(X, Y);
//			Parent.InvalidateRectangle(rectangle);
			Parent.InvalidateRectangle(x + X, y + Y, width, height);
		}
		#endregion

		/// <summary>
		/// Gets a collection of all light-weight views that are handled by
		/// the <see cref="ViewContainer"/> control.
		/// </summary>
		public ViewCollection Views
		{
			get
			{
				return views;
			}
		}

		/// <summary>
		/// Retrieves the <see cref="IView"/> object at the specified control
		/// coordinates.
		/// </summary>
		/// <param name="posX">The horizontal coordinate.</param>
		/// <param name="posY">The vertical coordinate.</param>
		/// <returns>
		/// An <see cref="IView"/> the mouse points at or <c>null</c> if
		/// the mouse does not point at a view.
		/// </returns>
		public IView HitTest2(int posX, int posY)
		{
			for(int i = views.Count - 1; i >= 0; i--)
			{
				IView view = views[i];
				if((null != view) && view.Visible && view.HitTest(posX, posY))
				{
						return view;
				}
			}

			return null;
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
			foreach(IView view in views)
			{
				if(view.Visible)
				{
					graphics.TranslateTransform(view.X, view.Y);
					view.Draw(graphics);
					graphics.TranslateTransform(-view.X, -view.Y);
				}
			}
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
		protected override void OnMouseDown(int posX, int posY, MouseButtons buttons)
		{
			IView view = HitTest2(posX, posY);
			if((null != view) && view.Enabled)
			{
				capturingView = view;
				view.OnMouseDown(posX - X, posY - Y, buttons);
			}
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
		protected override void OnMouseUp(int posX, int posY, MouseButtons buttons)
		{
			if(null == capturingView)
				return;
			IView view = capturingView;
			if((null != view) && view.Enabled)
			{
				capturingView = null;
				view.OnMouseUp(posX - X, posY - Y, buttons);
			}
		}

		/// <summary>
		/// Mouse is being moved in the view's bounds notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		protected override void OnMouseHover(int posX, int posY)
		{
			IView view = HitTest2(posX, posY);
			if(view == currentTrackingView)
			{
				if(null != view)
					view.OnMouseHover(posX - view.X, posY - view.Y);
				return;
			}
			if(null != currentTrackingView)
			{
				if(currentTrackingView.Enabled)
				{
					currentTrackingView.OnMouseLeave(posX - currentTrackingView.X, posY - currentTrackingView.Y);
					currentTrackingView.Invalidate(0, 0, currentTrackingView.Width, currentTrackingView.Height);
				}
			}
			if(null != view)
			{
				if(view.Enabled)
				{
					view.OnMouseEnter(posX - view.X, posY - view.Y);
					view.Invalidate(0, 0, view.Width, view.Height);
				}
			}
			currentTrackingView = view;
		}

		/// <summary>
		/// Mouse left the view's bounds notification callback.
		/// </summary>
		/// <param name="posX">The x-coordinate of the mouse.</param>
		/// <param name="posY">The y-coordinate of the mouse.</param>
		protected override void OnMouseLeave(int posX, int posY)
		{
			if(null != currentTrackingView)
			{
				if(currentTrackingView.Enabled)
				{
					currentTrackingView.OnMouseLeave(posX - currentTrackingView.X, posY - currentTrackingView.Y);
					currentTrackingView.Invalidate(0, 0, currentTrackingView.Width, currentTrackingView.Height);
				}
				currentTrackingView = null;
			}
		}

	}
}
