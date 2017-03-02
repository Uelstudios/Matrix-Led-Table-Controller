using System;
using System.Resources;
using System.Drawing;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using MatrixLedTableController.Animations;

namespace MatrixLedTableController.Apps
{
    class TableAppAnimation : TableApp
    {
        PixelColor[][,] animationPhases;
        int fps;
        
        int _nextAnimation = 0;
        int _animationStep = 0;

        public TableAppAnimation(Animation animation)
        {
            fps = animation.fps;

            int frameCount = 5;         //animation.frames.Length

            List<PixelColor[,]> frames = new List<PixelColor[,]>();
            for (int frame = 0; frame < frameCount; frame ++)
            {
                PixelColor[,] currentFrame = new PixelColor[Program.TableWidth, Program.TableHeight];
                for (int p = 0; p < 100; p ++)          ///animation.frames.GetLength(1)
                {
                    int x = p % Program.TableWidth;
                    int y = (p - x) / Program.TableWidth;

                    //currentFrame[x, y] = Animation.UIntToColor(animation.frames[frame, p]);
                }
                frames.Add(currentFrame);
            }
            animationPhases = frames.ToArray();
        }

        public override void Draw()
        {
            _nextAnimation += fps / 20;

            if(_nextAnimation > 100)
            {
                _nextAnimation -= 100;
                _animationStep++;

                if (_animationStep >= animationPhases.Length)
                    _animationStep = 0;
            }

            SetPixels(animationPhases[_animationStep]);
        }

    }
}
