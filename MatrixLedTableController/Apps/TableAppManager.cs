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

        public static TableApp[] apps = {
            new TableAppIdle(),
            new TableAppRainbow(),
            new TableAppCha0s(),
            new TableAppSnake(),
            new TableAppBasicColorDot(),
            new TableAppText(),
            new TableAppTetris(),
            new TableAppSpaceInvaders(),
            new TableAppDevTest()
        };

        private Thread appThread;
        private TableApp currentApp;

        public void LaunchApp(TableApp app)
        {
            Program.Log(TAG, "Starting app: " + app.GetName());

            currentApp = app;
            
            appThread = new Thread(delegate ()
            {
                TableApp a = app;

                a.Init();
                while (a == currentApp)
                {
                    a.Draw();
                    a.RenderToTable();
                    Thread.Sleep(a.updateSpeed);
                }
            });
            appThread.Start();
        }

        public TableApp GetCurrentApp()
        {
            return currentApp;
        }

        public static TableApp GetAppById(string name)
        {
            foreach(TableApp app in apps)
                if (app.GetName() == name.Trim())
                    return (TableApp) Activator.CreateInstance(app.GetType());
            return new TableAppIdle();
        }
    }
}
