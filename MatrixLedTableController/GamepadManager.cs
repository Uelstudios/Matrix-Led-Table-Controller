using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenTK.Input;

namespace MatrixLedTableController
{
    class GamepadManager
    {
        static readonly string TAG = "GamepadManager";

        GamePadStatus[] gamePadStatus;

        Thread updateThread;

        public GamepadManager(/*GamePadStatusChanged statusChange*/)
        {
            //this.statusChange = statusChange;
           
            gamePadStatus = new GamePadStatus[4];
            for(int i = 0; i < gamePadStatus.Length; i++)
                gamePadStatus[i] = new GamePadStatus();


            updateThread = new Thread(delegate ()
            {
                while (true)
                {
                    PollInputs();
                    //Thread.Sleep(-1);
                }
            });
            updateThread.Start();

            Program.Log(TAG, ".OK");
        }

        public void PollInputs()
        {
            for (int i = 0; i < 4; i++)
            {
                GamePadState state = GamePad.GetState(i);
                GamePadStatus statusNew = new GamePadStatus().Set(state);
                GamePadStatus statusOld = gamePadStatus[i];
                
                if (statusNew.isConnected)
                {
                    /**
                     * BUTTONS
                     **/
                    if (statusNew.buttonStart != statusOld.buttonStart)                         //BUTTON -START-
                    {
                        if (statusNew.buttonStart)
                        {
                            Program.TriggerInput("pad_start");
                            Program.GamepadInput(i, GamepadKey.Start);
                        }
                    }

                    if (statusNew.buttonSelect != statusOld.buttonSelect)                       //BUTTON -SELECT-
                    {
                        if (statusNew.buttonSelect)
                        {
                            Program.TriggerInput("pad_select");
                            Program.GamepadInput(i, GamepadKey.Select);
                        }
                    }

                    if (statusNew.buttonA != statusOld.buttonA)                                 //BUTTON -A-
                    {
                        if (statusNew.buttonA)
                        {
                            Program.TriggerInput("pad_action");
                            Program.GamepadInput(i, GamepadKey.A);
                        }
                    }

                    if (statusNew.buttonB != statusOld.buttonB)                                 //BUTTON -B-
                    {
                        if (statusNew.buttonB)
                        {
                            Program.TriggerInput("pad_action2");
                            Program.GamepadInput(i, GamepadKey.B);
                        }
                    }

                    /**
                     * DPAD
                     **/
                    if (statusNew.dpadUp != statusOld.dpadUp)                                 //BUTTON -DPad Up-
                    {
                        if (statusNew.dpadUp)
                        {
                            Program.TriggerInput("pad_up");
                            Program.GamepadInput(i, GamepadKey.Up);
                        }
                    }

                    if (statusNew.dpadRight != statusOld.dpadRight)                                 //BUTTON -DPad Right-
                    {
                        if (statusNew.dpadRight)
                        {
                            Program.TriggerInput("pad_right");
                            Program.GamepadInput(i, GamepadKey.Right);
                        }
                    }

                    if (statusNew.dpadDown != statusOld.dpadDown)                                 //BUTTON -DPad Down-
                    {
                        if (statusNew.dpadDown)
                        {
                            Program.TriggerInput("pad_down");
                            Program.GamepadInput(i, GamepadKey.Down);
                        }
                    }

                    if (statusNew.dpadLeft != statusOld.dpadLeft)                                 //BUTTON -DPad Left-
                    {
                        if (statusNew.dpadLeft)
                        {
                            Program.TriggerInput("pad_left");
                            Program.GamepadInput(i, GamepadKey.Left);
                        }
                    }
                }

                if(statusNew.isConnected != statusOld.isConnected)
                {
                    if (statusNew.isConnected)
                    {
                        //statusChange.OnDeviceConnected(i, GamePad.GetName(i));
                        Program.Log(TAG, string.Format("GamePad({0}) connected", i));
                    }
                    else
                    {
                        //statusChange.OnDeviceDisconnected(i);
                        Program.Log(TAG, string.Format("GamePad({0}) disconnected", i));
                    }
                }

                //Update
                gamePadStatus[i] = statusNew;
            }
        }

        public void CleanUp()
        {
            updateThread.Abort();
        }

        public interface GamePadStatusChanged
        {
            void OnDeviceConnected(int index, string name);
            void OnDeviceDisconnected(int index);
        }

        public class GamePadStatus
        {
            public bool isConnected;

            //Menu Buttons
            public bool buttonStart;
            public bool buttonSelect;

            //Action Buttons
            public bool buttonA;
            public bool buttonB;
            
            //Dpad
            public bool dpadUp;
            public bool dpadRight;
            public bool dpadDown;
            public bool dpadLeft;

            public GamePadStatus()
            {

            }

            public GamePadStatus Set(GamePadState state)
            {
                //MISC
                isConnected = state.IsConnected;

                //Menu Buttons
                buttonStart = state.Buttons.RightStick == ButtonState.Pressed;
                buttonSelect = state.Buttons.LeftStick == ButtonState.Pressed;

                //Action Buttons
                buttonA = state.Buttons.B == ButtonState.Pressed;   //B IS MAPPED TO A
                buttonB = state.Buttons.A == ButtonState.Pressed;   //A IS MAPPED TO B

                //Dpad
                GamePadThumbSticks sticks = state.ThumbSticks;
                
                dpadRight = sticks.Left.X > 0.5f;
                dpadLeft = sticks.Left.X < -0.5f;

                dpadUp = sticks.Left.Y > 0.5f;
                dpadDown = sticks.Left.Y < -0.5f;

                return this;
            }
        }

        public enum GamepadKey
        {
            Start,
            Select,
            A,
            B,
            Up,
            Right,
            Down,
            Left
        }
    }
}
