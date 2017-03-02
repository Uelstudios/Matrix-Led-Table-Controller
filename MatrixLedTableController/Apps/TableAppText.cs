using System;
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

        int animationType = 0;

        public TableAppText()
        {
            text = "MOIN";
            updateSpeed = 25;

            userInterface = ClientUserInterface.RunningText;
        }

        public override void OnRawInput(string msg)
        {
            if(msg.StartsWith("speed:"))
            {
                int.TryParse(msg.Substring(msg.IndexOf(':') + 1), out scrollSpeed);
            }
            else if(msg.StartsWith("animationType:"))
            {
                int.TryParse(msg.Substring(msg.IndexOf(':') + 1), out animationType);
            }
            else if(msg.StartsWith("text:"))
            {
                text = msg.Substring(msg.IndexOf(':') + 1);
            }
        }

        public override void Draw()
        {
            ClearPixels();

            for(int i = 0; i < text.Length; i++)
            {
                char[] curChar = Convert.ToString((long)CharacterLookup.GetCharUlong(text[i]), 2).ToCharArray();
                Array.Reverse(curChar);
                for (int y = 0; y < CharacterLookup.characterHeight; y++)
                {
                    for (int x = 0; x < CharacterLookup.characterWidth; x++)
                    {
                        int index = x + (y * CharacterLookup.characterWidth);
                        if (curChar.Length > index && curChar[index] == '1') {
                            SetPixel(x + scrollPosition + i * 7, y + 1, PixelColor.FromHSL((Math.Sin((index * 0.01f) + colorFade) + 1) * .5f, 0.5, 0.5));
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
                nextScroll ++;
            }
        }
    }
}
