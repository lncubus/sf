using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Pvax.UI;
using System.Drawing.Drawing2D;

namespace Sample
{
    public class IconView : Pvax.UI.Views.View
    {
        public IconView() : base()
        {
        }

        public override Pvax.UI.Views.IControlService Parent
        {
            get
            {
                return base.Parent;
            }
            set
            {
                if (base.Parent != value)
                {
                    base.Parent = value;
                    UpdateLayout();
                }
            }
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
        public Symbol Symbol
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
                }
            }
        }

        private float x, y, w, h;

        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                UpdateLayout();
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                UpdateLayout();
            }
        }

        public float W
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
                UpdateLayout();
            }
        }

        public float H
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
                UpdateLayout();
            }
        }

        public virtual void UpdateLayout()
        {
            PointF origin;
            PointF scale;
            if (Parent is SpaceView)
            {
                origin = ((SpaceView)Parent).WorldOrigin;
                scale = ((SpaceView)Parent).WorldScale;
            }
            else
            {
                origin = new PointF(0, 0);
                scale = SpaceView.Dpi;
            }
            float left = origin.X + (X - Math.Abs(W)/2F)*scale.X;
            float top = origin.Y + (Y - Math.Abs(H)/2F)*scale.Y;

            //Location = 
            Width = (int)(Math.Ceiling(W*scale.X));
            Height = (int)(Math.Ceiling(H*scale.Y));
            Left = (int)(Math.Round(left));
            Top = (int)(Math.Round(top));
        }

        /// <summary>
        /// Paint the symbol.
        /// </summary>
        /// <param name="g">An instance of the <see cref="Graphics"/>
        /// object to paint on.</param>
        protected override void Draw(Graphics g)
        {
            Color color = Tracking ? HoverColor : BackColor;
            Brush brush = DrawHelper.Instance.CreateSolidBrush(color);
            Pen pen = DrawHelper.Instance.CreateColorPen(EdgeColor, 1.2F);
            Rectangle layout = Bounds;
            layout.Inflate(-2, -2);
            switch (Symbol)
            {
                case Symbol.Rectangle:
                    g.FillRectangle(brush, layout);
                    g.DrawRectangle(pen, layout);
                    break;
                case Symbol.Ellipse:
                    g.FillEllipse(brush, layout);
                    g.DrawEllipse(pen, layout);
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
    }
}

