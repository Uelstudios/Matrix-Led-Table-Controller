using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;
using System.Diagnostics;

namespace MatrixLedTableController
{
    class TableRendererRemoteDisplay : TableRenderer
    {
        TcpCommunicationServer server;
        PixelColor[,] preMap;
        Process displayProcess;

        public TableRendererRemoteDisplay(int w, int h) : base(w, h)
        {
            server = new TcpCommunicationServer(25568, true);
            server.LaunchServer();

            string displayPath = @"C:\Users\Paul\Documents\Visual Studio 2015\Projects\TableSimulator\TableSimulator\bin\Debug\TableSimulator.exe";

            if (System.IO.File.Exists(displayPath))
            {
                displayProcess = Process.Start(displayPath);
                Console.CancelKeyPress += OnProcesssExit;
            }
        }

        void OnProcesssExit(object sender, EventArgs e)
        {
            if (displayProcess != null)
                displayProcess.Kill();
        }

        public override void Render(PixelColor[,] colorMap)
        {
            if (preMap == null)
                preMap = PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK);

            bool madeChange = false;
            string outString = string.Empty;
            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!preMap[x, y].Same(colorMap[x, y]))
                    {
                        outString += string.Format("{0};{1}_{2}:{3}:{4}|", new object[]
                        {
                        x,
                        y,
                        colorMap[x, y].r,
                        colorMap[x, y].g,
                        colorMap[x, y].b
                        });
                        madeChange = true;
                        preMap[x, y] = colorMap[x, y];
                    }
                }
            }
            outString = outString.TrimEnd('|').Trim();

            if (madeChange)
            {
                server.Send(outString);
            }
        }
    }
}
