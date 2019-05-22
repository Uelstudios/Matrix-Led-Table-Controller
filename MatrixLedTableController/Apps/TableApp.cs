using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    abstract class TableApp
    {
        private PixelColor[,] colorMap;
        public enum ClientUserInterface { None = 0, XPad = 1, RunningText = 2, List = 3, Paint = 4, Custom = 5 };
        public ClientUserInterface userInterface = ClientUserInterface.Custom;
        public bool selectable = true;

        public int updateSpeed = 200;

        public void Init()
        {
            ClearPixels();
        }

        public string GetName()
        {
            string internalAppName = GetType().ToString();
            return internalAppName.Substring(internalAppName.LastIndexOf('.') + 1).Trim();
        }

        public abstract void Draw();

        public abstract FeatureSet GetFeatures();

        public void RenderToTable()
        {
            Program.tableRenderer.Render(colorMap);
        }

        protected void ClearPixels()
        {
            colorMap = PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK);
        }

        protected void SetPixelAtIndex(int index, PixelColor c)
        {
            int y = index / Program.TableWidth;
            int x = index - y * Program.TableWidth;
            SetPixel(x, y, c);
        }

        protected void SetPixel(Position pos, PixelColor c)
        {
            SetPixel(pos.x, pos.y, c);
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
            //Program.communicationServer.Send(string.Format("/Gameover {0}|{1}|{2}", message, score, gameName));
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
        public virtual void OnInputMade(InputKey key) { }

        public virtual void OnRawInput(string msg) { }

        public virtual void OnControllerInput(int controller, GamepadManager.GamepadKey key) { }

        public virtual void OnCustomInterfaceInput(string id, string data)
        {
            if (id == "updateSpeed")
            {
                updateSpeed = int.Parse(data);
            }
        }

        public virtual CustomInterface GetCustomInterface()
        {
            return new CustomInterface().AddSlider("updateSpeed", updateSpeed, 1, 300);
        }

        public static InputKey GetCorrespondingKey(string kString)
        {
            switch (kString)
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

    struct FeatureSet
    {
        public bool gamepad, touch;

        public FeatureSet(bool gamepad, bool touch)
        {
            this.gamepad = gamepad;
            this.touch = touch;
        }
    }
}
