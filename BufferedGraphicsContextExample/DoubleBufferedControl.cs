using System;
using System.Drawing;
using System.Windows.Forms;

namespace BufferedGraphicsContextExample
{
    public partial class DoubleBufferedControl : UserControl
    {
        private bool initializationComplete;
        private bool isDisposing;
        private BufferedGraphicsContext backbufferContext;
        private BufferedGraphics backbufferGraphics;
        private Graphics drawingGraphics;

        #region Fields for Redraw
        private Timer timer;
        private System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        private System.Diagnostics.Stopwatch internal_watch = new System.Diagnostics.Stopwatch();

        double rw, rp, gw, gp, bw, bp;
        double xw, yw, xp, yp, xp0, yp0;
        int frames = 0;
        Random random = new Random();
        #endregion

        public DoubleBufferedControl()
        {
            InitializeComponent();

            // Set the control style to double buffer.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Assign our buffer context.
            backbufferContext = BufferedGraphicsManager.Current;  
            initializationComplete = true;

            if (DesignMode)
                return;

            this.rw = 0.5 + random.NextDouble();
            this.gw = 0.5 + random.NextDouble();
            this.bw = 0.5 + random.NextDouble();
            this.rp = random.NextDouble();
            this.gp = random.NextDouble();
            this.bp = random.NextDouble();
            xw = 2 + random.NextDouble();
            yw = 2 + random.NextDouble();
            xp = random.NextDouble();
            yp = random.NextDouble();
            RecreateBuffers();

            Redraw();

            #region Redraw Logic
            timer = new Timer();
            timer.Interval = 15;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            #endregion
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Redraw();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RecreateBuffers();
            Redraw();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            isDisposing = true;
            if (disposing)
            {
                if(components != null)
                    components.Dispose();

                // We must dispose of backbufferGraphics before we dispose of backbufferContext or we will get an exception.
                if (backbufferGraphics != null)
                    backbufferGraphics.Dispose();
                if (backbufferContext != null)
                    backbufferContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private void RecreateBuffers()
        {
            // Check initialization has completed so we know backbufferContext has been assigned.
            // Check that we aren't disposing or this could be invalid.
            if (!initializationComplete || isDisposing || DesignMode)
                return;

            // We recreate the buffer with a width and height of the control. The "+ 1" 
            // guarantees we never have a buffer with a width or height of 0. 
            backbufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);

            // Dispose of old backbufferGraphics (if one has been created already)
            if (backbufferGraphics != null)
                backbufferGraphics.Dispose();

            // Create new backbufferGrpahics that matches the current size of buffer.
            backbufferGraphics = backbufferContext.Allocate(this.CreateGraphics(),
                new Rectangle(0, 0, Math.Max(this.Width, 1), Math.Max(this.Height, 1)));

            // Assign the Graphics object on backbufferGraphics to "drawingGraphics" for easy reference elsewhere.
            drawingGraphics = backbufferGraphics.Graphics;

            // This is a good place to assign drawingGraphics.SmoothingMode if you want a better anti-aliasing technique.
           
            // Invalidate the control so a repaint gets called somewhere down the line.
            this.Invalidate();
        }

        private void Redraw()
        {
            // In this Redraw method, we simply make the control fade from black to white on a timer.
            // But, you can put whatever you want here and detach the timer. The trick is just making
            // sure redraw gets called when appropriate and only when appropriate. Examples would include
            // when you resize, when underlying data is changed, when any of the draqwing properties are changed
            // (like BackColor, Font, ForeColor, etc.)
            if (drawingGraphics == null || DesignMode)
                return;
            internal_watch.Start();

            Color mask = Color.FromArgb(1   , 0, 0, 0);
            using (Brush b = new SolidBrush(mask))
                drawingGraphics.FillRectangle(b, Bounds);

            using (Brush b = new SolidBrush(Color.FromArgb(64, random.Next(128), random.Next(128), random.Next(128))))
            using (Pen p = new Pen(Color.FromArgb(64, random.Next(128), random.Next(128), random.Next(128))))
            {
                int x = random.Next(Width * 15 / 16);
                int y = random.Next(Height * 15 / 16);
                drawingGraphics.DrawEllipse(p, x, y, Width / 16, Height / 16);
                drawingGraphics.FillEllipse(b, x, y, Width / 16, Height / 16);
            }

            double m = watch.Elapsed.TotalSeconds;
            int red = (int)(byte.MaxValue * (0.5 + 0.49 * Math.Sin(rw * m + rp)));
            int green = (int)(byte.MaxValue * (0.5 + 0.49 * Math.Sin(gw * m + gp)));
            int blue = (int)(byte.MaxValue * (0.5 + 0.49 * Math.Sin(bw * m + bp)));
            Color c = Color.FromArgb(16, red, green, blue);
            using (Brush brush = new SolidBrush(c))
            {
                int x = (int)(Width * (0.5 + 0.49 * Math.Sin(xw * m + xp)));
                int y = (int)(Width * (0.5 + 0.49 * Math.Sin(yw * m + yp)));

                drawingGraphics.FillEllipse(brush, x/4, y/4, Width / 3, Height / 3);
                drawingGraphics.FillEllipse(brush, 2*Width/3 - x / 4, 2*Height/3 - y / 4, Width / 3, Height / 3);
                double intern = 100 * internal_watch.ElapsedTicks / watch.ElapsedTicks;
                string mark = (1000*frames/(watch.ElapsedMilliseconds)).ToString("0.##") + "    " + intern.ToString("0.###") +
                    "    " + xp.ToString("0.000") + "    " + yp.ToString("0.000");
                drawingGraphics.FillRectangle(Brushes.Black, 0, 0, 2*drawingGraphics.DpiX, drawingGraphics.DpiY/2);
                drawingGraphics.DrawString(mark, Font, Brushes.White, 0, 0);
            }
            
            // Force the control to both invalidate and update. 
            this.Invalidate(false);
            internal_watch.Stop();
        }

        protected override void OnPaint(PaintEventArgs e)
        {           
            base.OnPaint(e);
            // If we've initialized the backbuffer properly, render it on the control. 
            // Otherwise, do just the standard control paint.
            if (!isDisposing && backbufferGraphics != null && !DesignMode)
                backbufferGraphics.Render(e.Graphics);
            frames++;
        }

        bool rotating;
        Point origin;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
                return;
            origin = PointToScreen(e.Location);
            rotating = true;
            Cursor = Cursors.NoMove2D;
            xp0 = xp;
            yp0 = yp;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.Left || !rotating)
            {
                Cursor = Cursors.Default;
                return;
            }
            Point current = PointToScreen(e.Location);
            xp = xp0 + (current.X - origin.X) / 120.0;
            yp = yp0 + (current.Y - origin.Y) / 120.0;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            rotating = false;
            Cursor = Cursors.Default;
        }
    }
}
