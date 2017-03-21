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
        public struct ArcF
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
            public float StartAngle { get; set; }
            public float SweepAngle { get; set; }
        };

        protected static readonly IDictionary<Symbol, IEnumerable<ArcF>> flowers =
            new Dictionary<Symbol, IEnumerable<ArcF>>()
        {
                {
                    Symbol.Quatrefoil, new []
                    {
                        new ArcF { X = 0.25F, Y =    0F, Width = 0.5F, Height = 0.5F, StartAngle = 180, SweepAngle = 180},
                        new ArcF { X =  0.5F, Y = 0.25F, Width = 0.5F, Height = 0.5F, StartAngle = 270, SweepAngle = 180},
                        new ArcF { X = 0.25F, Y =  0.5F, Width = 0.5F, Height = 0.5F, StartAngle =   0, SweepAngle = 180},
                        new ArcF { X =    0F, Y = 0.25F, Width = 0.5F, Height = 0.5F, StartAngle =  90, SweepAngle = 180},
                    }
                }
        };

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
            layout.Inflate(-1, -1);
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
                case Symbol.Diamond:
                case Symbol.Cross:
                    PointF[] points = polygons[Symbol].ToArray();
                    for (int i = 0; i < points.Length; i++)
                    {
                        PointF p = points[i];
                        p.X = layout.Left + p.X * layout.Width;
                        p.Y = layout.Top + p.Y * layout.Height;
                        points[i] = p;
                    }
                    g.FillPolygon(brush, points);
                    g.DrawPolygon(pen, points);
                    break;
                case Symbol.Quatrefoil:
                    GraphicsPath flower = new GraphicsPath();
                    ArcF[] arcs = flowers[Symbol].ToArray();
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

