using System;
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
    public partial class SpaceView : UserControl
    {
        protected PointF deviceScale = new PointF(Dpi.X, Dpi.Y);
        protected Point deviceOrigin = new Point(0, 0);

        protected Matrix4x4 worldMatrix = Matrix4x4.Identity;
        protected bool hideNegative = false;
        protected bool perspectiveProjection = false;

        public static readonly Point Dpi;
        public static readonly Size Resolution;

        public SpaceView() : base()
        {
            AutoSize = false;
            AutoScroll = false;
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

        private void UpdateLayout()
        {
            // TODO: IMPLEMENT
            // throw new NotImplementedException();
        }
    }
}
