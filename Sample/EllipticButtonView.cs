using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Pvax.UI;
using Pvax.UI.Views;

namespace Sample
{
    /// <summary>
    /// Description of EllipseView.
    /// </summary>
    public class EllipticButtonView : ButtonView
    {
        public EllipticButtonView(int x, int y, int width, int height) : base(x, y, width, height)
        {
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
			//ForwardDiagonal);
				//DrawHelper.Instance.CreateSolidBrush(Tracking ? HoverColor : BackColor);
			graphics.FillEllipse(brush, rect);

			//Pen lightpen = DrawHelper.Instance.CreateColorPen(light, 2);
            //Pen darkpen = DrawHelper.Instance.CreateColorPen(dark, 2);
			Pen pen = DrawHelper.Instance.CreateColorPen(ForeColor, 2);
			//graphics.DrawArc(Pressed ? darkpen : lightpen, rect, -45, -180);
			//graphics.DrawArc(!Pressed ? darkpen : lightpen, rect, -225, -180);
			graphics.DrawEllipse(pen, rect);
        }

        protected override bool HitTest(int posX, int posY)
        {
            bool result;
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(X, Y, Width - 1, Height - 1);
                result = path.IsVisible(posX, posY);
            }
            return result;
        }
    }
}
