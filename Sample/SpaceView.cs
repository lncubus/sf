using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
//using System.Windows.Forms;

using Pvax.UI;
using Pvax.UI.Views;


namespace Sample
{
	public class SpaceView : Pvax.UI.Views.View
	{
		public List<Icon> Icons = new List<Icon>();

		public SpaceView () : base()
		{
		}

		public SpaceView(int x, int y, int width, int height):
		base(x, y, width, height)
		{
		}

        protected override void Draw(Graphics g)
        {
            Font font = Parent.Control.Font;
            float dpiX = g.DpiX;
            float dpiY = g.DpiY;
            foreach(Icon i in Icons)
                i.Draw(g, font, dpiX, dpiY, false);
        }

        public enum Symbol
        {
            Rectangle,
            Diamond,
            Ellipse,
            Cross,
            //Asterisk,
            Quatrefoil,
            //Up,
            //Down,
            //Left,
            //Right,
        };

        protected static readonly IDictionary<Symbol, IEnumerable<PointF>> symbols =
            new Dictionary<Symbol, IEnumerable<PointF>>()
        {
            {
                Symbol.Rectangle, new []
                {
                    new PointF(0, 0),
                    new PointF(0, 1),
                    new PointF(1, 1),
                    new PointF(1, 0),
                }
            },
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

        public class Icon
		{
			public float X, Y, W, H;

            public Symbol Symbol;

			public Color TextColor = Color.Empty;

			public Color EdgeColor = Color.Empty;

			public Color FillColor = Color.Empty;

			public Color HoverColor = Color.Empty;

			public string Text;

			public void Draw(Graphics g, Font font, float scaleX, float scaleY, bool hovering)
			{
				Color color = hovering && HoverColor != Color.Empty ? HoverColor : FillColor;
				Brush brush = DrawHelper.Instance.CreateSolidBrush(color);
				Pen pen = DrawHelper.Instance.CreateColorPen(EdgeColor, 2);
                float x = scaleX * X;
                float y = scaleY * Y;
                float w = scaleX * W - 1;
                float h = scaleY * H - 1;
                switch (Symbol)
				{
                    case Symbol.Rectangle:
                        g.FillRectangle(brush, x, y, w, h);
                        g.DrawRectangle(pen, x, y, w, h);
                        break;
                    case Symbol.Diamond:
                    case Symbol.Cross:
                        PointF[] points = symbols[Symbol].ToArray();
                        for (int i = 0; i < points.Length; i++)
                        {
                            PointF p = points[i];
                            p.X = x + p.X * w;
                            p.Y = y + p.Y * h;
                            points[i] = p;
                        }
                        g.FillPolygon(brush, points);
                        g.DrawPolygon(pen, points);
                        break;
                    case Symbol.Ellipse:
                        g.FillEllipse(brush, x, y, w, h);
                        g.DrawEllipse(pen, x, y, w, h);
                        break;
                    case Symbol.Quatrefoil:
                        
                        GraphicsPath flower = new GraphicsPath();
                        flower.AddArc(x + w / 4, y, w / 2, h / 2, 180, 180);
                        flower.AddArc(x + w / 2, y + h / 4, w / 2, h / 2, 270, 180);
                        flower.AddArc(x + w / 4, y + h / 2, w / 2, h / 2, 0, 180);
                        flower.AddArc(x, y + h / 4, w / 2, h / 2, 90, 180);
                        g.FillPath(brush, flower);
                        g.DrawPath(pen, flower);
                        break;
				}
                if (!string.IsNullOrEmpty(Text))
                {
                    brush = DrawHelper.Instance.CreateSolidBrush(TextColor);
                    StringFormat format =
                        DrawHelper.Instance.CreateTypographicStringFormat(ContentAlignment.TopCenter);
                    g.DrawString(Text, font, brush, x + w * 0.5F, y + h + scaleY/12, format);
                }
			}
		}
	}
}

