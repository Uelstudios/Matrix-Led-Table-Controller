#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;
#endregion

namespace MatrixLedTableController.Renderers
{
    class TableRendererFadeCandy : TableRenderer
    {
        TcpClient client;
        NetworkStream stream;
        int ledCount, numBytes, packetLen;

        public TableRendererFadeCandy(int width, int height) : base(width, height)
        {
            int ledCount = width * height;
            int numBytes = 3 * ledCount;
            int packetLen = 4 + numBytes;

            client = new TcpClient();
            client.Connect(Program.GetParameter("fchost", "localhost"),
                Program.GetParameterInt("fcport", 7890));
            stream = client.GetStream();
        }

        public override void Render(PixelColor[,] colorMap)
        {
            if(client != null && client.Connected && stream != null)
            {
                ledCount = colorMap.GetLength(0) * colorMap.GetLength(1);
                numBytes = 3 * ledCount;
                packetLen = 4 + numBytes;
                byte[] packetData = new byte[packetLen];
                packetData[0] = 0;  // Channel
                packetData[1] = 0;  // Command (Set pixel colors)
                packetData[2] = (byte)(numBytes >> 8);
                packetData[3] = (byte)(numBytes & 0xFF);

                //      WRAP: ZigZag
                for (int x = 0; x < colorMap.GetLength(0); x++)
                {
                    for (int y = 0; y < colorMap.GetLength(1); y++)
                    {
                        //int i = y + x * colorMap.GetLength(1);

                        int i = (Mathf.IsOdd(x) ? (colorMap.GetLength(1) - y) - 1 : y) + x * colorMap.GetLength(1);

                       //i = Math.Abs(i - 99);

                        packetData[i * 3 + 4] = (byte)(colorMap[x, y].r);       //  R
                        packetData[i * 3 + 5] = (byte)(colorMap[x, y].g);       //  G
                        packetData[i * 3 + 6] = (byte)(colorMap[x, y].b);       //  B
                    }
                }

                /*      WRAP: Line for Line
                 for (int x = 0; x < colorMap.GetLength(0); x++)
                {
                    for (int y = 0; y < colorMap.GetLength(1); y++)
                    {
                        int i = y + x * colorMap.GetLength(1);
                        
                        packetData[i * 3 + 4] = (byte)(colorMap[x, y].r);       //  R
                        packetData[i * 3 + 5] = (byte)(colorMap[x, y].g);       //  G
                        packetData[i * 3 + 6] = (byte)(colorMap[x, y].b);       //  B
                    }
                }
                 */

                //Send to FadeCandy Server
                stream.Write(packetData, 0, packetData.Length);
                stream.Write(packetData, 0, packetData.Length);
            }
        }

        public override void CleanUp()
        {
            stream.Close();
            client.Close();
        }
    }
}
