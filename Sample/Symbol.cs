using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace Sample
{
    public enum Symbol
    {
        None,
        Custom,
        Text,
        Rectangle,
        Ellipse,
        Diamond,
        Cross,

        Quatrefoil,
        //Up,
        //Down,
        //Left,
        //Right,
        Pentagram,
        Star,
        Asterisk,
    };

    public struct ArcF
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float StartAngle { get; set; }

        public float SweepAngle { get; set; }
    };

    public static class SymbolHelper
    {
        public static IEnumerable<PointF>Star(int n, int m)
        {
            for (int i = 0; i < n; i++)
            {
                double a = 2*Math.PI*i*m/n;
                double x = (1 + Math.Sin(a))/2;
                double y = (1 - Math.Cos(a))/2;
                yield return new PointF((float)x, (float)y);
            }
        }

        public static IEnumerable<PointF>Asterisk(int n)
        {
            double r = 0.5;//Math.Cos(2*Math.PI/n)
            for (int i = 0; i < 2*n; i++)
            {
                double a = Math.PI*i/n;
                double k = i % 2 == 0 ? 1 : r;
                double x = (1 + k*Math.Sin(a))/2;
                double y = (1 - k*Math.Cos(a))/2;
                yield return new PointF((float)x, (float)y);
            }
        }

        public static void AddBeziers(this GraphicsPath that, float[][] curves)
        {
            foreach (float[] points in curves)
            {
                int n = points.Length / 6 - 1;
                for (int i = 0; i < n; i++)
                {
                    float x1 = points[6 * i];
                    float y1 = points[6 * i + 1];
                    float x2 = points[6 * i + 2];
                    float y2 = points[6 * i + 3];
                    float x3 = points[6 * i + 4];
                    float y3 = points[6 * i + 5];
                    float x4 = points[6 * i + 6];
                    float y4 = points[6 * i + 7];
                    that.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);
                }
                that.CloseFigure();
            }
        }

        public static readonly IDictionary<Symbol, IEnumerable<ArcF>> Flowers =
            new Dictionary<Symbol, IEnumerable<ArcF>>()
            {
                {
                    Symbol.Quatrefoil, new []
                    {
                        new ArcF { X = 0.25F, Y =    0F, Width = 0.5F, Height = 0.5F, StartAngle = 180, SweepAngle = 180},
                        new ArcF { X =  0.5F, Y = 0.25F, Width = 0.5F, Height = 0.5F, StartAngle = 270, SweepAngle = 180},
                        new ArcF { X = 0.25F, Y =  0.5F, Width = 0.5F, Height = 0.5F, StartAngle =   0, SweepAngle = 180},
                        new ArcF { X =    0F, Y = 0.25F, Width = 0.5F, Height = 0.5F, StartAngle =  90, SweepAngle = 180},
                    }
                }
            };

        public static readonly IDictionary<Symbol, IEnumerable<PointF>> Polygons =
            new Dictionary<Symbol, IEnumerable<PointF>>()
            {
                {
                    Symbol.Diamond, new []
                    {
                        new PointF(0.5F, 0),
                        new PointF(1, 0.5F),
                        new PointF(0.5F, 1),
                        new PointF(0, 0.5F),
                    }
                },
                {
                    Symbol.Cross, new []
                    {
                        new PointF(0.35F, 0),
                        new PointF(0.65F, 0),
                        new PointF(0.65F, 0.35F),
                        new PointF(1, 0.35F),
                        new PointF(1, 0.65F),
                        new PointF(0.65F, 0.65F),
                        new PointF(0.65F, 1),
                        new PointF(0.35F, 1),
                        new PointF(0.35F, 0.65F),
                        new PointF(0, 0.65F),
                        new PointF(0, 0.35F),
                        new PointF(0.35F, 0.35F),
                    }
                },
                {
                    Symbol.Pentagram, Star(5, 2).ToArray()
                },
                {
                    Symbol.Star, Asterisk(5).ToArray()
                },
                {
                    Symbol.Asterisk, Asterisk(6).ToArray()
                }
            };
    }
}