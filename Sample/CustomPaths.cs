using System.Drawing;
using System.Drawing.Drawing2D;

namespace Sample
{
    public static class CustomPaths
    {
        public const int Delta = 2;

        public static GraphicsPath Ellipse(int width, int height, int dummy)
        {
            var result = new GraphicsPath(); ;
            result.AddEllipse(Delta, Delta, width - Delta, height - Delta);
            return result;
        }

        public static GraphicsPath RoundRectangle(int width, int height, int cut)
        {
            var diameter = 2*cut;
            var result = new GraphicsPath();
            result.AddArc(Delta, Delta, diameter, diameter, 180, 90);
            result.AddArc(width - diameter - Delta, Delta, diameter, diameter, 270, 90);
            result.AddArc(width - diameter - Delta, height - Delta - diameter, diameter, diameter, 0, 90);
            result.AddArc(Delta, height - Delta - diameter, diameter, diameter, 90, 90);
            result.CloseFigure();
            return result;
        }

        public static GraphicsPath Diamond(int width, int height, int cut)
        {
            var result = new GraphicsPath();
            Point[] points = new[]
            {
                new Point(width/2, Delta),
                new Point(width - Delta, height/2),
                new Point(width/2, height - Delta),
                new Point(Delta, height/2),
            };
            result.AddPolygon(points);
            return result;
        }

        public static GraphicsPath CutRectangle(int width, int height, int cut)
        {
            var result = new GraphicsPath();
            Point[] points = new[]
            {
                new Point(cut, Delta),
                new Point(width - cut, Delta),
                new Point(width - Delta, cut),
                new Point(width - Delta, height - cut),
                new Point(width - cut, height - Delta),
                new Point(cut, height - Delta),
                new Point(Delta, height - cut),
                new Point(Delta, cut),
            };
            result.AddPolygon(points);
            return result;
        }
    }
}
