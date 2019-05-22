using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController
{
    public abstract class TableRenderer
    {
        public int width, height;

        public TableRenderer(int width, int height)
        {
            this.width = width;
            this.height = height;

            Program.Log("TableRenderer", "Selected Renderer: " + ToString());
            Program.Log("TableRenderer", ".OK");
        }

        public abstract void Render(PixelColor[,] colorMap);

        public abstract void CleanUp();
    }
}
