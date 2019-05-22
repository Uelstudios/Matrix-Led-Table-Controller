using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppBlubbl : TableApp
    {
        List<Blubblo> blubblos = new List<Blubblo>();

        public TableAppBlubbl()
        {
            updateSpeed = 200;
        }

        public override void Draw()
        {
            ClearPixels();

            for (int i = 0; i < blubblos.Count; i++)
            {
                SetPixel(blubblos[i].pos, blubblos[i].color);
                blubblos[i].Update();
            }
        }

        private class Blubblo
        {
            public Position pos;
            public PixelColor color;

            public Blubblo()
            {
                pos = GetRandomPosition();
                RandomizedNewColor();
            }

            public void Update()
            {
                pos.y -= 1;
                if (pos.y < 0)
                {
                    pos = GetRandomPosition();
                }
            }

            private Position GetRandomPosition()
            {
                return new Position(Program.random.Next(Program.TableWidth), Program.TableHeight);
            }

            public void RandomizedNewColor()
            {
                color = PixelColor.FromHSL(Program.random.NextDouble(), 1.0, 0.5);
            }
        }

        public override CustomInterface GetCustomInterface()
        {
            return new CustomInterface()
                .AddLabel("Johannes tolle Idee für eine App. Es heißt sich Blubbl")
                .AddButton("addBlubblo", "Mehr Blubbl")
                .AddButton("remBlubblo", "Mach weg")
                .AddButton("randColors", "Mischmasch Farben")
                .AddSpacer(true)
                .AddLabel("Geschwindigkeit")
                .AddSlider("updateSpeed", updateSpeed, 0, 400);
        }

        public override void OnInputMade(InputKey key)
        {
            if(key == InputKey.XPadAction)
            {
                blubblos.Add(new Blubblo());
            }
        }

        public override void OnCustomInterfaceInput(string id, string data)
        {
            if (id == "addBlubblo")
            {
                blubblos.Add(new Blubblo());
            }
            else if (id == "remBlubblo")
            {
                blubblos = new List<Blubblo>();
            }
            else if (id == "randColors")
            {
                foreach (Blubblo b in blubblos)
                {
                    b.RandomizedNewColor();
                }
            }
            else if (id == "updateSpeed")
            {
                updateSpeed = int.Parse(data);
            }
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(true, false);
        }
    }
}
