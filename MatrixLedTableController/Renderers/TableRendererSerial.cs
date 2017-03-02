using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController
{
    class TableRendererSerial : TableRenderer
    {
        const string TAG = "TableRendererSerial";
        private PixelColor[,] preMap;

        private int skipStep = 0;
        private int currentStep = 0;

        private int currentRow = 0;

        public TableRendererSerial(int w, int h) : base(w, h)
        {
            skipStep = Program.GetParameterInt("serialrate", 3);
        }

        public override void Render(PixelColor[,] colorMap)
        {
            if(currentStep < skipStep)
            {
                currentStep++;
                return;
            }
            else
                currentStep = 0;

            if (preMap == null)
                preMap = PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK);

            bool madeChange = false;
            string outString = string.Empty;

            for (int x = 0; x < 4; x++)
            {
                if (!preMap[x, currentRow].Same(colorMap[x, currentRow]))
                {
                    string hexColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color
                        .FromArgb(colorMap[x, currentRow].r, colorMap[x, currentRow].g, colorMap[x, currentRow].b));
                    outString += string.Format("{0}{1}.", new object[]
                    {(x + (currentRow * 4)), hexColor});                         //TODO Hardcoded Table Width !!!!

                    madeChange = true;
                    preMap[x, currentRow] = colorMap[x, currentRow];
                }
            }

            if (madeChange)
            {
                //Send changes to arduino
                Program.serialController.Write(outString);
            }


            //Cycle trough all rows
            currentRow++;
            if (currentRow > 1)
                currentRow = 0;
        }

    }
}
