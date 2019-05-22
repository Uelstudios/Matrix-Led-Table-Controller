using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppDraw : TableApp
    {
        PixelColor[,] drawMap;

        public TableAppDraw()
        {
            updateSpeed = 10;
            drawMap = PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, PixelColor.BLACK);
        }

        public override void Draw()
        {
            SetPixels(drawMap);
        }

        public override CustomInterface GetCustomInterface()
        {
            return new CustomInterface()
                .AddDrawingboard("drawingboard");
        }

        public override void OnCustomInterfaceInput(string id, string data)
        {
            if(id == "drawingboard")
            {
                Position position = CustomInterface.CustomInterfaceItemDrawingboard.ParsePosition(data);
                PixelColor color = CustomInterface.CustomInterfaceItemDrawingboard.ParseColor(data);

                drawMap[position.x, position.y] = color;
            }
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
