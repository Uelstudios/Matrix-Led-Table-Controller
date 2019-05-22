using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppPong : TableApp
    {
        float hueSpeed = 0.0001f;
        float hueBall = 0f;
        float curPadHue = 0f;

        Position ballPos = new Position(3, 2);
        Position ballVelocity = new Position(1, 1);
        int ballSpeed = 100;
        int ballNextMove = 0;

        int padPos;

        int score = 0;

        float lostTime = 0f;

        public TableAppPong()
        {
            updateSpeed = 1;
        }

        public override void Draw()
        {
            ClearPixels();

            for (int x = 0; x < 3; x++)
            {
                SetPixel(padPos + x, 0, PixelColor.FromHSL(curPadHue, 1f, 0.5f));
            }

            MoveBall();
            SetPixel(ballPos, PixelColor.FromHSL(hueBall, 1f, 0.5f));

            if (hueSpeed > 0.0001f)
            {
                hueSpeed -= 0.00001f;
            }
            else if (hueSpeed < 0.0001f)
            {
                hueSpeed = 0.0001f;
            }

            if (lostTime > 0f)
            {
                SetPixels(PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.Random(new Random())));
                lostTime += 0.001f;

                if (lostTime > 1f) {
                    GameOver("Du hast wohl nicht genügend Zielwasser getrunken...", score, "pong");
                    Program.tableAppManager.LaunchApp(TableAppManager.GetAppById(new TableAppIdle().GetName()));
                }
            }


            hueBall += hueSpeed;
            if (hueBall > 1f) hueBall -= 1f;
        }

        void HitPad()
        {
            curPadHue = hueBall;
            hueSpeed += 0.01f;

            score++;
        }

        void MoveBall()
        {
            ballNextMove++;

            if (ballNextMove >= ballSpeed)
            {
                ballPos += ballVelocity;
                ballNextMove = 0;


                if (ballPos.y >= Program.TableHeight - 1)
                {
                    ballVelocity.y = -ballVelocity.y;
                }

                if (ballPos.x <= 0)
                {
                    ballVelocity.x = -ballVelocity.x;
                }

                if (ballPos.x >= Program.TableWidth - 1)
                {
                    ballVelocity.x = -ballVelocity.x;
                }

                if (ballPos.y <= 1)
                {
                    if (ballPos.x == padPos)
                    {
                        if (ballPos.x != 0)
                            ballVelocity.x = -Math.Abs(ballVelocity.x);

                        ballVelocity.y = Math.Abs(ballVelocity.y);
                        HitPad();
                    }
                    else if (ballPos.x == padPos + 1)
                    {
                        ballVelocity.y = Math.Abs(ballVelocity.y);
                        HitPad();
                    }
                    else if (ballPos.x == padPos + 2)
                    {
                        if (ballPos.x != Program.TableWidth - 1)
                            ballVelocity.x = Math.Abs(ballVelocity.x);
                        ballVelocity.y = Math.Abs(ballVelocity.y);
                        HitPad();

                    }
                    else if (ballPos.y < 0)
                    {
                        ballSpeed -= 5;
                        ballVelocity = new Position(0, 0);
                        ballPos = new Position(0, 10);
                        lostTime = 0.1f;
                    }
                }

            }
        }

        public override void OnControllerInput(int controller, GamepadManager.GamepadKey key)
        {
            if (controller == 0)
            {
                if (key == GamepadManager.GamepadKey.Left)
                {
                    if (padPos > 0) padPos--;
                }
                else if (key == GamepadManager.GamepadKey.Right)
                {
                    if (padPos < Program.TableWidth - 3) padPos++;
                }
            }
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(true, false);
        }
    }
}
