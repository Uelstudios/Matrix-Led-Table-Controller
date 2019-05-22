using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppImage : TableApp
    {
        PixelColor[,] map;

        public TableAppImage()
        {
            //Image image = Image.FromFile(@"D:\Projekte\Visual Studio\MatrixLedTableController\MatrixLedTableController\bin\Debug\eye.jpg");
            //image = ResizeImage(image, Program.TableWidth, Program.TableHeight);

            /*
            map = new PixelColor[image.Width, image.Height];
            using (Bitmap bmp = new Bitmap(image))
            {
                for(int x = 0; x < image.Width; x++)
                {
                    for(int y = 0; y < image.Height; y++)
                    {
                        Color clr = bmp.GetPixel(x, y);
                        int red = clr.R;
                        int green = clr.G;
                        int blue = clr.B;

                        map[x, y] = new PixelColor(red, green, blue);
                    }
                }
            }
            */

            map = PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.RED);
        }

        public override void Draw()
        {
            SetPixels(map);
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
