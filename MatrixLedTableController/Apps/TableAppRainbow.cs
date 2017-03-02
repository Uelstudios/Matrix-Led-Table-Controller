using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppRainbow : TableApp
    {
        double spectrumPosition = 0;

        public TableAppRainbow()
        {
            updateSpeed = 10;
        }

        public override void Draw()
        {
            spectrumPosition += 0.001;
            if (spectrumPosition > 1)
                spectrumPosition -= 1;
            
            PixelColor[,] m = new PixelColor[Program.TableWidth, Program.TableHeight];
            for(int x = 0; x < Program.TableWidth; x++)
            {
                for(int y = 0; y < Program.TableHeight; y++)
                {
                    double offset = (x * 0.01f) + (y * 0.01f);
                    double p = spectrumPosition + offset;

                    //Invert Touched Pixels
                    if (Program.touchManager.GetInputAt(new Position(x, y)))
                        p += 0.5;

                    if (p > 1)
                        p -= 1;
                    m[x, y] = PixelColor.FromHSL(p, 0.5f, 0.5f);
                }
            }

            SetPixels(m);
        }
    }
}
