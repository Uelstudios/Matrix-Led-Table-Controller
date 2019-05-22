using System;
using System.Runtime;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;
using MatrixLedTableController.Renderers;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Diagnostics;
using MatrixLedTableController.Web;

namespace MatrixLedTableController
{
    static class Program
    {
        static readonly string TAG = "Program";
        static string[] programArgs;

        public static SerialController serialController;
        public static TcpCommunicationServer communicationServer;
        public static WebSocketServer webSocketServer;
        public static TableAppManager tableAppManager;
        public static TableRenderer tableRenderer;
        public static TouchManager touchManager;
        public static GamepadManager gamepadManager;

        public const int TableWidth = 10;
        public const int TableHeight = 10;

        static string MUTED_TAG = "";

        public static Random random;

        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;

            //logName = startTime.ToString("dd.MM.yy H-mm-ss");
            //if (!System.IO.Directory.Exists("log/"))
            //    System.IO.Directory.CreateDirectory("logs/");

            LogFullLength(TAG, "System is now starting");

            //Extras
            programArgs = args;
            MUTED_TAG = GetParameter("mute", "");
            random = new Random();

            //Create TouchManager
            touchManager = new TouchManager(TableWidth, TableHeight);

            //Create Serial communication
            // /dev/ttyACM0
            serialController = new SerialController(GetParameter("com", "COM5"), GetParameterInt("baud", 57600), touchManager.OnRawInput);

            //Create Renderer
            string renderString = GetParameter("renderer", "fadecandy");
            CreateRenderer(renderString);

            //Launch Server
            int port = GetParameterInt("port", 25564);
            //communicationServer = new TcpCommunicationServer(port, false);
            //communicationServer.LaunchServer();

            //Launch WebSocket server
            webSocketServer = new WebSocketServer();

            //Create Appmanager
            tableAppManager = new TableAppManager();
            tableAppManager.LaunchApp(TableAppManager.GetAppById(GetParameter("app", new TableAppIdle().GetName())));


            //Open Serial communication
            serialController.Open();

            //Gamepads
            gamepadManager = new GamepadManager();

            LogFullLength(TAG, "System successfully started!");
            LogFullLength(TAG, "Time passed " + (DateTime.Now - startTime).TotalSeconds + " Seconds");

           // ConsoleInputManagment();
            
        }

        static string lastCommand = "";
        public static void ConsoleInputManagment()
        {
            string consolePrompt = string.Format("led-table/{0}>", Environment.UserName);

            new Thread(delegate () 
            {
                while (true)
                {
                    string command = "";
                    Console.Write(consolePrompt);
                    command = Console.ReadLine();

                    if (DoConsoleCommand(command))
                    {
                        //Success
                    }
                    else
                    {
                        break;
                    }
                }
            }).Start();
        }

        static bool DoConsoleCommand(string command)
        {
            if (command == "exit")
            {
                Environment.Exit(0);
                return false;
            }
            else if (command == "shutdown")
            {
                Console.WriteLine("Shutting down the system...");
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
                return false;
            }
            else if (command.StartsWith("exec"))
            {
                if (communicationServer != null)
                    communicationServer.ExecuteCommand(command.Replace("exec", "").Trim());
                else
                    Console.WriteLine("The Communication Server is offline. Sorry.");
            }
            else if (command.StartsWith("clear"))
            {
                Console.Clear();
            }
            else if (command.StartsWith("echo"))
            {
                Console.WriteLine(command.Replace("echo", "").Trim());
            }
            else if (command.StartsWith("log"))
            {
                string userName = WindowsIdentity.GetCurrent().Name;
                LogFullLength(userName, command.Replace("log", "").Trim());
            }
            else if (command.StartsWith("whoami"))
            {
                Console.WriteLine(Environment.UserName);
            }
            else if (command.StartsWith("uptime"))
            {
                Console.WriteLine(Environment.TickCount + " ticks");
            }
            else if (command.StartsWith("tcp"))
            {
                communicationServer.Send(command.Replace("tcp", "").Trim());
            }
            else if (command.StartsWith("ws"))
            {
                webSocketServer.Broadcast(command.Replace("ws", "").Trim());
            }
            else if (command.StartsWith("beep"))
            {
                Console.Beep();
            }
            else if (command.StartsWith("play"))
            {
                string path = command.Replace("play", "").Trim();
            }
            else if (command.StartsWith("."))
            {
                if (lastCommand.Length > 0)
                {
                    Console.WriteLine("led-table: rep>" + lastCommand);
                    DoConsoleCommand(lastCommand);
                }
                return true;
            }
            else if (command.StartsWith("status"))
            {
                Console.WriteLine("### SYSTEM STATUS ###");
                Console.WriteLine("Renderer: " + tableRenderer.GetType().ToString());
                Console.WriteLine("App: " + tableAppManager.GetCurrentApp().GetName());
            }
            else if (command == "help" || command == "?")
            {
                Console.WriteLine("## HELP ##");
                Console.WriteLine("- help/?");
                Console.WriteLine("- exec [command]");
                Console.WriteLine("- log [msg]");
                Console.WriteLine("- whoami");
                Console.WriteLine("- uptime");
                Console.WriteLine("- tcp [msg]");
                Console.WriteLine("- ws [msg]");
                Console.WriteLine("- echo [msg]");
                Console.WriteLine("- clear");
                Console.WriteLine("- beep");
                Console.WriteLine("- status");
                Console.WriteLine("- exit");
                Console.WriteLine("##########");
            }

            lastCommand = command;
            return true;
        }

