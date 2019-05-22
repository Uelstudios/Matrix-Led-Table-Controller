using System;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MatrixLedTableController.Apps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MatrixLedTableController.Web
{
    class WebSocketServer
    {
        const string TAG = "WebSocket";

        TcpListener tcpListener;
        List<WebSocketSession> sessions;

        public WebSocketServer()
        {
            IPAddress ip = IPAddress.Any;
            int port = 25560;
            tcpListener = new TcpListener(ip, port);
            sessions = new List<WebSocketSession>();

            tcpListener.Start();
            Program.Log(TAG, string.Format("Server has started on {0}:{1}. Waiting for a connection...", ip, port));

            new Thread(delegate ()
            {
                while (true)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();

                    Program.Log(TAG, string.Format("A client connected.", Environment.NewLine));

                    WebSocketSession wss = new WebSocketSession(client, delegate (WebSocketSession session)
                    {
                        sessions.Remove(session);
                        Program.Log(TAG, string.Format("A client disconnected.", Environment.NewLine));
                    });

                    wss.UpdateAppList(Program.tableAppManager.GetApps());
                    sessions.Add(wss);
                }
            }).Start();

            //Program.Log(TAG, string.Format("A client disconnected.", Environment.NewLine));
        }

        public void Broadcast(string message)
        {
            foreach (WebSocketSession wss in sessions)
            {
                wss.Send(message);
            }
        }
    }

    class WebSocketSession
    {
        const string TAG = "WebSocket-Connection";

        Thread connectionThread;
        List<string> messagesToSend = new List<string>();

        Action<WebSocketSession> OnDisconnected;

        public WebSocketSession(TcpClient client, Action<WebSocketSession> OnDisconnected)
        {
            this.OnDisconnected = OnDisconnected;

            connectionThread = GetConnectionThread(client);
            connectionThread.Start();
        }

        public Thread GetConnectionThread(TcpClient client)
        {
            return new Thread(delegate ()
            {
                NetworkStream stream = client.GetStream();
                while (client.Connected && SocketConnected(client.Client))
                {
                    if (stream.DataAvailable)
                    {
                        byte[] bytes = new byte[client.Available];

                        stream.Read(bytes, 0, bytes.Length);

                        string data = Encoding.UTF8.GetString(bytes);

                        if (Regex.IsMatch(data, "^GET"))
                        {
                            byte[] response = Encoding.UTF8.GetBytes(
                                "HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                                + "Connection: Upgrade" + Environment.NewLine
                                + "Upgrade: websocket" + Environment.NewLine
                                + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                                    SHA1.Create().ComputeHash(
                                        Encoding.UTF8.GetBytes(
                                            new Regex("Sec-WebSocket-Key: (.*)").Match(data).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                        )
                                    )
                                ) + Environment.NewLine + Environment.NewLine);

                            stream.Write(response, 0, response.Length);
                        }
                        else
                        {
                            string msg = DecodeMessage(bytes);
                            Program.LogFullLength(TAG, msg);
                            HandleInput(msg);
                        }
                    }
                    else if (messagesToSend.Count > 0)
                    {
                        string message = messagesToSend[0];
                        messagesToSend.RemoveAt(0);

                        byte[] bytes = EncodeMessageToSend(message);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }

                if (OnDisconnected != null)
                    OnDisconnected.Invoke(this);
            });
        }

        public void Send(string message)
        {
            messagesToSend.Add(message);
        }

        public void HandleInput(string json)
        {
            try
            {
                JObject data = JObject.Parse(json);

                switch ((string)data.GetValue("message_type"))
                {
                    case "start_app":
                        TableApp app = TableAppManager.GetAppById((string)data.GetValue("app"));
                        Program.tableAppManager.LaunchApp(app);

                        CustomInterface customInterface = app.GetCustomInterface();
                        Send("{\"message_type\": \"show_interface\", \"app\": {\"name\": \"" + app.GetName().Replace("TableApp", "") + "\", \"id\": \"" + app.GetName() + "\"}, \"interface\": {\"items\": " + customInterface.toJson() + "}}");
                        break;
                    case "input":
                        JObject input = JObject.Parse(data["input"].ToString());

                        Program.tableAppManager.GetCurrentApp()
                            .OnCustomInterfaceInput((string)input.GetValue("id"), (string)input.GetValue("value"));
                        break;
                }
            }
            catch (Exception e)
            {
                Program.LogFullLength(TAG, "Parsing received message failed! " + e.ToString());
            }
        }

        public void UpdateAppList(TableApp[] apps)
        {
            string json = string.Empty;
            json += "{\"message_type\": \"app_list\", \"apps\": [";

            for (int i = 0; i < apps.Length; i++)
            {
                TableApp app = apps[i];
                if (!app.selectable) continue;

                var name = app.GetName().Replace("TableApp", "");
                StringBuilder newText = new StringBuilder(name.Length * 2);
                newText.Append(name[0]);
                for (int j = 1; j < name.Length; j++)
                {
                    if (char.IsUpper(name[j]) && name[j - 1] != ' ')
                        newText.Append(' ');
                    newText.Append(name[j]);
                }

                name = newText.ToString();

                json += "{\"id\": \"" + app.GetName() + "\", \"name\": \"" + name + "\", \"features\": " + JObject.FromObject(app.GetFeatures()).ToString() + "}";
                if (i < apps.Length - 1) json += ",";
            }

            json += "]}";

            Send(json);
        }

        private String DecodeMessage(Byte[] bytes)
        {
            String incomingData = String.Empty;
            Byte secondByte = bytes[1];
            Int32 dataLength = secondByte & 127;
            Int32 indexFirstMask = 2;
            if (dataLength == 126)
                indexFirstMask = 4;
            else if (dataLength == 127)
                indexFirstMask = 10;

            IEnumerable<Byte> keys = bytes.Skip(indexFirstMask).Take(4);
            Int32 indexFirstDataByte = indexFirstMask + 4;

            Byte[] decoded = new Byte[bytes.Length - indexFirstDataByte];
            for (Int32 i = indexFirstDataByte, j = 0; i < bytes.Length; i++, j++)
            {
                decoded[j] = (Byte)(bytes[i] ^ keys.ElementAt(j % 4));
            }

            return incomingData = Encoding.UTF8.GetString(decoded, 0, decoded.Length);
        }

        private static Byte[] EncodeMessageToSend(String message)
        {
            Byte[] response;
            Byte[] bytesRaw = Encoding.UTF8.GetBytes(message);
            Byte[] frame = new Byte[10];

            Int32 indexStartRawData = -1;
            Int32 length = bytesRaw.Length;

            frame[0] = (Byte)129;
            if (length <= 125)
            {
                frame[1] = (Byte)length;
                indexStartRawData = 2;
            }
            else if (length >= 126 && length <= 65535)
            {
                frame[1] = (Byte)126;
                frame[2] = (Byte)((length >> 8) & 255);
                frame[3] = (Byte)(length & 255);
                indexStartRawData = 4;
            }
            else
            {
                frame[1] = (Byte)127;
                frame[2] = (Byte)((length >> 56) & 255);
                frame[3] = (Byte)((length >> 48) & 255);
                frame[4] = (Byte)((length >> 40) & 255);
                frame[5] = (Byte)((length >> 32) & 255);
                frame[6] = (Byte)((length >> 24) & 255);
                frame[7] = (Byte)((length >> 16) & 255);
                frame[8] = (Byte)((length >> 8) & 255);
                frame[9] = (Byte)(length & 255);

                indexStartRawData = 10;
            }

            response = new Byte[indexStartRawData + length];

            Int32 i, reponseIdx = 0;

            //Add the frame bytes to the reponse
            for (i = 0; i < indexStartRawData; i++)
            {
                response[reponseIdx] = frame[i];
                reponseIdx++;
            }

            //Add the data bytes to the response
            for (i = 0; i < length; i++)
            {
                response[reponseIdx] = bytesRaw[i];
                reponseIdx++;
            }

            return response;
        }

        bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
    }
}
