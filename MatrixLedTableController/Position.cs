using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController
{
    public struct Position
    {
        public int x, y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IsTheSame(Position pos)
        {
            return x == pos.x && y == pos.y;
        }

        public static Position operator +(Position x, Position y)
        {
            return new Position(x.x + y.x, x.y + y.y);
        }

        public static Position operator -(Position x, Position y)
        {
            return new Position(x.x - y.x, x.y - y.y);
        }

        public override string ToString()
        {
            return string.Format("Position(x:{0}; y:{1})", x, y);
        }

        public void RotateAround(Position center, int angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            this = new Position
            {
                x =
                    (int)
                    (cosTheta * (x - center.x) -
                    sinTheta * (y - center.y) + center.x),
                y =
                    (int)
                    (sinTheta * (x - center.x) +
                    cosTheta * (y - center.y) + center.y)
            };
        }
    }
}
