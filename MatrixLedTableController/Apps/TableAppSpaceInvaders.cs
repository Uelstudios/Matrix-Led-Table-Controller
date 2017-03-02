using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppSpaceInvaders : TableApp
    {
        Ship ship;
        List<Projectile> projectiles;
        Invader[] invaders;
        bool gameOver = false;
        int score = 0;

        int flameWallRange = Program.TableHeight;

        public TableAppSpaceInvaders()
        {
            userInterface = ClientUserInterface.XPad;

            updateSpeed = 50;

            ship = new Ship(PixelColor.ORANGE);
            projectiles = new List<Projectile>();

            invaders = new Invader[5];
            for(int i = 0; i < 5; i++)
            {
                invaders[i] = new Invader(new Position(i * 2, 0), PixelColor.GREEN);
            }
        }

        public override void Draw()
        {
            if (!gameOver)
            {
                ClearPixels();

                SetPixel(ship.position.x, ship.position.y, ship.color);
                bool hitEdge = false;
                foreach (Invader i in invaders)
                {
                    if (i.Update())
                    {
                        if (i.position.x >= Program.TableWidth - 1 || i.position.x <= 0)
                            hitEdge = true;
                        if (i.position.y >= Program.TableHeight - 1)
                        {
                            gameOver = true;
                            GameOver("Die Aliens haben dich zerstört!", score, "space_invaders");
                            ClearPixels();
                            return;
                        }
                    }
                    SetPixel(i.position.x, i.position.y, i.color);
                }

                if (hitEdge)
                {
                    foreach (Invader i in invaders)
                    {
                        i.ShiftDown();
                    }
                }

                foreach (Projectile p in projectiles)
                {
                    if(p.position.y < 0)
                    {
                        projectiles.Remove(p);
                        return;
                    }

                    p.Update();
                    int hitIndex = p.Hit(invaders);
                    if(hitIndex >= 0)
                    {
                        invaders = invaders.Where((val, idx) => idx != hitIndex).ToArray();
                    }
                    SetPixel(p.position.x, p.position.y, p.color);
                }

            }
            else
            {
                for(int i = 0; i < Program.TableWidth; i++)
                {
                    SetPixel(i, flameWallRange, PixelColor.FromHSL(Math.Abs(flameWallRange / 50f - 0.10), 0.5, 0.5));
                }
                flameWallRange--;
                if(flameWallRange < -4)
                {
                    Close();
                }
            }
        }

        public override void OnInputMade(InputKey key)
        {
            switch(key)
            {
                case InputKey.XPadAction:
                    projectiles.Add(new Projectile(ship.position, PixelColor.PURPLE));
                    break;
                case InputKey.XPadLeft:
                    ship.Move(-1);
                    break;
                case InputKey.XPadRight:
                    ship.Move(1);
                    break;
            }
        }

        class Ship
        {
            public Position position;
            public PixelColor color;

            public Ship(PixelColor color)
            {
                position = new Position(4, Program.TableHeight - 1);
                this.color = color;
            }

            public void Move(int dir)
            {
                position += new Position(dir, 0);
                if (position.x < 0)
                    position = new Position(0, Program.TableHeight - 1);
                else if (position.x >= Program.TableWidth)
                    position = new Position(Program.TableWidth - 1, Program.TableHeight - 1);
            }
        }
        
        class Projectile
        {
            public Position position;
            public PixelColor color;

            public Projectile(Position startPosition, PixelColor color)
            {
                position = startPosition;
                this.color = color;
            }

            public int Hit(Invader[] invaders)
            {
                for(int i = 0; i < invaders.Length; i++)
                {
                    if (invaders[i].position.IsTheSame(position))
                        return i;
                }

                return -1;
            }

            public void Update()
            {
                position -= new Position(0, 1);
            }
        }

        class Invader
        {
            public Position position;
            public PixelColor color;
            int xSpeed = 1;
            int moveSpeed = 2;
            int skipStep = 0;

            public Invader(Position startPosition, PixelColor color)
            {
                position = startPosition;
                this.color = color;
            }

            public void ShiftDown()
            {
                position += new Position(0, 1);
                xSpeed *= -1;
            }

            public bool Update()
            {
                if(skipStep >= 40)
                {
                    skipStep = 0;
                    position += new Position(xSpeed, 0);
                    return true;
                }
                else
                {
                    skipStep += moveSpeed;
                    return false;
                }
            }
        }
    }
}
