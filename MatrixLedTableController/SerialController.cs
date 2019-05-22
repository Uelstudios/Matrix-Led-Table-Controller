using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

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
            if (!File.Exists(name) && Program.GetParameterInt("comcheck", 0) == 1)
            {
                Program.Log(TAG, "Port " + name + " does not exist!");
                Program.Log(TAG, ".FAILED");
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
                Program.Log(TAG, ".FAILED");
                return;
            }

            new Thread(delegate ()
            {
                Program.Log(TAG, "Port opened. Ready for incoming data!");
                Program.Log(TAG, ".OK");

                //TODO Edited from true
                while (mySerial.IsOpen)
                {
                    try
                    {
                        if (mySerial.BytesToRead > 0)
                        {
                            string input = mySerial.ReadLine();
                            if (Program.GetParameter("serial-verbose", "true") == "true")
                                Program.Log(TAG + "/In", input);
                            OnMessageReceived(input);
                        }

                        if (!string.IsNullOrEmpty(toSend))
                        {
                            if (Program.GetParameter("serial-verbose", "true") == "true")
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
            }).Start();
        }

        public void Close()
        {
            mySerial.Close();
        }

        public void Write(string message)
        {
            toSend += message;
        }
    }
}
