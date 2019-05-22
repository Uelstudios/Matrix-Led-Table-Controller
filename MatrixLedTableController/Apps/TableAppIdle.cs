using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace MatrixLedTableController.Apps
{
    class TableAppIdle : TableApp
    {
        bool[,] touchMap = new bool[Program.TableWidth, Program.TableHeight];
        float time = 0;
        float midBrightness = 0f;

        public TableAppIdle()
        {
            userInterface = ClientUserInterface.None;
            selectable = false;
            updateSpeed = 5;
        }

        public override void Draw()
        {
            ClearPixels();

            for (int x = 0; x < Program.TableWidth; x++)
                for (int y = 0; y < Program.TableHeight; y++)
                    SetPixel(x, y, PixelColor.FromHSL(0.0, 0.0, midBrightness));


            int dirStep = 0;
            int nextStep = 0;
            Position pos = new Position(0, 0);
            for (int i = 0; i < 36; i++)
            {
                float hsv = i / 36f + time;
                if (hsv > 1f) hsv -= (float)Math.Floor(hsv);

                SetPixel(pos.x, pos.y, PixelColor.FromHSL(hsv, 1f, 0.5));
                if (dirStep == 0)
                {
                    pos.x++;
                }
                else if (dirStep == 1)
                {
                    pos.y++;
                }
                else if (dirStep == 2)
                {
                    pos.x--;
                }
                else if (dirStep == 3)
                {
                    pos.y--;
                }

                nextStep++;
                if (nextStep >= 9)
                {
                    dirStep++;
                    nextStep = 0;
                }
            }

            time += 0.015f / 10f;
            if (midBrightness > 0f) midBrightness -= 0.08f / 10f;
        }

        public override void OnTouchUpdated(TouchManager manager)
        {
            for (int x = 0; x < touchMap.GetLength(0); x++)
            {
                for (int y = 0; y < touchMap.GetLength(1); y++)
                {
                    touchMap[x, y] = manager.GetInputAt(new Position(x, y));
                }
            }
        }

        public override void OnInputMade(InputKey key)
        {
            if (key == InputKey.XPadAction)
            {
                midBrightness = 1f;
            }
        }


        public override FeatureSet GetFeatures()
        {
            return new FeatureSet();
        }
    }
}
