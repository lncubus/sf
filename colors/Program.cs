using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace colors
{
    static class Program
    {
        const int N = 5;
        const int N1 = 3;
        const int N2 = 5;
        const int W = 16;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] lines = File.ReadAllLines("colors.json").Where(l => l.Contains("\"r\": ")).ToArray();
            //
            using (Bitmap b = new Bitmap(W * ((N + 1) * N2 + 1), W * ((N + 1) * N1 + 1)))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    int index = 0;
                    int kk = 0;
                    g.Clear(Color.Black);
                    for (int k1 = 0; k1 < N1; k1++)
                        for (int k2 = 0; k2 < N2; k2++)
                        {
                            kk++;
                            for (int j = 0; j < N; j++)
                            {
                                for (int i = j; i < N; i++)
                                {
                                    int x, y;
                                    if (kk <= 7)
                                    {
                                        x = i * W - j * W / 2;
                                        y = j * W;
                                    }
                                    else
                                    {
                                        y = (i - j) * W;
                                        x = j * W + (i - j) * W / 2;
                                    }
                                    x += (k2 * (N + 1) + 1) * W;
                                    y += (k1 * (N + 1) + 1) * W;
                                    string line = lines[index++];
                                    line = new string(line.Where(c => char.IsDigit(c) || c == ',').ToArray());
                                    string[] rgb = line.Split(',');
                                    int red = int.Parse(rgb[0]);
                                    int green = int.Parse(rgb[1]);
                                    int blue = int.Parse(rgb[2]);
                                    using (Brush brush = new SolidBrush(Color.FromArgb(red, green, blue)))
                                        g.FillRectangle(brush, x, y, W, W);
                                }
                            }
                        }
                }
                b.Save("colors.bmp");
            }

        }
    }
}
