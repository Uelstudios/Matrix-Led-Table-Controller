using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController
{
    static class Program
    {
        static readonly string TAG = "Program";
        static string[] programArgs;

        public static SerialController serialController;
        public static TcpCommunicationServer communicationServer;
        public static TableAppManager tableAppManager;
        public static TableRenderer tableRenderer;
        public static TouchManager touchManager;

        public const int TableWidth = 10;
        public const int TableHeight= 10;

        static string MUTED_TAG = "";

        static void Main(string[] args)
        {
            //Extras
            programArgs = args;
            MUTED_TAG = GetParameter("mute", "");

            //Create TouchManager
            touchManager = new TouchManager(TableWidth, TableHeight);

            //Create Serial communication
            serialController = new SerialController(GetParameter("com", "/dev/ttyACM0"), GetParameterInt("baud", 9600), touchManager.OnRawInput);

            //Create Renderer
            string renderString = GetParameter("renderer", "serialcomp");
            if (renderString == "remote")
            {
                tableRenderer = new TableRendererRemoteDisplay(TableWidth, TableHeight);
            }
            else if (renderString == "console")
            {
                tableRenderer = new TableRendererConsole(TableWidth, TableHeight);
            }
            else if (renderString == "serial")
            {
                tableRenderer = new TableRendererSerial(TableWidth, TableHeight);
            }
            else if(renderString == "ethernet")
            {
                tableRenderer = new TableRendererEthernet(TableWidth, TableHeight);
            }
            else if (renderString == "serialcomp")
            {
                tableRenderer = new TableRendererSerialCompress(TableWidth, TableHeight);
            }
            else
            {
                tableRenderer = new TableRenderer(TableWidth, TableHeight);
            }


            //Launch Server
            Log(TAG, "Starting TcpCommunicationServer...");
            int port = GetParameterInt("port", 25564);
            communicationServer = new TcpCommunicationServer(port, false);
            communicationServer.LaunchServer();

            //Create Appmanager
            Log(TAG, "Starting TableAppManager...");
            tableAppManager = new TableAppManager();
            tableAppManager.LaunchApp(TableAppManager.GetAppById(GetParameter("app", new TableAppIdle().GetName())));




            serialController.Open();
        }

        public static string GetParameter(string key, string defValue)
        {
            key = "-" + key;
            for (int i = 0; i < programArgs.Length; i++)
            {
                if (programArgs[i] == key)
                {
                    if(programArgs.Length >= i)
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
            if(int.TryParse(GetParameter(key, "nixda"), out i))
                return i;
            return defValue;
        }

        public static void TriggerInput(string key)
        {
            TableApp.InputKey inputKey = TableApp.GetCorrespondingKey(key);
            if(inputKey != TableApp.InputKey.Unknown)
                tableAppManager.GetCurrentApp().OnInputMade(inputKey);
            else
            {
                tableAppManager.GetCurrentApp().OnRawInput(key);
            }
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
        }

        public static void Log(string tag, string message)
        {
            if(message.Length > 75)
                message = message.Substring(0, 75) + "(..)";
            LogFullLength(tag, message);
        }

        static Dictionary<string, ConsoleColor> colorDictionary;
        private static ConsoleColor GetNextConsoleColor(ConsoleColor[] usedColors)
        {
            while(true)
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