        public static void CreateRenderer(string rendererName)
        {
            if (tableRenderer != null) tableRenderer.CleanUp();

            if (rendererName == "remote")
            {
                tableRenderer = new TableRendererRemoteDisplay(TableWidth, TableHeight);
            }
            else if (rendererName == "console")
            {
                tableRenderer = new TableRendererConsole(TableWidth, TableHeight);
            }
            else if (rendererName == "serial")
            {
                tableRenderer = new TableRendererSerial(TableWidth, TableHeight);
            }
            else if (rendererName == "ethernet")
            {
                tableRenderer = new TableRendererEthernet(TableWidth, TableHeight);
            }
            else if (rendererName == "serialcomp")
            {
                tableRenderer = new TableRendererSerialCompress(TableWidth, TableHeight);
            }
            else if (rendererName == "fadecandy")
            {
                tableRenderer = new TableRendererFadeCandy(TableWidth, TableHeight);
            }
            else
            {
                tableRenderer = new TableRendererRemoteDisplay(TableWidth, TableHeight);
            }
        }

        public static string GetParameter(string key, string defValue)
        {
            key = "-" + key;
            for (int i = 0; i < programArgs.Length; i++)
            {
                if (programArgs[i] == key)
                {
                    if (programArgs.Length >= i)
                    {
                        return programArgs[i + 1];
                    }
                }
            }
            return defValue;
        }

        public static int GetParameterInt(string key, int defValue)
        {
            int i;
            if (int.TryParse(GetParameter(key, "nixda"), out i))
                return i;
            return defValue;
        }

        public static void TriggerInput(string key)
        {
            TableApp.InputKey inputKey = TableApp.GetCorrespondingKey(key);
            if (inputKey != TableApp.InputKey.Unknown)
            {
                tableAppManager.GetCurrentApp().OnInputMade(inputKey);
            }
            else if (key.Contains("custom"))
            {
                string[] keyValue = key.Split('.')[1].Split('=');
                tableAppManager.GetCurrentApp().OnCustomInterfaceInput(keyValue[0], keyValue[1]);
            }
            else
            {
                tableAppManager.GetCurrentApp().OnRawInput(key);
            }
        }

        public static void GamepadInput(int controller, GamepadManager.GamepadKey key)
        {
            Log(controller.ToString(), key.ToString());
            tableAppManager.GetCurrentApp().OnControllerInput(controller, key);
        }

        public static void LogFullLength(string tag, string message)
        {
            if (tag.Equals(MUTED_TAG))
                return;

            if (colorDictionary == null)
            {
                colorDictionary = new Dictionary<string, ConsoleColor>();
                colorDictionary.Add("Program", ConsoleColor.Cyan);
                colorDictionary.Add("Server", ConsoleColor.Yellow);
                colorDictionary.Add("Client", ConsoleColor.Green);
                colorDictionary.Add("TableAppManager", ConsoleColor.DarkRed);
                colorDictionary.Add("SerialController", ConsoleColor.Magenta);
            }

            if (!colorDictionary.ContainsKey(tag))
                colorDictionary.Add(tag, GetNextConsoleColor(colorDictionary.Values.ToArray()));

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(string.Format("{0} ", DateTime.Now.ToLongTimeString()));
            Console.ForegroundColor = colorDictionary[tag];
            Console.Write("[" + tag + "] ");

            Console.WriteLine(message);
            Console.ResetColor();

            //string fileMsg = string.Format("{0} ", DateTime.Now.ToLongTimeString()) + "[" + tag + "]" + message + Environment.NewLine;
            //System.IO.File.AppendAllText("logs/" + logName + ".txt", fileMsg);
        }

        public static void Log(string tag, string message)
        {
            if (message.Length > 75)
                message = message.Substring(0, 75) + "(..)";
            LogFullLength(tag, message);
        }

        static Dictionary<string, ConsoleColor> colorDictionary;
        private static ConsoleColor GetNextConsoleColor(ConsoleColor[] usedColors)
        {
            while (true)
            {
                ConsoleColor c = GetRandomConsoleColor();
                if (!usedColors.Contains(c))
                    return c;
            }
        }
        private static ConsoleColor GetRandomConsoleColor()
        {
            ConsoleColor[] cols =
            {
                ConsoleColor.Blue,
                ConsoleColor.Cyan,
                ConsoleColor.DarkCyan,
                ConsoleColor.DarkGreen,
                ConsoleColor.DarkMagenta,
                ConsoleColor.DarkRed,
                ConsoleColor.DarkYellow,
                ConsoleColor.Green,
                ConsoleColor.Magenta,
                ConsoleColor.Red,
                ConsoleColor.Yellow
            };
            return cols[new Random().Next(0, cols.Length)];
        }
    }
}
