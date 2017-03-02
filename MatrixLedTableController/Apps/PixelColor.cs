using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    public struct PixelColor
    {
        public static PixelColor WHITE = new PixelColor(255, 255, 255);
        public static PixelColor BLACK = new PixelColor(0, 0, 0);
        public static PixelColor RED = new PixelColor(255, 0, 0);
        public static PixelColor BLUE = new PixelColor(0, 0, 255);
        public static PixelColor GREEN = new PixelColor(0, 255, 0);
        public static PixelColor YELLOW = new PixelColor(255, 255, 0);
        public static PixelColor ORANGE = new PixelColor(255, 255, 128);
        public static PixelColor PURPLE = new PixelColor(255, 0, 255);
        public static PixelColor CYAN = new PixelColor(128, 128, 255);

        public const int maxValue = 255;
        public const int minValue = 0;
        public int r;
        public int g;
        public int b;

        public PixelColor(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public static PixelColor Random(Random rand)
        {
            return new PixelColor(rand.Next(minValue, maxValue), rand.Next(minValue, maxValue), rand.Next(minValue, maxValue));
        }

        public override string ToString()
        {
            return string.Format("[R:{0}, G:{1}, B:{2}]", r, g, b);
        }

        public bool Same(PixelColor color)
        {
            return 
                r == color.r &&
                g == color.g &&
                b == color.b;
        }

        public static PixelColor[,] GetSingleColorMap(int w, int h, PixelColor color)
        {
            PixelColor[,] m = new PixelColor[w, h];
            for(int x = 0; x < w; x++)
            {
                for(int y = 0; y < h; y++)
                {
                    m[x, y] = color;
                }
            }
            return m;
        }

        // Given H,S,L in range of 0-1
        // Returns a Color (RGB struct) in range of 0-255
        public static PixelColor FromHSL(double h, double sl, double l)

        {

            double v;

            double r, g, b;



            r = l;   // default to gray

            g = l;

            b = l;

            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

            if (v > 0)

            {

                double m;

                double sv;

                int sextant;

                double fract, vsf, mid1, mid2;



                m = l + l - v;

                sv = (v - m) / v;

                h *= 6.0;

                sextant = (int)h;

                fract = h - sextant;

                vsf = v * sv * fract;

                mid1 = m + vsf;

                mid2 = v - vsf;

                switch (sextant)

                {

                    case 0:

                        r = v;

                        g = mid1;

                        b = m;

                        break;

                    case 1:

                        r = mid2;

                        g = v;

                        b = m;

                        break;

                    case 2:

                        r = m;

                        g = v;

                        b = mid1;

                        break;

                    case 3:

                        r = m;

                        g = mid2;

                        b = v;

                        break;

                    case 4:

                        r = mid1;

                        g = m;

                        b = v;

                        break;

                    case 5:

                        r = v;

                        g = m;

                        b = mid2;

                        break;

                }

            }

            PixelColor color = new PixelColor(Convert.ToByte(r * 255.0f), Convert.ToByte(g * 255.0f), Convert.ToByte(b * 255.0f));
            return color;

        }
    }
}
