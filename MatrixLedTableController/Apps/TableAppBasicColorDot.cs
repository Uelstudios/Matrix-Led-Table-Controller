using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppBasicColorDot : TableApp
    {
        int positionX = 0;
        int positionY = 0;
        PixelColor currentColor = PixelColor.BLACK;

        public TableAppBasicColorDot()
        {
            userInterface = ClientUserInterface.XPad;
            updateSpeed = 5;
        }

        public override void Draw()
        {
            ClearPixels();
            SetPixel(positionX, positionY, currentColor);
        }

        public override void OnInputMade(InputKey key)
        {
            if (key == InputKey.XPadUp)
            {
                positionY--;
            }
            else if (key == InputKey.XPadLeft)
            {
                positionX--;
            }
            else if (key == InputKey.XPadRight)
            {
                positionX++;
            }
            else if (key == InputKey.XPadDown)
            {
                positionY++;
            }
            else if (key == InputKey.XPadAction)
            {
                currentColor = PixelColor.Random(new Random());
            }
        }
    }
}
