using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppLamp : TableApp
    {
        float hue = 0f;
        bool auto = true;
        PixelColor lastColor;

        public TableAppLamp()
        {
            userInterface = ClientUserInterface.Custom;
            updateSpeed = 10;
        }

        public override void Draw()
        {
            ClearPixels();

            if (auto)
            {
                hue += 0.0005f;
                if (hue > 1f)
                    hue -= 1f;
            }

            float delta = 0.01f;
            PixelColor color = PixelColor.LerpRGB(lastColor, PixelColor.FromHSL(hue, 1f, 0.5f), delta);
            SetPixels(PixelColor.GetSingleColorMap(Program.TableWidth, Program.TableHeight, color));
            lastColor = color;
        }

        public override void OnCustomInterfaceInput(string id, string data)
        {
            if(id == "sliderHue")
            {
                hue = int.Parse(data) / 100f;
                auto = false;
            }

            if(id == "btnAuto")
            {
                auto = true;
            }
        }

        public override CustomInterface GetCustomInterface()
        {
            return new CustomInterface()
                .AddLabel("Farbe")
                .AddSlider("sliderHue", 0, 0, 100)
                .AddSpacer(true)
                .AddLabel("Automatisch")
                .AddButton("btnAuto", "Aktivieren");
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
