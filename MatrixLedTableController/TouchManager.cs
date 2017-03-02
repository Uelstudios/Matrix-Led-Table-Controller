using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixLedTableController.Apps;

namespace MatrixLedTableController
{
    public class TouchManager
    {
        private bool[,] inputMap;

        public TouchManager(int tableWidth, int tableHeight)
        {
            inputMap = new bool[tableWidth, tableHeight];
        }

        public bool GetInputAt(Position position)
        {
            return inputMap[position.x, position.y];
        }

        public void Reset()
        {
            inputMap = new bool[inputMap.GetLength(0), inputMap.GetLength(1)];
        }

        public void OnRawInput(string msg)
        {
            msg = msg.Trim();
            if (msg.Length < 3)
            {
                Program.Log("TouchManager", "Illegal input >" + msg + "<");
                return;
            }

            int x, y;
            if(int.TryParse(msg[0].ToString(), out x) && int.TryParse(msg[1].ToString(), out y))
            {
                UpdateTouchAt(new Position(x, y), msg[2] == '1');
            }
        }

        private void UpdateTouchAt(Position position, bool state)
        {
            if(position.x >= 0 && position.y >= 0 && 
                position.x < inputMap.GetLength(0) &&
                position.y < inputMap.GetLength(1))
            {
                inputMap[position.x, position.y] = state;
                Program.tableAppManager.GetCurrentApp().OnTouchUpdated(this);
            }
        }
    }
}
