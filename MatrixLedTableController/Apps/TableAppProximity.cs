using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppProximity : TableApp
    {
        float[,] amounts = new float[Program.TableWidth, Program.TableHeight];

        public TouchManager touchManager;

        public TableAppProximity()
        {
            updateSpeed = 10;
            touchManager = Program.touchManager;
        }

        public override void Draw()
        {
            for (int x = 0; x < Program.TableWidth; x++)
            {
                for (int y = 0; y < Program.TableHeight; y++)
                {
                    amounts[x, y] = Mathf.Clamp01(amounts[x, y] - 0.01f);

                    if (touchManager.GetInputAt(new Position(x, y))) amounts[x, y] = 1f;
                    SetPixel(x, y, PixelColor.LerpRGB(PixelColor.BLACK, PixelColor.WHITE, amounts[x, y]));
                }
            }
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, true);
        }
    }
}
