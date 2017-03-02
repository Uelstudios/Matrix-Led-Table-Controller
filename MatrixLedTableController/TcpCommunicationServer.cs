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
        private string payloadData = string.Empty;
        private int port;
        private bool muted;

        public TcpCommunicationServer (int port, bool muted)
        {
            this.port = port;
            this.muted = muted;
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
                                PrintConsole(Sender.Client, clientData);
                                if (clientData[0] == '/')
                                    ExecuteCommand(clientData.Substring(1));
                            }


                            if (!string.IsNullOrEmpty(payloadData))
                            {
                                byte[] payload = Encoding.UTF8.GetBytes(payloadData);
                                stream.Write(payload, 0, payload.Length);
                                stream.Flush();
                                payloadData = string.Empty;
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

                    Program.tableAppManager.LaunchApp(new TableAppIdle());
                    PrintConsole(Sender.Server, "Client disconnected.");
                }

                Program.tableAppManager.LaunchApp(new TableAppIdle());
                PrintConsole(Sender.Server, "Server is now offline.");
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

        private void ShutdownServer()
        {
            Send("Server is shutting down.");
            hosting = false;
            tcpServer.Stop();
        }

        public void Send(string message)
        {
            if (!hosting)
            {
                PrintConsole(Sender.Server, "!Server isn\'t running!");
                return;
            }

            payloadData = message;
            if(!muted)
                PrintConsole(Sender.Server, message);
        }

        enum Sender { Server, Client }
        private void PrintConsole(Sender sender, string message)
        {
            string prefix = sender == Sender.Client ? "Client" : "Server";
            Program.Log(prefix, message.Trim());
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

            if (command.Contains("Stop"))
                ShutdownServer();
            else if(command.Contains("Serial") && !string.IsNullOrWhiteSpace(argument))
            {
                Program.serialController.Write(argument.Trim());
            }
            else if(command.Contains("Init"))
            {
                Program.tableAppManager.LaunchApp(new TableAppIdle());

                string appList = string.Empty;
                for(int i = 0; i < TableAppManager.apps.Length; i++)
                {
                    TableApp a = TableAppManager.apps[i];
                    if(a.selectable)
                        appList += a.GetName() + ":" + (int)a.userInterface + "|";
                }


                Send("/Setup " + appList.Trim('|').Trim());
            }
            else if (command.Contains("Input") && !string.IsNullOrWhiteSpace(argument))
                Program.TriggerInput(argument.Trim());
            else if (command.Contains("App") && !string.IsNullOrWhiteSpace(argument))
                Program.tableAppManager.LaunchApp(Apps.TableAppManager.GetAppById(argument.Trim()));
            else
                Send("Command " + command + " not found.");
        }
    }
}
