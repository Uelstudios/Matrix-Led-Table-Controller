using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController
{
    class TableRendererSerialCompress : TableRenderer
    {
        const string TAG = "TableRendererSerialCompress";
        private PixelColor[,] preMap;

        private int skipStep = 0;
        private int currentStep = 0;
        private int trasmitPart = 0;

        public TableRendererSerialCompress(int width, int height) : base(width, height)
        {
            skipStep = Program.GetParameterInt("serialrate", -1);
        }

        public override void Render(PixelColor[,] colorMap)
        {
            if (currentStep < skipStep)
            {
                currentStep++;
                return;
            }
            else
                currentStep = 0;


            List<DataFragment> fragments = new List<DataFragment>();
            Random rand = new Random();

            int startIndex = 0;
            int endIndex = 100;

            /*
            trasmitPart++;
            if (trasmitPart >= 4)
                trasmitPart = 0;
                */

            for (int i = startIndex; i < endIndex; i++)
            {
                fragments.Add(new DataFragment(i, rand.Next(0, 99)));
            }

            string output = string.Empty;
            foreach (DataFragment f in fragments)
            {
                //output += string.Format("{1}", f.pixelIndex.ToString("D2"), f.colorIndex.ToString("D2"));
                //Program.serialController.Write(f.colorIndex.ToString("D2"));
                output += f.colorIndex.ToString("D2");
            }

            if (trasmitPart == 3)
                output += "D";

            //Program.serialController.Write(output);
            Program.serialController.Write(output + "D");
        }
    }
    
    public struct DataFragment
    {
        public int colorIndex;
        public int pixelIndex;

        public DataFragment(int index, int color)
        {
            pixelIndex = index;
            colorIndex = color;
        }
    }
}
