using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MatrixLedTableController.Apps
{
    class TableAppManager
    {
        static readonly string TAG = "TableAppManager";

        #region List of Apps
        public static TableApp[] apps = {
            new TableAppIdle(),
            new TableAppClock(),
            new TableAppSinCos(),
            new TableAppBlubbl(),
            new TableAppRainbow(),
            new TableAppCha0s(),
            new TableAppSnake(),
            new TableAppPong(),
            new TableAppBasicColorDot(),
            new TableAppText(),
            new TableAppTetris(),
            new TableAppSpaceInvaders(),
            new TableAppDevTest(),
            new TableAppChess(),
            new TableAppStars(),
            new TableAppAnimation(new Animations.Animation(@"D:\Projekte\Visual Studio\MatrixLedTableController\MatrixLedTableController\bin\Debug\animations\animation0.png")),
            new TableAppPolice(),
            new TableAppKnightRider(),
            new TableAppFire(),
            new TableAppRevolver(),
            new TableAppDraw(),
            new TableAppLamp(),
            new TableAppBlink(),
            new TableAppShine(),
            new TableAppImage(),
            new TableAppProximity()
        };
        #endregion

        private Thread appThread;
        private TableApp currentApp;

        public TableAppManager()
        {
            Program.Log(TAG, ".OK");
        }

        public void LaunchApp(TableApp app)
        {
            if (app != null)
                Program.Log(TAG, "Starting app: " + app.GetName());

            currentApp = app;

            appThread = new Thread(delegate ()
            {
                TableApp a = app;
                a.Init();

                if (a.userInterface == TableApp.ClientUserInterface.Custom)
                {
                    CustomInterface customInterface = a.GetCustomInterface();
                    string itemString = string.Empty;
                    for (int i = 0; i < customInterface.items.Count; i++)
                    {
                        itemString += customInterface.items[i];
                        if (i != customInterface.items.Count - 1)
                            itemString += ",";
                    }

                    ///Program.communicationServer.Send("create_custom_interface " + itemString);
                }

                int customSpeed = Program.GetParameterInt("appspeed", -1);
                while (a == currentApp)
                {
                    a.Draw();
                    a.RenderToTable();

                    Thread.Sleep(customSpeed >= 0 ? customSpeed : a.updateSpeed);
                }
            });
            if (currentApp != null)
                appThread.Start();
        }

        public void CleanUp()
        {
            currentApp = null;

            //Clear the screen
            PixelColor[,] clear = PixelColor
                .GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK);
            Program.tableRenderer.Render(clear);
        }

        public TableApp GetCurrentApp()
        {
            return currentApp;
        }

        public static TableApp GetAppById(string name)
        {
            foreach (TableApp app in apps)
                if (app.GetName() == name.Trim())
                    return (TableApp)Activator.CreateInstance(app.GetType());
            return new TableAppIdle();
        }

        public TableApp[] GetApps()
        {
            return apps;
        }
    }
}
