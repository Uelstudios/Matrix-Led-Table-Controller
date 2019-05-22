using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppClock : TableApp
    {
        int toggleTime;
        bool secondBlink = true;

        public TableAppClock()
        {
            userInterface = ClientUserInterface.Custom;
            updateSpeed = 1000 / 10;
        }

        public override void Draw()
        {
            ClearPixels();
            DateTime now = DateTime.Now;
            int second = now.Second;
            int minute = now.Minute;
            int hour = now.Hour;

            if (toggleTime < 50)
            {
                //Hour
                char[] curChar = Convert.ToString((long)CharacterLookup.GetCharUlong((hour).ToString()[0]), 2).ToCharArray();
                Array.Reverse(curChar);
                for (int y = 0; y < CharacterLookup.characterHeight; y++)
                {
                    for (int x = 0; x < CharacterLookup.characterWidth; x++)
                    {
                        int index = x + (y * CharacterLookup.characterWidth);
                        if (curChar.Length > index && curChar[index] == '1')
                        {
                            SetPixel(x - 1, y + 1, PixelColor.YELLOW);
                        }
                    }
                }

                curChar = Convert.ToString((long)CharacterLookup.GetCharUlong((hour).ToString()[0]), 2).ToCharArray();
                Array.Reverse(curChar);
                for (int y = 0; y < CharacterLookup.characterHeight; y++)
                {
                    for (int x = 0; x < CharacterLookup.characterWidth; x++)
                    {
                        int index = x + (y * CharacterLookup.characterWidth);
                        if (curChar.Length > index && curChar[index] == '1')
                        {
                            SetPixel(x + 4, y + 1, PixelColor.YELLOW);
                        }
                    }
                }
            }
            else
            {

                //Minute
                for (int i = 0; i < minute; i++)
                {
                    SetPixelAtTime(i, PixelColor.WHITE);
                }

                //Second
                if (secondBlink)
                    SetPixelAtTime(second, PixelColor.RED);
                secondBlink = !secondBlink;
            }

            if (toggleTime > 100) toggleTime = 0;
             toggleTime++;
        }

        private void SetPixelAtTime(int time, PixelColor color)
        {
            SetPixel(circlePositions[(int)Math.Round((time / 60f) * 35)], color);
        }

        public override CustomInterface GetCustomInterface()
        {
            return new CustomInterface()
                .AddLabel("Diese App zeigt dir die aktuelle Uhrzeit an. :)");
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }

        Position[] circlePositions = new Position[]
        {
            new Position(0, 0),
            new Position(1, 0),
            new Position(2, 0),
            new Position(3, 0),
            new Position(4, 0),
            new Position(5, 0),
            new Position(6, 0),
            new Position(7, 0),
            new Position(8, 0),
            new Position(9, 0),
            new Position(9, 1),
            new Position(9, 2),
            new Position(9, 3),
            new Position(9, 4),
            new Position(9, 5),
            new Position(9, 6),
            new Position(9, 7),
            new Position(9, 8),
            new Position(9, 9),
            new Position(8, 9),
            new Position(7, 9),
            new Position(6, 9),
            new Position(5, 9),
            new Position(4, 9),
            new Position(3, 9),
            new Position(2, 9),
            new Position(1, 9),
            new Position(0, 9),
            new Position(0, 8),
            new Position(0, 7),
            new Position(0, 6),
            new Position(0, 5),
            new Position(0, 4),
            new Position(0, 3),
            new Position(0, 2),
            new Position(0, 1)
        };
    }
}
