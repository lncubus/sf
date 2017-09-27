using System;
using System.Drawing;
using System.Linq;

using Pvax.UI;
using System.Drawing.Drawing2D;

namespace Sample
{
	public class IconView : Pvax.UI.Views.View, IDisposable
    {
        protected Vectors.Vector3 _vector;
        protected SizeF _size = SizeF.Empty;
        protected int _z;
        protected string _text;
		private Bitmap _cached = null;
		private int _cachedWidth = -1;
		private int _cachedHeight = -1;
		private Color _cachedFillColor = Color.Empty;
		private Color _cachedEdgeColor = Color.Empty;

		public void Dispose()
		{
			ClearCache();
		}

		protected void ClearCache()
		{
			_cachedHeight = -1;
			_cachedWidth = -1;
			_cachedFillColor = Color.Empty;
			_cachedEdgeColor = Color.Empty;
			if (_cached != null)
			{
				_cached.Dispose ();
				_cached = null;
			}
		}

		protected bool IsCacheOkay(Color fill, Color edge)
		{
			return _cached != null &&
				_cachedHeight == Height && _cachedWidth == Width &&
				_cachedFillColor == fill && _cachedEdgeColor == edge;
		}

        /// <summary>
        /// Gets or sets the text associated with this view.
        /// </summary>
        /// <value>
        /// The text associated with this view.
        /// </value>
        public virtual string Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (_text != value)
                {
                    _text = value;
                    Invalidate();
                }
            }
        }

        public virtual int Z
        {
            get
            {
                return _z;
            }
            set
            {
                if (_z == value)
                    return;
                _z = value;
                SpaceView parent = Parent.Control as SpaceView;
                if (parent != null)
                    Invalidate();
            }
        }

        public virtual Vectors.Vector3 Vector
        {
            get
            {
                return _vector;
            }
            set
            {
                if (_vector.Equals(value))
                    return;
                _vector = value;
                SpaceView parent = Parent.Control as SpaceView;
                if (parent != null)
                    parent.UpdateLayout(this);
            }
        }
        
        public virtual SizeF IconSize
        {
            get
            {
                if (!_size.IsEmpty)
                    return _size;
                return new SizeF(2 * Width / SpaceView.Dpi.X, 2 * Height / SpaceView.Dpi.Y);
            }
            set
            {
                if (_size.Equals(value))
                    return;
                _size = value;
                Width = (int)(_size.Width * SpaceView.Dpi.X / 2);
                Height = (int)(_size.Height * SpaceView.Dpi.Y / 2);
                SpaceView parent = Parent.Control as SpaceView;
                if (parent != null)
                    parent.UpdateLayout(this);
            }
        }

        public IconView() : base()
        {
        }

        private Color edgeColor = Color.Empty;

        /// <summary>
        /// Gets or sets the border color of the symbol.
        /// </summary>
        public virtual Color EdgeColor
        {
            get
            {
                if (edgeColor != Color.Empty)
                    return edgeColor;
                return ForeColor;
            }
            set
            {
                if (edgeColor != value)
                {
                    edgeColor = value;
                    Invalidate();
                }
            }
        }

        private Symbol symbol;
        public virtual Symbol Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                if (symbol != value)
                {
                    symbol = value;
                    Invalidate();
                }
            }
        }

        private GraphicsPath customSymbol;
        private GraphicsPath cachedCustomSymbol;

        public virtual GraphicsPath CustomSymbol
        {
            get
            {
                return customSymbol; 
            }
            set
            {
                if (ReferenceEquals(customSymbol, value))
                    return;
                customSymbol = value;
                if (cachedCustomSymbol != null)
                    cachedCustomSymbol.Dispose();
                cachedCustomSymbol = null;
                _cached = null;
                Invalidate();
            }
        }

        private void BuildCustomSymbol()
        {
            if (customSymbol == null)
            {
                cachedCustomSymbol = new GraphicsPath();
                cachedCustomSymbol.AddString(Text, Font.FontFamily, 0, 72,
                    Point.Empty, StringFormat.GenericTypographic);
            }
            else
            {
                cachedCustomSymbol = (GraphicsPath)customSymbol.Clone();
            }
            RectangleF customBounds = cachedCustomSymbol.GetBounds();
            RectangleF viewBounds = Bounds;
            viewBounds.Inflate(-2, -2);
            float widthRatio = viewBounds.Width / customBounds.Width;
            float heightRatio = viewBounds.Height / customBounds.Height;
            if (widthRatio > heightRatio)
            {
                float delta = (viewBounds.Width - customBounds.Width * heightRatio) / 2;
                viewBounds.Inflate(-delta, 0);
            }
            else if (widthRatio < heightRatio)
            {
                float delta = (viewBounds.Height - customBounds.Height * widthRatio) / 2;
                viewBounds.Inflate(0, -delta);
            }

            PointF topRight = new PointF
            {
                X = viewBounds.Right,
                Y = viewBounds.Top,
            };
            PointF bottomLeft = new PointF
            {
                X = viewBounds.Left,
                Y = viewBounds.Bottom,
            };
            Matrix transform = new Matrix(customBounds,
                new[] { viewBounds.Location, topRight, bottomLeft});
            cachedCustomSymbol.Transform(transform);
        }

		protected virtual void DrawCache(Color fill, Color edge)
		{
			ClearCache();
			int width = Width;
			int height = Height;
			Bitmap cache = new Bitmap(width, height);
			Brush brush = DrawHelper.Instance.CreateSolidBrush(fill);
			Pen pen = DrawHelper.Instance.CreateColorPen(edge, 1.2F);
			Rectangle layout = new Rectangle(0, 0, width, height);
			layout.Inflate(-2, -2);
			using (Graphics g = Graphics.FromImage(cache))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;
				switch (Symbol)
				{
				case Symbol.Text:
					StringFormat format = DrawHelper.Instance.CreateTypographicStringFormat(ContentAlignment.MiddleCenter);
					g.DrawString(Text, Font, brush, layout, format);
					break;
				case Symbol.Rectangle:
					g.FillRectangle(brush, layout);
					g.DrawRectangle(pen, layout);
					break;
				case Symbol.Ellipse:
					g.FillEllipse(brush, layout);
					g.DrawEllipse(pen, layout);
					break;
				case Symbol.Custom:
					//g.DrawRectangle(pen, layout);
					if (cachedCustomSymbol == null)
						BuildCustomSymbol();
					g.FillPath(brush, cachedCustomSymbol);
					g.DrawPath(pen, cachedCustomSymbol);
					break;
				case Symbol.Quatrefoil:
					ArcF[] arcs = SymbolHelper.Flowers[Symbol].ToArray();
					using (GraphicsPath flower = new GraphicsPath())
					{
						for (int i = 0; i < arcs.Length; i++)
						{
							ArcF a = arcs[i];
							a.X = layout.Left + a.X * layout.Width;
							a.Y = layout.Top + a.Y * layout.Height;
							a.Width *= layout.Width;
							a.Height *= layout.Height;
							flower.AddArc(a.X, a.Y, a.Width, a.Height, a.StartAngle, a.SweepAngle);
						}
						g.FillPath(brush, flower);
						g.DrawPath(pen, flower);
					}
					break;
				default :
					if (!SymbolHelper.Polygons.ContainsKey(Symbol))
						throw new NotImplementedException(Symbol.ToString());
					PointF[] points = SymbolHelper.Polygons[Symbol].ToArray();
					for (int i = 0; i < points.Length; i++)
					{
						PointF p = points[i];
						p.X = layout.Left + p.X * layout.Width;
						p.Y = layout.Top + p.Y * layout.Height;
						points[i] = p;
					}
					g.FillPolygon(brush, points, FillMode.Winding);
					g.DrawPolygon(pen, points);
					break;
				}
			}
			Tl.Log(string.Format("Rendered {0} {1}x{2} {3} {4}", Name, width, height, fill, edge));
			_cached = cache;
			_cachedEdgeColor = edge;
			_cachedFillColor = fill;
			_cachedWidth = width;
			_cachedHeight = height;
		}

        /// <summary>
        /// Paint the symbol.
        /// </summary>
        /// <param name="g">An instance of the <see cref="Graphics"/>
        /// object to paint on.</param>
        protected override void Draw(Graphics g)
        {
            Color color = !Enabled ? DisabledColor : (Tracking ? HoverColor : BackColor);
			bool isOkay = IsCacheOkay (color, EdgeColor);
			if (!isOkay)
			{
				ClearCache();
				DrawCache(color, EdgeColor);
			}
			g.DrawImageUnscaled (_cached, Bounds);
        }
    }
}

