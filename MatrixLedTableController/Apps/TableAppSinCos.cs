using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppSinCos : TableApp
    {
        float time = 0;

        public TableAppSinCos()
        {
            updateSpeed = 50;
        }

        public override void Draw()
        {
            for (int i = 0; i < 50; i++)
            {
                SetPixelAtIndex(i, PixelColor.FromHSL((Math.Sin(time) + 1) * 0.5, 1.0, 0.5));
            }

            for (int i = 50; i < 100; i++)
            {
                SetPixelAtIndex(i, PixelColor.FromHSL((Math.Cos(time) + 1) * 0.5, 1.0, 0.5));
            }

            time += 0.01f;
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
