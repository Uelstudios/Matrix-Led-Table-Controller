using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;
using System.Drawing;

namespace MatrixLedTableController
{
    class TableRendererConsole : TableRenderer
    {
        public TableRendererConsole(int w, int h) : base(w, h) { }

        public override void CleanUp()
        {
            Console.Clear();
        }

        public override void Render(PixelColor[,] colorMap)
        {
            Console.Clear();
            Console.WriteLine("The TableRendererConsole is not available anymore.");
        }
    }
}
