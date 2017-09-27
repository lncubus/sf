using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Screen = System.Windows.Forms.Screen;
using Pvax.UI.Views;
using Vectors;

namespace Sample
{
    public class SpaceView : Pvax.UI.Views.ViewContainer
    {
        protected PointF deviceScale = new PointF(Dpi.X, Dpi.Y);
        protected Point deviceOrigin = new Point(0, 0);

        protected Matrix4x4 worldMatrix = Matrix4x4.Identity;
		protected bool hideNegative = false;
		protected bool perspectiveProjection = false;

        public static readonly Point Dpi;
        public static readonly Size Resolution;

        static SpaceView()
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                Dpi = new Point
                {
                    X = (int)graphics.DpiX,
                    Y = (int)graphics.DpiY,
                };
            }
            Resolution = Screen.PrimaryScreen.Bounds.Size;
        }

        public SpaceView() : base()
        {
            AutoSize = false;
            AutoScroll = false;
        }

        public Point DeviceOrigin
        {
            get
            {
                return deviceOrigin;
            }
            set
            {
                deviceOrigin = value;
                UpdateLayout();
            }
        }

        public PointF DeviceScale
        {
            get
            {
                return deviceScale;
            }
            set
            {
                deviceScale = value;
                UpdateLayout();
            }
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Camera_matrix
        /// </summary>
        public Matrix4x4 WorldMatrix
        {
            get
            {
                return worldMatrix;
            }
            set
            {
                worldMatrix = value;
                UpdateLayout();
            }
        }

		public bool HideNegative
		{
			get
			{
				return hideNegative;
			}
			set
			{
				hideNegative = value;
				UpdateLayout();
			}
		}

		public bool PerspectiveProjection
		{
			get
			{
				return perspectiveProjection;
			}
			set
			{
				perspectiveProjection = value;
				UpdateLayout();
			}
		}

        protected virtual int GetZIndex(IView view)
        {
            var icon = view as IconView;
            return (icon == null) ? 0 : -icon.Z;
        }

        protected override IEnumerable<IView> GetViews()
        {
            List<IView> icons = new List<IView>();
            List<IView> others = new List<IView>();
			foreach (IView view in base.GetViews())
				if (view is IconView)
				{
					if (!HideNegative || ((IconView)view).Z > 0)
						icons.Add (view);
				}
                else
                    others.Add(view);
            IEnumerable<IView> result = icons.OrderBy(GetZIndex).Concat(others);
            return result;
        }

		protected override void OnPaint (System.Windows.Forms.PaintEventArgs e)
		{
			try {
				Form1.drawWatch.Start ();
				base.OnPaint (e);
			} finally {
				Form1.drawWatch.Stop ();
			}
		}
/*
		static List<Tuple<Vector3, Vector3>> lines = null;

		protected override void PaintBackground (Graphics g, Rectangle clip)
		{
			base.PaintBackground (g, clip);
			Pen pen = Pvax.UI.DrawHelper.Instance.CreateColorPen (Color.White);
			const int N = 3;
			if (lines == null)
			{
				lines = new List<Tuple<Vector3, Vector3>> ();
				for (int a = 0; a < N; a++)
					for (int b = 0; b < N; b++)
					{
						Vector3 begin = new Vector3 (a, b, 0);
						Vector3 end = new Vector3 (a, b, N);
						lines.Add (new Tuple<Vector3, Vector3> (begin, end));
						begin = new Vector3 (a, 0, b);
						end = new Vector3 (a, N, b);
						lines.Add (new Tuple<Vector3, Vector3> (begin, end));
						begin = new Vector3 (0, a, b);
						end = new Vector3 (N, a, b);
						lines.Add (new Tuple<Vector3, Vector3> (begin, end));
					}
			}
			foreach (var line in lines)
			{
				var begin = WorldToDevice (line.Item1);
				var end = WorldToDevice (line.Item2);
				Rectangle rect = new Rectangle
				{
					X = Math.Min(begin.Item1, end.Item1),
					Y = Math.Min(begin.Item2, end.Item2),
					Width = Math.Abs(begin.Item1 - end.Item1),
					Height = Math.Abs(begin.Item2 - end.Item2),
				};
				if (clip.IntersectsWith(rect))
					g.DrawLine (pen, begin.Item1, begin.Item2, end.Item1, end.Item2);
			}
		} 
*/
        protected override IView HitTest(int posX, int posY)
        {
            IView result = base.HitTest(posX, posY);
            if (result == null || !(result is IconView))
                return result;
            var views = GetViews().ToList();
            return HitTest(views, posX, posY).FirstOrDefault();
		}

        /// <summary>
        /// https://en.wikipedia.org/wiki/3D_projection        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected virtual Tuple<int, int, int> WorldToDevice(Vector3 v)
        {
            var q = new Quaternion(v, 1);
            // camera transform
            q = WorldMatrix * q;
            double x = (q.X) * DeviceScale.X + DeviceOrigin.X; //  - q.Z / 2)
            double y = (-q.Y) * DeviceScale.Y + DeviceOrigin.Y; //  + q.Z / 2
            double z = q.Z * DeviceScale.X;

            var result = new Tuple<int, int, int>((int)x, (int)y, (int)z);
            return result;
        }

        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            foreach (IconView icon in Views.OfType<IconView>())
            {
                icon.Tracking = ((Pvax.UI.Views.IView)icon).HitTest(e.Location.X, e.Location.Y);
            }
        }

        public void UpdateLayout()
        {
            BeginUpdate();
            foreach (IconView icon in Views.OfType<IconView>())
                UpdateLayout(icon);
            // todo!
            EndUpdate();
        }

        public void UpdateLayout(IconView icon)
        {
            //icon.Invalidate(); will be invalidated if coordinates changed
            var place = WorldToDevice(icon.Vector);
            icon.Left = place.Item1 - icon.Width/2;
            icon.Top = place.Item2 - icon.Height/2;
            icon.Z = place.Item3;
        }
    }
}