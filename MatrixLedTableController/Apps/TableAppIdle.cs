using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace MatrixLedTableController.Apps
{
    class TableAppIdle : TableApp
    {
        List<MovingDot> dots = new List<MovingDot>();

        public TableAppIdle()
        {
            selectable = false;
            updateSpeed = 50;


            for(int i = 0; i < 9; i++)
                dots.Add(new MovingDot(new Position(i, 0), new Position(1, 0), 0, 0, PixelColor.FromHSL(i / 50f, 0.5, 0.5)));
            for (int i = 9; i > 0; i--)
                dots.Add(new MovingDot(new Position(i, 9), new Position(-1, 0), 0, 0, PixelColor.FromHSL(i / 50f + 0.5, 0.5, 0.5)));
        }

        public override void Draw()
        {
            ClearPixels();
            foreach (MovingDot d in dots)
            {
                d.Update();
                SetPixel(d.GetPositon().x, d.GetPositon().y, d.color);
            }
        }

        public class MovingDot
        {
            public PixelColor color;
            Position position;
            Position speed;
            int bound;
            int skipStep;
            int currentSkip = 0;

            public MovingDot(Position start, Position direction, int bound, int skipStep, PixelColor c)
            {
                position = start;
                speed = direction;
                this.bound = bound;
                color = c;
                this.skipStep = skipStep;
            }

            public void Update ()
            {
                if (currentSkip > skipStep)
                {
                    currentSkip = 0;

                    if (position.x + speed.x >= Program.TableWidth - bound)
                        speed = new Position(0, 1);
                    if (position.y + speed.y >= Program.TableHeight - bound)
                        speed = new Position(-1, 0);
                    if (position.x + speed.x < bound)
                        speed = new Position(0, -1);
                    if (position.y + speed.y < bound)
                        speed = new Position(1, 0);

                    position += speed;

                }
                else
                {
                    currentSkip++;
                }
            }

            public Position GetPositon()
            {
                return position;
            }
        }
    }
}
