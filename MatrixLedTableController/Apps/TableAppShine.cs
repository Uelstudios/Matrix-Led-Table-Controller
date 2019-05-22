using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppShine : TableApp
    {
        PixelColor color;
        bool raising = false;
        float amount = 0;
        float speed = 0.002f;

        public TableAppShine()
        {
            updateSpeed = 10;
        }

        public override void Draw()
        {
            if (raising)
                amount += speed;
            else
                amount -= speed;


            SetPixels(PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.LerpRGB(PixelColor.BLACK, color, Mathf.Clamp01(amount))));

            if (raising)
            {
                if (amount >= 1.5f)
                {
                    raising = false;
                }
            }
            else if (amount <= -0.5f)
            {
                ClearPixels();
                Reset();
            }
        }

        void Reset()
        {
            amount = 0f;
            raising = true;

            color = PixelColor.FromHSL(Program.random.NextDouble(), 1.0, 0.5);
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
