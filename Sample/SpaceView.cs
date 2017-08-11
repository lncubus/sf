﻿using System;
using System.Drawing;
using System.Linq;
using Vectors;

namespace Sample
{
    public class SpaceView : Pvax.UI.Views.ViewContainer
    {
        protected PointF deviceScale = new PointF(Dpi.X, Dpi.Y);
        protected Point deviceOrigin = new Point(0, 0);

        protected Matrix4x4 worldMatrix = Matrix4x4.Identity;

        public static readonly Point Dpi;

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

        /// <summary>
        /// https://en.wikipedia.org/wiki/3D_projection        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected virtual Point WorldToDevice(Vector3 v)
        {
            var q = new Quaternion(v, 1);
            // camera transform
            q = WorldMatrix * q;
            double x = (q.X - q.Z / 2) * deviceScale.X + deviceOrigin.X;
            double y = (-q.Y + q.Z / 2) * deviceScale.Y + deviceOrigin.Y;
            // Use https://en.wikipedia.org/wiki/Camera_matrix, stupid!!!
            Point result = new Point
            {
                X = (int)x,
                Y = (int)y,
            };
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
            Point place = WorldToDevice(icon.Vector);
            icon.Left = place.X - icon.Width/2;
            icon.Top = place.Y - icon.Height/2;
            // todo!
            icon.Invalidate();
        }
    }
}