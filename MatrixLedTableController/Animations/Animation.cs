using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController.Animations
{
    class Animation
    {
        public int fps = 15;

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
