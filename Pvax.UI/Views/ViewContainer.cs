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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pvax.UI.Views
{
	/// <summary>
	/// Represents a container control for light-weight views.
	/// </summary>
	[Designer(typeof(ViewContainerDesigner))]
	public class ViewContainer: Panel, IControlService
	{
		#region ViewsCollection class
		class ViewsCollection: ViewCollection
		{
			ViewContainer owner;
			
			public ViewsCollection(ViewContainer owner)
			{
				this.owner = owner;
			}
			
			public override void Add(IView view)
			{
				base.Add(view);
				view.Parent = owner;
				owner.CalcExtent();
				owner.Invalidate();
			}

            public override void AddRange(IEnumerable<IView> views)
            {
				owner.BeginUpdate();
				try
				{
					base.AddRange(views);
					foreach(IView view in views)
						view.Parent = owner;
				}
				finally
				{
					owner.EndUpdate();
				}
			}

            public override void Clear()
			{
				owner.BeginUpdate();
				try
				{
					base.Clear();
				}
				finally
				{
					owner.EndUpdate();
				}
			}
			
			public override void Insert(int index, IView view)
			{
				base.Insert(index, view);
				view.Parent = owner;
				owner.CalcExtent();
				//owner.Invalidate();
				view.Invalidate(0, 0, view.Width, view.Height);
			}
			
			public override bool Remove(IView view)
			{
				bool result = base.Remove(view);
                if (!result)
                    return false;
				view.Parent = NullControlService.Instance;
				owner.CalcExtent();
				view.Invalidate(0, 0, view.Width, view.Height);
                return true;
            }

            public override void RemoveAt(int index)
			{
				IView view = base[index];
				if(null == view)
					return;
				base.RemoveAt(index);
				view.Parent = NullControlService.Instance;
				owner.CalcExtent();
				view.Invalidate(0, 0, view.Width, view.Height);
			}
		}
		#endregion
		
		ViewCollection views;
		
		IView capturingView;
		
		IView currentTrackingView;
		
		int updating;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewContainer"/>
		/// control.
		/// </summary>
		public ViewContainer()
		{
			SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.Selectable, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.UserMouse, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			views = new ViewsCollection(this);
			AutoScroll = true;
			BorderStyle = BorderStyle.Fixed3D;
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
				return this;
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
		public void InvalidateRectangle(int x, int y, int width, int height)
		{
			if(!Created)
				return;
			Invalidate(new Rectangle(x + AutoScrollPosition.X, y + AutoScrollPosition.Y, width, height));
		}
		#endregion
		
		/// <summary>
		/// Gets a collection of all light-weight views that are handled by
		/// the <see cref="ViewContainer"/> control.
		/// </summary>
		public virtual ViewCollection Views
		{
			get
			{
				return views;
			}
		}
		
		/// <summary>
		/// Releases all managed and unmanaged resources owned by the
		/// <see cref="ViewContainer"/> object.
		/// </summary>
		/// <param name="disposing">
		/// <c>true</c> to release both managed and unmanaged resources;
		/// <c>false</c> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// Returns the flag indicating if the control is being updated.
		/// </summary>
		protected bool Updating
		{
			get
			{
				return updating > 0;
			}
		}
		
		/// <summary>
		/// Begins the update process.
		/// </summary>
		public void BeginUpdate()
		{
			updating++;
		}
		
		/// <summary>
		/// Ends the update process.
		/// </summary>
		public void EndUpdate()
		{
            if (updating <= 0)
                throw new InvalidOperationException(
                    nameof(EndUpdate) + " without " + nameof(BeginUpdate));
			updating--;
            if (!Updating)
            {
                CalcExtent();
                Invalidate();
            }
		}
		
		/// <summary>
		/// Calculates the extent (the size of the rectangle that bounds all
		/// the views held by the <see cref="ViewContainer"/> control.
		/// </summary>
		protected virtual void CalcExtent()
		{
            if(Updating || !AutoScroll)
				return;
			Point scrollPoint = AutoScrollPosition;
			scrollPoint.X = -scrollPoint.X;
			scrollPoint.Y = -scrollPoint.Y;
			Size size = new Size();
			foreach(IView view in views)
			{
				if(view.Visible)
				{
					size.Width = Math.Max(size.Width, view.Left + view.Width);
					size.Height = Math.Max(size.Height, view.Top + view.Height);
				}
			}
			AutoScrollMinSize = size;
			AutoScrollPosition = scrollPoint;
		}
		
		/// <summary>
		/// Calls <see cref="CalcExtent"/> when the native window handle is
		/// (re)created.
		/// </summary>
		/// <param name="e">
		/// An <see cref="EventArgs"/> object that contains the event data.
		/// </param>
		protected override void OnHandleCreated(System.EventArgs e)
		{
			base.OnHandleCreated(e);
			CalcExtent();
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
		protected virtual IView HitTest(int posX, int posY)
		{
			for(int i = views.Count - 1; i >= 0; i--)
			{
				IView view = views[i];
				if(view.Visible && view.HitTest(posX, posY))
				{
					return view;
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// Checks if the view is in the current visible area of the control.
		/// </summary>
		/// <param name="view">The view to test visibil</param>
		/// <returns>
		/// <c>true</c> if the <paramref name="view"/> is visible in
		/// the control; <c>flase</c> otherwise.
		/// </returns>
		protected bool IsViewVisible(IView view)
		{
			if(null == view)
				return false;
			Rectangle viewBounds = new Rectangle(view.Left, view.Top, view.Width, view.Height);
			Rectangle client = ClientRectangle;
			return client.IntersectsWith(viewBounds);
		}
		
        protected virtual IEnumerable<IView> GetDrawingViews()
        {
            return Views;
        }

		/// <summary>
		/// Perfoms the control painting.
		/// </summary>
		/// <param name="e">
		/// An <see cref="EventArgs"/> object that contains the event data.
		/// </param>
		/// <remarks>
		/// Be aware that the <see cref="ViewContainer"/> does not raise
		/// the <see cref="System.Windows.Forms.Control.Paint"/> event but
		/// performs all drawing on it's own.
		/// </remarks>
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle clip = e.ClipRectangle;
			// Cache some frequently accessed properties
			Graphics g = e.Graphics;
			// NB: here I paint the background
			g.FillRectangle(DrawHelper.Instance.CreateSolidBrush(BackColor), clip);
			Rectangle clipRect = new Rectangle(clip.X - AutoScrollPosition.X, clip.Y - AutoScrollPosition.Y, clip.Width + 1, clip.Height + 1);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;

			Rectangle bounds = new Rectangle();
			foreach(IView view in GetDrawingViews())
			{
				if(view.Visible)
				{
					bounds.X = view.Left;
					bounds.Y = view.Top;
					bounds.Width = view.Width;
					bounds.Height = view.Height;
					if(clipRect.IntersectsWith(bounds))
					{
						g.TranslateTransform(view.Left + AutoScrollPosition.X, view.Top + AutoScrollPosition.Y);
						view.Draw(g);
						g.TranslateTransform(-view.Left - AutoScrollPosition.X, -view.Top - AutoScrollPosition.Y);
					}
				}
			}
		}
		
		/// <summary>
		/// Translates the mouse down event to <see cref="IView.OnMouseDown"/>
		/// call.
		/// </summary>
		/// <param name="e">
		/// A <see cref="MouseEventArgs"/> object that contains the event data.
		/// </param>
		/// <remarks>
		/// Be aware that the <see cref="ViewContainer"/> does not raise
		/// the <see cref="System.Windows.Forms.Control.MouseDown"/> event but
		/// performs all mouse processing on it's own.
		/// </remarks>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			int x = e.X - AutoScrollPosition.X;
			int y = e.Y - AutoScrollPosition.Y;
			IView view = HitTest(x, y);
			if(null == view)
				return;
			if(view.Enabled)
			{
				capturingView = view;
				Capture = true;
				view.OnMouseDown(x - view.Left, y - view.Top, e.Button);
			}
		}
		
		/// <summary>
		/// Translates the mouse up event to <see cref="IView.OnMouseUp"/>
		/// call. In addition handles mouse capture.
		/// </summary>
		/// <param name="e">
		/// A <see cref="MouseEventArgs"/> object that contains the event data.
		/// </param>
		/// <remarks>
		/// Be aware that the <see cref="ViewContainer"/> does not raise
		/// the <see cref="System.Windows.Forms.Control.MouseUp"/> event but
		/// performs all mouse processing on it's own.
		/// </remarks>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			IView view;
			if(null == capturingView)
				return;
			view = capturingView;
			int x = e.X - AutoScrollPosition.X;
			int y = e.Y - AutoScrollPosition.Y;
			if(view.Enabled)
				view.OnMouseUp(x - view.Left, y - view.Top, e.Button);
			capturingView = null;
			Capture = false;
		}
		
		/// <summary>
		/// Translates the mouse move event to <see cref="IView.OnMouseHover"/>,
		/// <see cref="IView.OnMouseEnter"/> or <see cref="IView.OnMouseLeave"/>
		/// calls. In addition handles mouse capture.
		/// </summary>
		/// <param name="e">
		/// A <see cref="MouseEventArgs"/> object that contains the event data.
		/// </param>
		/// <remarks>
		/// Be aware that the <see cref="ViewContainer"/> does not raise
		/// the <see cref="System.Windows.Forms.Control.MouseMove"/> event but
		/// performs all mouse processing on it's own.
		/// </remarks>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			int x = e.X - AutoScrollPosition.X;
			int y = e.Y - AutoScrollPosition.Y;
			IView view = HitTest(x, y);
			if(view == currentTrackingView)
			{
				if(null != view)
					view.OnMouseHover(x - view.Left, y - view.Top);
				return;
			}
			if(null != currentTrackingView && currentTrackingView.Enabled)
			{
				currentTrackingView.OnMouseLeave(x - currentTrackingView.Left, y - currentTrackingView.Top);
			}
			if(null != view && view.Enabled)
			{
				view.OnMouseEnter(x - view.Left, y - view.Top);
			}
			currentTrackingView = view;
		}
		
		/// <summary>
		/// Translates the mouse move event to <see cref="IView.OnMouseLeave"/>
		/// call. In addition handles mouse capture.
		/// </summary>
		/// <param name="e">
		/// A <see cref="MouseEventArgs"/> object that contains the event data.
		/// </param>
		/// <remarks>
		/// Be aware that the <see cref="ViewContainer"/> does not raise
		/// the <see cref="System.Windows.Forms.Control.MouseLeave"/> event but
		/// performs all mouse processing on it's own.
		/// </remarks>
		protected override void OnMouseLeave(System.EventArgs e)
		{
			if(null != currentTrackingView)
			{
				if(currentTrackingView.Enabled)
				{
					currentTrackingView.OnMouseLeave(0, 0);
				}
				currentTrackingView = null;
			}
		}
		
		/// <summary>
		/// Should raise <see cref="System.Windows.Forms.Control.MouseEnter"/>
		/// event.
		/// </summary>
		/// <param name="e">
		/// An EventArgs that contains the event data.
		/// </param>
		/// <remarks>
		/// This class performs custom mouse processing and does not raise
		/// the propert event.
		/// </remarks>
		protected override void OnMouseEnter(System.EventArgs e)
		{}
		
		/// <summary>
		/// Should raise <see cref="System.Windows.Forms.Control.MouseHover"/>
		/// event.
		/// </summary>
		/// <param name="e">
		/// An EventArgs that contains the event data.
		/// </param>
		/// <remarks>
		/// This class performs custom mouse processing and does not raise
		/// the propert event.
		/// </remarks>
		protected override void OnMouseHover(System.EventArgs e)
		{}
		
		/// <summary>
		/// Should raise the
		/// <see cref="System.Windows.Forms.Control.MouseWheel"/> event.
		/// </summary>
		/// <param name="e">
		/// A <see cref="MouseEventArgs"/> that contains the event data.
		/// </param>
		/// <remarks>
		/// This class performs custom mouse processing and does not raise
		/// the propert event.
		/// </remarks>
		protected override void OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
		{}
		
		static bool Between(int value, int minimum, int maximum)
		{
			return ((value >= minimum) && (value < maximum));
		}
		
		static int GetLineSize(int extent)
		{
			if(Between(extent, 0, 100))
				return 1;
			if(Between(extent, 100, 1000))
				return 10;
			if(Between(extent, 1000, 10000))
				return 100;
			return 1000;
		}
		
		static int GetPageSize(int extent)
		{
			return 10 * GetLineSize(extent);
		}
	}
}
