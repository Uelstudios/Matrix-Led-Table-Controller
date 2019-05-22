using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace MatrixLedTableController
{
    public class Mathf
    {
        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static float Clamp01(float value)
        {
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            return value;
        }
    }
}
