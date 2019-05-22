using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppFire : TableApp
    {
        float hueOffset = 0.1f;
        float[,] brightnesses = new float[Program.TableWidth, Program.TableHeight];

        public TableAppFire()
        {
            updateSpeed = 50;
        }

        public override void Draw()
        {
            for (int x = 0; x < brightnesses.GetLength(0); x++)
            {
                for (int y = 0; y < brightnesses.GetLength(1); y++)
                {
                    if (brightnesses[x, y] > 0f)
                        brightnesses[x, y] -= 0.02f;
                    SetPixel(x, y, PixelColor.FromHSL(brightnesses[x, y] * hueOffset, brightnesses[x, y], brightnesses[x, y] * 0.5));

                    if (Program.random.Next(0, 100) < 2)
                    {
                        brightnesses[x, y] = 1f;
                    }
                }
            }
        }

        public override void OnInputMade(InputKey key)
        {
            if (key == InputKey.XPadRight)
            {
                hueOffset += 0.2f;
                if (hueOffset > 1f) hueOffset -= 1f;
            }
            if (key == InputKey.XPadLeft)
            {
                hueOffset -= 0.2f;
                if (hueOffset < 0f) hueOffset += 1f;
            }
            if (key == InputKey.XPadAction)
            {
                for (int x = 0; x < brightnesses.GetLength(0); x++)
                    for (int y = 0; y < brightnesses.GetLength(1); y++)
                        brightnesses[x, y] = 1f;
            }
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(true, false);
        }
    }
}
