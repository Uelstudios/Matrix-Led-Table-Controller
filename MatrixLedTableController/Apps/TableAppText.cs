﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Drawing;

namespace MatrixLedTableController.Apps
{
    class TableAppText : TableApp
    {
        string text;
        int scrollPosition = 0;
        double colorFade = 0.0;

        int scrollSpeed = 2;
        int nextScroll;

        string[] animationTypes = { "Regenbogen", "Zufall", "Rot", "Blau", "Gelb", "Grün" };
        string animationType;

        public TableAppText()
        {
            text = "MOIN";
            updateSpeed = 25;

            animationType = animationTypes[0];
        }

        public override void Draw()
        {
            ClearPixels();

            for (int i = 0; i < text.Length; i++)
            {
                char[] curChar = Convert.ToString((long)CharacterLookup.GetCharUlong(text[i]), 2).ToCharArray();
                Array.Reverse(curChar);
                for (int y = 0; y < CharacterLookup.characterHeight; y++)
                {
                    for (int x = 0; x < CharacterLookup.characterWidth; x++)
                    {
                        int index = x + (y * CharacterLookup.characterWidth);
                        if (curChar.Length > index && curChar[index] == '1')
                        {
                            SetPixel(x + scrollPosition + i * 7, y + 1, GetColor(index, x, y));
                        }
                    }
                }

            }

            colorFade += 0.01;

            if (nextScroll > scrollSpeed)
            {
                nextScroll = 0;

                scrollPosition -= 1;
                if (scrollPosition < text.Length * -7)
                    scrollPosition = 9;
            }
            else
            {
                nextScroll++;
            }
        }

        PixelColor GetColor(int index, int x, int y)
        {
            switch (animationType)
            {
                case "Regenbogen":
                    return PixelColor.FromHSL((Math.Sin((index * 0.01f) + colorFade) + 1) * .5f, 0.5, 0.5);
                case "Zufall":
                    return PixelColor.FromHSL(Program.random.NextDouble(), 1.0, 0.5);
                case "Gelb":
                    return PixelColor.YELLOW;
                case "Rot":
                    return PixelColor.RED;
                case "Grün":
                    return PixelColor.GREEN;
                case "Blau":
                    return PixelColor.BLUE;
                default: return PixelColor.BLACK;
            }
        }

        public override void OnCustomInterfaceInput(string id, string value)
        {
            if (id == "text")
            {
                text = value;
            }
            else if (id == "speed")
            {
                scrollSpeed = int.Parse(value);
            }
            else if (id == "animation")
            {
                animationType = value;
            }
        }

        public override CustomInterface GetCustomInterface()
        {
            return new CustomInterface()
                .AddLabel("Gib einen schönen Text ein")
                .AddEditText("text", text, "Deine Nachricht...")
                .AddSpacer(true)
                .AddLabel("Style")
                .AddSlider("speed", scrollSpeed, 0, 10)
                .AddList("animation", "Animation", animationTypes);
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(false, false);
        }
    }
}
