﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vectors;

namespace Sample
{
    public partial class SpaceView : DoubleBufferedControl
    {
        protected bool hideNegative = false;
        public bool DemoRotating = true;

        public const float Epsilon = 0.00001F;
        public static readonly Point Dpi;
        public static readonly Size Resolution;

        public bool PerspectiveProjection = true;
        public PointF DeviceScale = new PointF(Dpi.X, Dpi.Y);

        /// <summary>
        /// https://en.wikipedia.org/wiki/Camera_matrix
        /// </summary>
        public Matrix4x4 WorldMatrix = Matrix4x4.Identity;

        public SpaceView() : base()
        {
            AutoSize = false;
            AutoScroll = false;
            timer.Tick += new EventHandler(timer_Tick);
        }

        private Vector3 axis = new Vector3(1, 1, 1) / Math.Sqrt(3);

        private void timer_Tick(object sender, EventArgs e)
        {
            if (DemoRotating)
            {
                var rotation = Matrix4x4.CreateFromAxisAngle(Vector3.UnitX, Math.PI/360);
                WorldMatrix = rotation * WorldMatrix;
            }
        }

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

        /// <summary>
        /// https://en.wikipedia.org/wiki/3D_projection        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected virtual Tuple<float, float, float, float> WorldToDevice(Vector3 v)
        {
            var q = new Quaternion(v, 1);
            // camera transform
            q = WorldMatrix * q;
			double k = 1;
            if (PerspectiveProjection)
            {
                double depth = Math.Max(Resolution.Width/DeviceScale.X, Resolution.Height / DeviceScale.Y);
                k = 1 + q.Z / depth;
                if (k > Epsilon)
                {
                    q.X /= k;
                    q.Y /= k;
                }
            }
            double x = (q.X) * DeviceScale.X + ClientSize.Width / 2; //  - q.Z / 2)
            double y = (-q.Y) * DeviceScale.Y + ClientSize.Height / 2; //  + q.Z / 2
            double z = q.Z * DeviceScale.X;
			return new Tuple<float, float, float, float>((float)x, (float)y, (float)z, (float)k);
        }

        protected override void PerformRedraw(Graphics graphics)
        {
            const int N = 3;
            base.PerformRedraw(graphics);
            for(int x = -N; x <= N; x++)
                for (int y = -N; y <= N; y++)
                    for (int z = -N; z <= N; z++)
                    {
                        Vector3 v = new Vector3(x, y, z);
                        var p = WorldToDevice(v);
						float radius = (Dpi.X / 8) / p.Item4;
                        graphics.FillEllipse(Brushes.Blue, p.Item1 - radius, p.Item2 - radius, 2 * radius, 2 * radius);
                        graphics.DrawEllipse(Pens.White, p.Item1 - radius, p.Item2 - radius, 2*radius, 2*radius);
                    }
            if (total_watch.ElapsedMilliseconds > 1000)
            {
                var fps = (drawCount*1000 / total_watch.ElapsedMilliseconds).ToString("0.#") + "\n" +
                    (internal_watch.ElapsedMilliseconds*100 / total_watch.ElapsedMilliseconds).ToString("0.#") + "%";
                graphics.DrawString(fps, Font, Brushes.White, 0, 0);
            }
        }

    }
}
