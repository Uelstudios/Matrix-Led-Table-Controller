using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableApp
    {
        private PixelColor[,] colorMap;
        public enum ClientUserInterface { None = 0, XPad = 1, RunningText = 2, List = 3, Paint = 4 };
        public ClientUserInterface userInterface = ClientUserInterface.None;
        public bool selectable = true;

        public int updateSpeed = 200;

        public void Init()
        {
            colorMap = PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK);
        }

        public string GetName()
        {
            string internalAppName = GetType().ToString();
            return internalAppName.Substring(internalAppName.LastIndexOf('.') + 1).Trim();
        }

        public virtual void Draw()
        {
            throw new NotImplementedException();
        }

        public void RenderToTable()
        {
            Program.tableRenderer.Render(colorMap);
        }

        protected void ClearPixels()
        {
            colorMap = PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK);
        }

        protected void SetPixel(int x, int y, PixelColor c)
        {
            if (x < 0 || y < 0 || x >= colorMap.GetLength(0) || y >= colorMap.GetLength(1))
                return;
            colorMap[x, y] = c;
        }

        protected void SetPixels(PixelColor[,] map)
        {
            colorMap = map;
        }

        protected void GameOver(string message, int score, string gameName)
        {
            Program.communicationServer.Send(string.Format("/Gameover {0}|{1}|{2}", message, score, gameName));
        }

        protected void Close()
        {
            Program.tableAppManager.LaunchApp(new TableAppIdle());
        }

        protected bool GetPixelInput(int x, int y)
        {
            throw new NotImplementedException();
        }

        public virtual void OnTouchUpdated(TouchManager manager) { }

        public enum InputKey { Unknown, XPadLeft, XPadRight, XPadUp, XPadDown, XPadAction };
        public virtual void OnInputMade(InputKey key)
        { }

        public virtual void OnRawInput(string msg)
        { }

        public static InputKey GetCorrespondingKey(string kString)
        {
            switch(kString)
            {
                case "pad_action":
                    return InputKey.XPadAction;
                case "pad_left":
                    return InputKey.XPadLeft;
                case "pad_right":
                    return InputKey.XPadRight;
                case "pad_up":
                    return InputKey.XPadUp;
                case "pad_down":
                    return InputKey.XPadDown;
                default:
                    return InputKey.Unknown;
            }
        }
    }
}
