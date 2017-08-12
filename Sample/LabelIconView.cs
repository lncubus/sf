using System;
using System.Drawing;

using Pvax.UI;
using System.Drawing.Drawing2D;

namespace Sample
{
    public class LabelIconView : IconView
    {
        public override Symbol Symbol
        {
            get
            {
                return Symbol.Text;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        string text;

        /// <summary>
        /// Gets or sets the text associated with this view.
        /// </summary>
        /// <value>
        /// The text associated with this view.
        /// </value>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text != value)
                {
                    text = value;
                    SpaceView parent = Parent.Control as SpaceView;
                    if (parent != null)
                        parent.UpdateLayout(this);
                    else
                        Invalidate();
                }
            }
        }


        /// <summary>
        /// Paint the label.
        /// </summary>
        /// <param name="g">An instance of the <see cref="Graphics"/>
        /// object to paint on.</param>
        protected override void Draw(Graphics g)
        {
            Rectangle layout = Bounds;
            layout.Inflate(-2, -2);
            Brush brush = DrawHelper.Instance.CreateSolidBrush(ForeColor);
            StringFormat format = DrawHelper.Instance.CreateTypographicStringFormat(ContentAlignment.MiddleCenter);
            g.DrawString(Text, Font, brush, layout, format);

        }
    }
}
