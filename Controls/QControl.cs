using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controls
{
    public partial class QControl : Component
    {
        public QControl()
        {
            InitializeComponent();
        }

        public QControl(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        private QScreen parent;

        private int x;
        private int y;
        private int width;
        private int height;
        private string text;

        private Color backColor = Color.Black;
        private Color foreColor = Color.Black;

        [Category("Layout")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Bottom
        {
            get
            {
                return y + height;
            }
            set
            {
                Height = value - y;
            }
        }

        [Category("Layout")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                SetBounds(x, y, width, value, BoundsSpecified.Height);
            }
        }

        [Category("Layout")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Left
        {
            get
            {
                return x;
            }
            set
            {
                SetBounds(value, y, width, height, BoundsSpecified.X);
            }
        }

        [Category("CatLayout")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Right
        {
            get
            {
                return x + width;
            }
            set
            {
                Width = value - x;
            }
        }

        [Category("Layout")]
        [Localizable(true)]
        public Point Location
        {
            get
            {
                return new Point(x, y);
            }
            set
            {
                SetBounds(X, Y, width, height, BoundsSpecified.Location);
            }
        }

        [Category("Layout")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                if (value.IsEmpty || backColor == value)
                    return;
                backColor = value;
                OnBackColorChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public string Name
        {
            get; set;
        }

        [Category("Behavior")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public QScreen Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (parent == value)
                    return;
                if (value != null)
                    value.Add(this);
                if (parent != null)
                    parent.Remove(this);
                parent = value;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(x, y, width, height);
            }
            set
            {
                SetBounds(value.X, value.Y, value.Width, value.Height, BoundsSpecified.All);
            }
        }

        public void SetBounds(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if ((specified & BoundsSpecified.X) == BoundsSpecified.None)
                x = this.x;
            if ((specified & BoundsSpecified.Y) == BoundsSpecified.None)
                y = this.y;
            if ((specified & BoundsSpecified.Width) == BoundsSpecified.None)
                width = this.width;
            if ((specified & BoundsSpecified.Height) == BoundsSpecified.None)
                height = this.height;
            if (this.x != x || this.y != y || (this.width != width || this.height != height))
            {
                SetBoundsCore(x, y, width, height, specified);
            }
        }
    }
}
