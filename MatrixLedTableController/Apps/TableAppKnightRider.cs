using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppKnightRider : TableApp
    {
        int row = 0;
        bool increasing = true;

        public TableAppKnightRider()
        {
            updateSpeed = 100;
        }

        public override void Draw()
        {
            ClearPixels();
            for(int i = 0; i < Program.TableHeight; i++)
            {
                SetPixel(row, i, PixelColor.RED);
            }


            if (increasing)
                row++;
            else
                row--;

            if(row > Program.TableWidth - 1)
            {
                row = Program.TableWidth - 1;
                increasing = false;
            }

            if(row < 0)
            {
                row = 0;
                increasing = true;
            }
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
