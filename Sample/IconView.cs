using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Pvax.UI;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Sample
{
    public class IconView : Pvax.UI.Views.View
    {
        protected static readonly IDictionary<Symbol, IEnumerable<PointF>> polygons =
            new Dictionary<Symbol, IEnumerable<PointF>>()
        {
            {
                Symbol.Diamond, new []
                {
                    new PointF(0.5F, 0),
                    new PointF(1, 0.5F),
                    new PointF(0.5F, 1),
                    new PointF(0, 0.5F),
                }
            },
            {
                Symbol.Cross, new []
                {
                    new PointF(0.35F, 0),
                    new PointF(0.65F, 0),
                    new PointF(0.65F, 0.35F),
                    new PointF(1, 0.35F),
                    new PointF(1, 0.65F),
                    new PointF(0.65F, 0.65F),
                    new PointF(0.65F, 1),
                    new PointF(0.35F, 1),
                    new PointF(0.35F, 0.65F),
                    new PointF(0, 0.65F),
                    new PointF(0, 0.35F),
                    new PointF(0.35F, 0.35F),
                }
            }
        };

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
                    Recalculate();
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
                Recalculate();
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
                Recalculate();
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
                Recalculate();
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
                Recalculate();
            }
        }

        protected virtual void Recalculate()
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
            int w = Width - 1;
            int h = Height - 1;
            switch (Symbol)
            {
                case Symbol.Rectangle:
                    g.FillRectangle(brush, 0, 0, w, h);
                    g.DrawRectangle(pen, 0, 0, w, h);
                    break;
                case Symbol.Ellipse:
                    g.FillEllipse(brush,  0, 0, w, h);
                    g.DrawEllipse(pen,  0, 0, w, h);
                    break;
                case Symbol.Diamond:
                case Symbol.Cross:
                    PointF[] points = polygons[Symbol].ToArray();
                    for (int i = 0; i < points.Length; i++)
                    {
                        PointF p = points[i];
                        p.X = p.X * w;
                        p.Y = p.Y * h;
                        points[i] = p;
                    }
                    g.FillPolygon(brush, points);
                    g.DrawPolygon(pen, points);
                    break;
                case Symbol.Quatrefoil:
                    GraphicsPath flower = new GraphicsPath();
                    flower.AddArc(w/4F, 0,    w/2F, h/2F, 180, 180);
                    flower.AddArc(w/2F, h/4F, w/2F, h/2F, 270, 180);
                    flower.AddArc(w/4F, h/2F, w/2F, h/2F, 0, 180);
                    flower.AddArc(0,    h/4F, w/2F, h/2F, 90, 180);
                    g.FillPath(brush, flower);
                    g.DrawPath(pen, flower);
                    break;
                default :
                    throw new NotImplementedException(Symbol.ToString());
            }
        }
    }

    public enum Symbol
    {
        Rectangle,
        Ellipse,
        Diamond,
        Cross,
        //Asterisk,
        Quatrefoil,
        //Up,
        //Down,
        //Left,
        //Right,
    };

}

