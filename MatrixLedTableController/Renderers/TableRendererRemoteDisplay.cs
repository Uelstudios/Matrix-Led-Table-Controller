using System;
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

            string displayPath = @"D:\Projekte\Visual Studio\TableSimulator\TableSimulator\bin\Debug\TableSimulator.exe";

            /*
            if (System.IO.File.Exists(displayPath))
            {
                displayProcess = Process.Start(displayPath);
                Console.CancelKeyPress += OnProcesssExit;
            }
            */
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
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (!preMap[x, y].Same(colorMap[x, y]))
                    {
                        preMap[x, y] = colorMap[x, y];
                        if (Mathf.IsOdd(y))
                        {
                            y = height - y;
                        }

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

        public override void CleanUp()
        {
            server.ShutdownServer();
        }
    }
}
