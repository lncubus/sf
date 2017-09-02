using Pvax.UI;
using Pvax.UI.Views;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Sample
{
    /// <summary>
    /// Description of EllipseView.
    /// </summary>
    public class CustomButtonView : ButtonView
    {
        public readonly Func<int, int, int, GraphicsPath> ShapeBuilder;
        private GraphicsPath _cachedShape = null;
        private int _cachedWidth = -1;
        private int _cachedHeight = -1;
        private int _cachedCut = -1;

        private int cut;

        public virtual int Cut
        {
            get
            {
                return cut;
            }
            set
            {
                if (cut != value)
                {
                    cut = value;
                    Invalidate();
                }
            }
        }

        public CustomButtonView(Func<int, int, int, GraphicsPath> shapeBuilder, int x, int y, int width, int height, int cut = 0) : base(x, y, width, height)
        {
            ShapeBuilder = shapeBuilder;
            this.cut = cut;
        }

        protected virtual GraphicsPath GetCachedShape()
        {
            if (_cachedShape != null && Width == _cachedWidth && Height == _cachedHeight && Cut == _cachedCut)
                return _cachedShape;
            if (_cachedShape != null)
                _cachedShape.Dispose();
            _cachedShape = ShapeBuilder(Width, Height, Cut);
            _cachedWidth = Width;
            _cachedHeight = Height;
            _cachedCut = Cut;
            return _cachedShape;
        }

        protected override void DrawButton(Graphics graphics, Rectangle rect, ButtonState state)
        {
            //base.DrawButton(graphics, rect, state);
            rect.Inflate(-2, -2);
			Color color = Tracking ? HoverColor : BackColor;
			Color light = ControlPaint.Light(color);
			Color dark = ControlPaint.Dark(color);
			Brush brush = DrawHelper.Instance.CreateLinearGradientBrush(
		        rect, Pressed ? dark : light, Pressed ? light : dark,
				LinearGradientMode.Vertical);
            GraphicsPath path = GetCachedShape();
            graphics.FillPath(brush, path);
			Pen pen = DrawHelper.Instance.CreateColorPen(ForeColor, 2);
			graphics.DrawPath(pen, path);
        }

        protected override bool HitTest(int posX, int posY)
        {
            return base.HitTest(posX, posY) && GetCachedShape().IsVisible(posX - Left, posY - Top);
        }
    }
}
