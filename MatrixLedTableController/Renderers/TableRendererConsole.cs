using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;
using System.Drawing;
using Console = Colorful.Console;

namespace MatrixLedTableController
{
    class TableRendererConsole : TableRenderer
    {
        public TableRendererConsole(int w, int h) : base(w, h) { }

        public override void Render(PixelColor[,] colorMap)
        {
            Console.Clear();

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Color c = Color.FromArgb(colorMap[w, h].r, colorMap[w, h].g, colorMap[w, h].b);
                    Console.Write('#', c);
                }
                Console.Write(Environment.NewLine, Color.Transparent);
                
            }
        }
    }
}
