using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController
{
    class TableRendererEthernet : TableRenderer
    {
        const string TAG = "TableRendererEthernet";
        private PixelColor[,] preMap;

        private int skipStep = 0;
        private int currentStep = 0;

        TcpCommunicationServer server;

        public TableRendererEthernet(int w, int h) : base(w, h)
        {
            server = new TcpCommunicationServer(25568, false);
            server.LaunchServer();

            skipStep = Program.GetParameterInt("renderskip", 0);
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

            if (preMap == null)
                preMap = PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK);

            bool madeChange = false;
            string outString = string.Empty;

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    if (!preMap[x, y].Same(colorMap[x, y]))
                    {
                        string hexColor = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color
                            .FromArgb(colorMap[x, y].r, colorMap[x, y].g, colorMap[x, y].b));
                        outString += string.Format("{0}{1}.", new object[]
                        {(x + (y * 4)), hexColor});                         //TODO Hardcoded Table Width !!!!

                        madeChange = true;
                        preMap[x, y] = colorMap[x, y];
                    }
                }
            }

            if (madeChange)
            {
                //Send changes over the ethernet
                server.Send(outString);
            }
        }

        public override void CleanUp()
        {
            server.ShutdownServer();
        }
    }
}
