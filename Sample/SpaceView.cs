using System;
using System.Drawing;
using System.Linq;

namespace Sample
{
    public class SpaceView : Pvax.UI.Views.ViewContainer
    {
        protected PointF origin = new PointF(0, 0);
        protected PointF scale = new PointF(Dpi.X, Dpi.Y);

        public static readonly PointF Dpi;

        static SpaceView()
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                Dpi = new PointF
                {
                    X = graphics.DpiX,
                    Y = graphics.DpiY,
                };
            }
        }

        public SpaceView() : base()
        {
            AutoSize = false;
            AutoScroll = false;
        }

        public PointF WorldOrigin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
                foreach (IconView icon in Views.OfType<IconView>())
                    icon.UpdateLayout();
            }
        }

        public PointF WorldScale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                foreach (IconView icon in Views.OfType<IconView>())
                    icon.UpdateLayout();
            }
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            foreach (IconView icon in Views.OfType<IconView>())
            {
                icon.Tracking = ((Pvax.UI.Views.IView)icon).HitTest(e.Location.X, e.Location.Y);
            }
        }
    }
}