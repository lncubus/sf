using System;
using System.Drawing;
using System.Windows.Forms;
using SmoothingMode = System.Drawing.Drawing2D.SmoothingMode;

namespace Sample
{
    public partial class DoubleBufferedControl : UserControl
    {
        private bool initializationComplete;
        private bool isDisposing;
        private BufferedGraphicsContext backbufferContext;
        private BufferedGraphics backbufferGraphics;
        private Graphics drawingGraphics;

        #region Fields for Redraw
        protected Timer timer;
        protected System.Diagnostics.Stopwatch total_watch = System.Diagnostics.Stopwatch.StartNew();
        protected System.Diagnostics.Stopwatch internal_watch = new System.Diagnostics.Stopwatch();

        protected int drawCount = 0;
        protected int paintCount = 0;
        #endregion

        public DoubleBufferedControl()
        {
            // Set the control style to double buffer.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            // Assign our buffer context.
            backbufferContext = BufferedGraphicsManager.Current;
            initializationComplete = true;

            if (DesignMode)
                return;

            RecreateBuffers();

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
            drawingGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            // Invalidate the control so a repaint gets called somewhere down the line.
            this.Invalidate();
        }

        protected virtual void PerformRedraw(Graphics graphics)
        {
            drawingGraphics.Clear(BackColor);
        }

        private void Redraw()
        {
            if (drawingGraphics == null || DesignMode)
                return;
            internal_watch.Start();
            PerformRedraw(drawingGraphics);
            drawCount++;
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
            paintCount++;
        }
    }
}
