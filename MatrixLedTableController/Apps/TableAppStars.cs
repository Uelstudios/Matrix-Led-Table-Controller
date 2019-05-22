using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppStars : TableApp
    {
        List<Star> stars;

        public TableAppStars()
        {
            stars = new List<Star>();
            for (int i = 0; i < 15; i++)
            {
                stars.Add(new Star());
            }

            updateSpeed /= 4;
        }

        public override void Draw()
        {
            ClearPixels();

            for (int i = 0; i < stars.Count; i++)
            {
                //Get Star
                Star star = stars[i];

                //Draw
                SetPixel(star.position.x, star.position.y, star.GetColor());

                //Update
                star.Update();
            }
        }

        public override void OnInputMade(InputKey key)
        {
            if(key == InputKey.XPadUp)
            {
                stars.Add(new Star());
            }
            if(key == InputKey.XPadDown)
            {
                if(stars.Count > 0)
                {
                    stars.RemoveAt(0);
                }
            }
        }

        class Star
        {
            public Position position;

            float colorHue;
            bool increasing;
            float brightness;
            float speed;

            public Star()
            {
                position = new Position();
                Reset(Program.random);
            }

            public void Reset(Random rand)
            {
                int x = rand.Next(Program.TableWidth);
                int y = rand.Next(Program.TableHeight);
                position = new Position(x, y);

                colorHue = rand.Next(0, 100) / 100f;
                speed = rand.Next(1, 8) / 100f;

                brightness = 0f;
                increasing = true;
            }

            public void Update()
            {
                if (increasing)
                {
                    brightness = Mathf.Clamp01(brightness + speed);
                    if (brightness >= 1f) increasing = false;
                }
                else
                {
                    brightness = Mathf.Clamp01(brightness - speed);
                    if (brightness <= 0f) Reset(Program.random);
                }
            }

            public PixelColor GetColor()
            {
                return PixelColor.FromHSL(colorHue, brightness, 0.5f * brightness);
            }
        }


        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(true, false);
        }
    }
}
