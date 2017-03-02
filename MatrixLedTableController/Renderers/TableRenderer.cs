using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController
{
    public class TableRenderer
    {
        public int width, height;

        public TableRenderer(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public virtual void Render(PixelColor[,] colorMap) 
        {
            //Not Implemented
        }
    }
}
