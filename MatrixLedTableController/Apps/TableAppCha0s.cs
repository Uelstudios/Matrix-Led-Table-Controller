using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppCha0s : TableApp
    {
        public TableAppCha0s()
        {
            updateSpeed = 1;
        }

        public override void Draw()
        {
            int w = Program.random.Next(0, Program.TableWidth);
            int h = Program.random.Next(0, Program.TableHeight);

            float hue = Program.random.Next(100) / 100f;
            SetPixel(w, h, PixelColor.FromHSL(hue, 1f, 0.5f));
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
