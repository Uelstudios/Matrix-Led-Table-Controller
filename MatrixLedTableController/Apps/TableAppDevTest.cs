using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppDevTest : TableApp
    {
        int ledIndex = 0;
        bool on = false;

        public override void Draw()
        {
            on = !on;

            /*
            SetPixels(PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight,
                on ? PixelColor.WHITE : PixelColor.BLACK));

            SetPixel(0, 0, !on ? PixelColor.WHITE : PixelColor.BLACK);
            SetPixel(0, Program.TableHeight - 1, !on ? PixelColor.WHITE : PixelColor.BLACK);
            SetPixel(Program.TableWidth - 1, 0, !on ? PixelColor.WHITE : PixelColor.BLACK);
            SetPixel(Program.TableWidth - 1 , Program.TableHeight -1, !on ? PixelColor.WHITE : PixelColor.BLACK);
            */

            ClearPixels();

            SetPixel(0, 0, PixelColor.RED);
            SetPixel(0, 9, PixelColor.GREEN);
            SetPixel(9, 0, PixelColor.YELLOW);
            SetPixel(9, 9, PixelColor.BLUE);

            int y = ledIndex / Program.TableWidth;
            int x = ledIndex - y * Program.TableWidth;
            SetPixel(x, y, PixelColor.ORANGE);

            ledIndex++;
            if (ledIndex > 99) ledIndex = 0;
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
