using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppDevTest : TableApp
    {
        bool on = false;

        public override void Draw()
        {
            on = !on;

            SetPixels(PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight,
                on ? PixelColor.WHITE : PixelColor.BLACK));

            SetPixel(0, 0, !on ? PixelColor.WHITE : PixelColor.BLACK);
            SetPixel(0, Program.TableHeight - 1, !on ? PixelColor.WHITE : PixelColor.BLACK);
            SetPixel(Program.TableWidth - 1, 0, !on ? PixelColor.WHITE : PixelColor.BLACK);
            SetPixel(Program.TableWidth - 1 , Program.TableHeight -1, !on ? PixelColor.WHITE : PixelColor.BLACK);
        }

    }
}
