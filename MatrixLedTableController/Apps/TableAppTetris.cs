using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppTetris : TableApp
    {
        private Tetramino currentTetramino;
        private GameGrid gameGrid;

        public TableAppTetris()
        {
            updateSpeed = 10;
            userInterface = ClientUserInterface.XPad;

            gameGrid = new GameGrid(Program.TableWidth, Program.TableHeight);
        }

        public override void Draw()
        {
            /**
                LOGIC
            **/
            if (currentTetramino == null || currentTetramino.reachedFinalDestination)
                currentTetramino = new Tetramino();

            /**
                UPDATE
            **/
            gameGrid.Update();
            currentTetramino.Update(gameGrid);

            /**
                DRAW
            **/
            ClearPixels();
            foreach (Position pos in currentTetramino.shape)
            {
                Position worldPos = pos + currentTetramino.position;
                SetPixel(worldPos.x, worldPos.y, currentTetramino.color);
            }

            for(int x = 0; x < gameGrid.width; x++)
            {
                for(int y = 0; y < gameGrid.height; y++)
                {
                    Position pos = new Position(x, y);
                    if(!gameGrid.IsFree(pos))
                    {
                        SetPixel(pos.x, pos.y, gameGrid.GetBlock(pos));
                    }
                }
            }

        }

        public override void OnInputMade(InputKey key)
        {
            if(!currentTetramino.reachedFinalDestination)
                switch(key)
                {
                    case InputKey.XPadLeft:
                        currentTetramino.Move(-1, 0, gameGrid);
                        break;
                    case InputKey.XPadRight:
                        currentTetramino.Move(1, 0, gameGrid);
                        break;
                    case InputKey.XPadAction:
                        currentTetramino.Rotate(gameGrid);
                        break;
                }
        }

        public override FeatureSet GetFeatures()
        {
            return new FeatureSet(true, false);
        }

        class Tetramino
        {
            public Position position;
            public PixelColor color;
            public bool reachedFinalDestination;
            public Position[] shape;
            private int mass = 50;
            private int currentSkip = 0;

            public Tetramino()
            {
                position = new Position(5, 0);
                reachedFinalDestination = false;

                char name = GetRandomShape(new Random());
                shape = GetShape(name);
                color = GetColor(name);
            }

            public void Update(GameGrid grid)
            {
                if (reachedFinalDestination)
                    return;

                if (currentSkip <= 1000)
                {
                    currentSkip += mass;
                    return;
                }
                currentSkip = 0;

                reachedFinalDestination = !Move(0, 1, grid);

                /**
                    Move tetramino to grid
                    if the groud had been hit.
                **/
                if (reachedFinalDestination)
                {
                    foreach (Position p in shape)
                    {
                        Position worldPos = p + position;
                        grid.SetBlock(worldPos, color);
                    }
                }
            }

            public void Rotate(GameGrid grid)
            {
                for(int i = 0; i < shape.Length; i++)
                {
                    shape[i].RotateAround(new Position(0, 0), 90);
                }
            }

            public bool Move(int xspeed, int yspeed, GameGrid grid)
            {
                Position newPosition = position + new Position(xspeed, yspeed);
                if (!canMoveTo(grid, newPosition))
                    return false;

                position = newPosition;
                return true;
            }

            private bool canMoveTo(GameGrid grid, Position newPosition)
            {
                foreach (Position p in shape)
                {
                    Position worldPos = p + newPosition;
                    if (!grid.IsFree(worldPos))
                        return false;
                }
                return true;
            }

            private static char GetRandomShape(Random rand)
            {
                char[] names = "szljtio".ToCharArray();
                return names[rand.Next(0, names.Length)];
            }

            private static PixelColor GetColor(char name)
            {
                switch(name)
                {
                    case 's':
                        return PixelColor.GREEN;
                    case 'z':
                        return PixelColor.RED;
                    case 'l':
                        return PixelColor.ORANGE;
                    case 'j':
                        return PixelColor.BLUE;
                    case 't':
                        return PixelColor.PURPLE;
                    case 'i':
                        return PixelColor.CYAN;
                    case 'o':
                        return PixelColor.YELLOW;
                    default:
                        return PixelColor.BLACK;
                }
            }

            private static Position[] GetShape(char name)
            {
                switch (name)
                {
                    case 's':
                        return new Position[] { new Position(0, 1), new Position(1, 1), new Position(1, 0), new Position(2, 0) };
                    case 'z':
                        return new Position[] { new Position(0, 0), new Position(1, 1), new Position(1, 0), new Position(2, 1) };
                    case 'l':
                        return new Position[] { new Position(0, 0), new Position(0, 1), new Position(0, 2), new Position(1, 2) };
                    case 'j':
                        return new Position[] { new Position(1, 0), new Position(1, 1), new Position(1, 2), new Position(0, 2) };
                    case 't':
                        return new Position[] { new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(1, 1) };
                    case 'i':
                        return new Position[] { new Position(0, 0), new Position(0, 1), new Position(0, 2) };
                    case 'o':
                        return new Position[] { new Position(0, 0), new Position(0, 1), new Position(1, 0), new Position(1, 1) };
                    default:
                        return new Position[] { };
                }
            }
           
        }

        class GameGrid
        {
            public int width, height;
            private PixelColor[,] grid;

            public GameGrid(int width, int height)
            {
                this.width = width;
                this.height = height;

                grid = PixelColor.GetSingleColorMap(width, height, PixelColor.BLACK);
            }

            public void Update()
            { }

            public void SetBlock(Position pos, PixelColor color)
            {
                if (pos.y < 0 || pos.x < 0 || pos.x >= width || pos.y >= height)
                    return;
                grid[pos.x, pos.y] = color;
            }

            public void ClearBlock(Position pos)
            {
                SetBlock(pos, PixelColor.BLACK);
            }

            public PixelColor GetBlock(Position pos)
            {
                return grid[pos.x, pos.y];
            }

            public bool IsFree(Position pos)
            {
                if (pos.x < 0 || pos.x >= width || pos.y >= height)
                    return false;
                if (pos.y < 0)
                    return true;

                return GetBlock(pos).Same( PixelColor.BLACK );
            }

        }
    }
}
