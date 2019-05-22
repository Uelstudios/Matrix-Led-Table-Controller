using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppBlink : TableApp
    {
        bool isOn = false;

        public TableAppBlink()
        {
            updateSpeed = 100;
        }

        public override void Draw()
        {
            if (isOn)
            {
                SetPixels(PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.Random(Program.random)));
            }
            else
            {
                ClearPixels();
            }

            isOn = !isOn;
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
