using System;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Diagnostics;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController
{
    public partial class TcpCommunicationServer
    {
        private TcpListener tcpServer;
        private bool hosting = false;
        private bool hasClient = false;
        private int port;
        private bool muted;

        private byte[] payload;

        public TcpCommunicationServer(int port, bool muted)
        {
            this.port = port;
            this.muted = muted;

            PrintConsole(Sender.Server, ".OK");
        }

        public void LaunchServer()
        {
            if (hosting)
            {
                PrintConsole(Sender.Server, "!Already hosting!");
                return;
            }


            //Start Server
            PrintConsole(Sender.Server, "Launching server.");
            IPAddress ip = IPAddress.Any;
            tcpServer = new TcpListener(ip, port);
            tcpServer.Start();
            hosting = true;
            PrintConsole(Sender.Server, string.Format("Server launched (ip:{0}). Now listening on port {1}.", GetLocalIPAddress(), port));

            Thread serverThread = new Thread(delegate ()
            {
                while (hosting)
                {
                    PrintConsole(Sender.Server, "Awaiting client.");
                    //Waiting for a client
                    TcpClient client;
                    try
                    {
                        client = tcpServer.AcceptTcpClient();
                        string clientIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                        PrintConsole(Sender.Server, "Client connected. (" + clientIp + ")");
                        hasClient = true;
                    }
                    catch (Exception e)
                    {
                        PrintConsole(Sender.Server, e.Message);
                        ShutdownServer();
                        break;
                    }

                    Thread clientConnection = new Thread(delegate ()
                    {
                        NetworkStream stream = client.GetStream();

                        while (hosting && SocketConnected(client.Client))
                        {
                            try
                            {
                                if (stream.DataAvailable)
                                {
                                    Byte[] data = new byte[2048];
                                    Int32 bytes = stream.Read(data, 0, data.Length);
                                    string clientData = Encoding.UTF8.GetString(data, 0, bytes);
                                    if (clientData.Length > 2)
                                    {
                                        if (!clientData.Contains("input") || Program.GetParameter("input-verbose", "false") == "true")
                                            PrintConsole(Sender.Client, clientData);
                                        ExecuteCommand(clientData);
                                    }
                                }


                                if (payload != null && payload.Length > 0)
                                {
                                    stream.Write(payload, 0, payload.Length);
                                    stream.Flush();
                                    payload = null;
                                }
                            }
                            catch (Exception ex)
                            {
                                PrintConsole(Sender.Server, ex.Message);
                            }
                        }
                        if (hasClient)
                            client.Close();
                        hasClient = false;

                        //Program.tableAppManager.LaunchApp(new TableAppIdle());
                        PrintConsole(Sender.Server, "Client disconnected.");
                    });
                    clientConnection.Start();
                }

                //Program.tableAppManager.LaunchApp(new TableAppIdle());
                PrintConsole(Sender.Server, "Server is now offline.");
                //Environment.Exit(0);
            });
            serverThread.Start();
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Local IP Address Not Found!";
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

        public void ShutdownServer()
        {
            Send("Server is shutting down.");

            new Thread(delegate ()
            {
                Program.tableAppManager.LaunchApp(null);
                Thread.Sleep(50);
                Program.tableRenderer.Render(PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK));
                Thread.Sleep(50);
                hosting = false;
                tcpServer.Server.Close();
                tcpServer.Stop();
                Thread.Sleep(100);

                try
                {
                    Process.Start(new ProcessStartInfo() { FileName = "sudo", Arguments = "shutdown -P now" });
                }
                catch (Exception e) { }
                try
                {
                    var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    Process.Start(psi);
                }
                catch (Exception e) { }

                Environment.Exit(0);
            }).Start();
        }

        public void Send(string message)
        {
            if (!hosting)
            {
                PrintConsole(Sender.Server, "!Server isn\'t running!");
                return;
            }

            payload = Encoding.UTF8.GetBytes(message);
            if (!muted)
                PrintConsole(Sender.Server, message);
        }

        enum Sender { Server, Client }
        private void PrintConsole(Sender sender, string message)
        {
            string prefix = sender == Sender.Client ? "Client" : "Server";
            Program.LogFullLength(prefix, string.Format(":{0} >{1}", port, message.Trim()));
        }

        public void ExecuteCommand(string command)
        {
            command = command.Trim();
            string argument = string.Empty;
            if (command.Contains(' '))
            {
                argument = command.Substring(command.IndexOf(' '));
                command = command.Substring(0, command.IndexOf(' '));
            }

            if (command.StartsWith("GET"))
            {
                string key = "";
                string[] headerLines = argument.Split('\n');
                foreach(string line in headerLines)
                {
                    if (line.Contains("Sec-WebSocket-Key:"))
                    {
                        key = line.Replace("Sec-WebSocket-Key: ", "").Trim();
                        break;
                    }
                }

                Program.LogFullLength("MAGIC", "The WebSocket Sec-Key is: '" + key + "'");

                Send("HTTP/1.1 101 Switching Protocols\r\n" +
                    "Upgrade: websocket\r\n" +
                    "Connection: Upgrade\r\n" +
                    "Sec-WebSocket-Accept: " + AcceptKey(ref key) + "\r\n\r\n");
            }
            else if (command.Contains("stop"))
            {
                ShutdownServer();
            }
            else if (command.Contains("serial") && !string.IsNullOrWhiteSpace(argument))
            {
                Program.serialController.Write(argument.Trim());
            }
            else if (command.Contains("init"))
            {
                Program.tableAppManager.LaunchApp(new TableAppIdle());

                string appList = string.Empty;
                for (int i = 0; i < TableAppManager.apps.Length; i++)
                {
                    TableApp a = TableAppManager.apps[i];
                    if (a.selectable)
                        appList += a.GetName() + ":" + (int)a.userInterface + "|";
                }


                Send("setup " + appList.Trim('|').Trim());
            }
            else if (command.Contains("input") && !string.IsNullOrWhiteSpace(argument))
                Program.TriggerInput(argument.Trim());
            else if (command.Contains("app") && !string.IsNullOrWhiteSpace(argument))
                Program.tableAppManager.LaunchApp(Apps.TableAppManager.GetAppById(argument.Trim()));
            else if (command.Contains("renderer") && !string.IsNullOrWhiteSpace(argument))
                Program.CreateRenderer(argument.Trim());
            else if (command.Contains("help"))
            {
                Send("Help - List of commands\n");
                Send("stop                   -       Shutdown the server\n");
                Send("serial [command]       -       Run a serial command\n");
                Send("init                   -       Get a list of apps\n");
                Send("input [input]          -       Triggers an input event. e.g. pad_start\n");
                Send("app [name]             -       Starts an app. e.g. TableAppIdle\n");
                Send("renderer [name]        -       Changes the renderer. e.g. remote\n");
                Send("help                   -       Shows this list of commands\n");
                Send("#######################");
            }
            else
                Send("Command " + command + " not found. Run 'help' for a list of commands");
        }

        static private string guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        private string AcceptKey(ref string key)
        {
            string longKey = key + guid;
            SHA1 sha1 = SHA1CryptoServiceProvider.Create();
            byte[] hashBytes = sha1.ComputeHash(System.Text.Encoding.ASCII.GetBytes(longKey));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
