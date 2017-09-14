﻿using System;
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

        protected virtual int GetZIndex(IView view)
        {
            var icon = view as IconView;
            return (icon == null) ? 0 : icon.Z;
        }

        protected override IEnumerable<IView> GetViews()
        {
            List<IView> icons = new List<IView>();
            List<IView> others = new List<IView>();
            foreach (IView view in base.GetViews())
                if (view is IconView)
                    icons.Add(view);
                else
                    others.Add(view);
            IEnumerable<IView> result = icons.OrderBy(GetZIndex).Concat(others);
            return result;
        }

        protected override IView HitTest(int posX, int posY)
        {
            IView result = base.HitTest(posX, posY);
            if (result == null || !(result is IconView))
                return result;
            var views = GetViews().ToList();
            return HitTest(views, posX, posY).First();
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
            icon.Invalidate();
            var place = WorldToDevice(icon.Vector);
            icon.Left = place.Item1 - icon.Width/2;
            icon.Top = place.Item2 - icon.Height/2;
            icon.Z = place.Item3;
            // todo!
            icon.Invalidate();
        }
    }
}