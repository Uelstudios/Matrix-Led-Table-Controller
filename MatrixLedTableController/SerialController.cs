using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace MatrixLedTableController
{
    class SerialController
    {
        const string TAG = "SerialController";
        private SerialPort mySerial;
        private string toSend = string.Empty;

        string name;
        int baud;
        Action<string> OnMessageReceived;

        public SerialController(string portName, int baudrate, Action<string> OnMessageReceived)
        {
            this.OnMessageReceived = OnMessageReceived;
            this.name = portName;
            this.baud = baudrate;
        }

        public void Open()
        {
            if(!File.Exists(name) && Program.GetParameterInt("comcheck", 1) == 1)
            {
                Program.Log(TAG, "Port " + name + " does not exist!");
                return;
            }

            mySerial = new SerialPort(name, baud, Parity.None, 8, StopBits.One);

            Program.Log(TAG, "Opening Port '" + name + "' with a BaudRate of " + baud + ".");

            try
            {
                mySerial.Open();
            }
            catch
            {
                Program.Log(TAG, "Opening port failed.");
                return;
            }

            Program.Log(TAG, "Port opened. Ready for incoming data!");

            while (true)
            {
                try
                {
                    if (mySerial.BytesToRead > 0)
                    {
                        string input = mySerial.ReadLine();
                        Program.Log(TAG + "/In", input);
                        OnMessageReceived(input);
                    }

                    if (!string.IsNullOrEmpty(toSend))
                    {
                        Program.LogFullLength(TAG + "/Out", toSend);
                        mySerial.Write(toSend);
                        toSend = string.Empty;
                    }
                }
                catch (TimeoutException ex)
                {
                    Program.Log(TAG, "Error:" + ex.Message);
                    break;
                }
            }

            Program.Log(TAG, "Connection ended.");
        }

        public void Write(string message)
        {
            toSend += message;
        }
    }
}
