using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppPolice : TableApp
    {
        int step = 0;
        bool red = true;

        public TableAppPolice()
        {
            updateSpeed = 60;
        }

        public override void Draw()
        {
            ClearPixels();

            if (step == 0 || step == 2)
            {
                for(int x = 0; x < Program.TableWidth / 2; x++)
                {
                    for (int y = 0; y < Program.TableHeight; y++)
                    {
                        SetPixel(y, x, PixelColor.RED);
                    }
                }
            }
            else if(step == 6 || step == 8)
            {
                for (int x = Program.TableWidth / 2; x < Program.TableWidth; x++)
                {
                    for (int y = 0; y < Program.TableHeight; y++)
                    {
                        SetPixel(y, x, PixelColor.BLUE);
                    }
                }
            }

            red = !red;
            step++;

            if (step > 11) step = 0;
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
