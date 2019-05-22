using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController.Animations
{
    class Animation
    {
        static readonly string TAG = "Animation";

        AnimationFrame[] frames;
        int fps = 15;

        public Animation(string imagePath, int framesPerSecond = 2)
        {
            /*
            string name = imagePath.Remove(0, imagePath.LastIndexOfAny(new char[] { '/', '\\' }));
            Program.Log(TAG, "Loading Animation... (" + name + ")");
            Bitmap bitmap = new Bitmap(imagePath);
            int frameCount = bitmap.Width / 10;

            Program.Log(TAG, "Animation successfully loaded. (" + frameCount + " frames)");
            */
            Program.Log(TAG, "Animation skipped. This version doesn't support it.");
        }

        class AnimationFrame
        {
            PixelColor[,] colorMap;
        }

        public static PixelColor UIntToColor(uint color)
        {
            //byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return new PixelColor(r, g, b);
        }
    }
}
