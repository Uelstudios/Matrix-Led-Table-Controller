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
            userInterface = ClientUserInterface.Paint;
        }

        public override void Draw()
        {
            Random rand = new Random();
            int w = rand.Next(0, Program.TableWidth);
            int h = rand.Next(0, Program.TableHeight);

            SetPixel(w, h, PixelColor.Random(rand));
        }
    }
}
