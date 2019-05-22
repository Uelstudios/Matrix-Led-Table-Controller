using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppChess : TableApp
    {
        PixelColor[,] chessMap;
        float time = 0;

        public TableAppChess()
        {
            updateSpeed = 50;

            //Setup Chess Field
            chessMap = new PixelColor[Program.TableWidth, Program.TableHeight];
            for (int i = 1; i < chessMap.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < chessMap.GetLength(1) - 1; j++)
                {
                    if ((j % 2 == 0 && i % 2 == 0) || (j % 2 != 0 && i % 2 != 0))
                        chessMap[i, j] = PixelColor.BLACK;
                    else if ((j % 2 == 0 && i % 2 != 0) || (j % 2 != 0 && i % 2 == 0))
                        chessMap[i, j] = PixelColor.WHITE;
                }
            }
        }

        public override void Draw()
        {
            int dirStep = 0;
            int nextStep = 0;
            Position pos = new Position(0, 0);
            for (int i = 0; i < 36; i++)
            {
                float hsv = i / 36f + time;
                if (hsv > 1f) hsv -= (float)Math.Floor(hsv);

                SetPixel(pos.x, pos.y, PixelColor.FromHSL(hsv, 0.5f, 0.5));
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

            time += 0.003f;

            SetPixels(chessMap);
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }

}
