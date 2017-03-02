using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixLedTableController.Apps
{
    class TableAppSnake : TableApp
    {
        Snake snake = new Snake();
        Food food;
        Random rand;
        int deadTime = 10;
        bool gameOver = false;

        public TableAppSnake()
        {
            userInterface = ClientUserInterface.XPad;

            rand = new Random();
        }

        public override void Draw()
        {
            ClearPixels();

            if (food == null)
                food = Food.Spawn(rand, snake);

            //Update Snake
            if(!snake.isDead)
                snake.Update();

            //Eat Food
            if (snake.Eat(food))
                food = Food.Spawn(rand, snake);


            //Draw Snake with tail
            SetPixel(snake.position.x, snake.position.y, snake.color);
            foreach (Position pos in snake.tail)
                SetPixel(pos.x, pos.y, snake.tailColor);

            //Draw Food
            SetPixel(food.position.x, food.position.y, food.color);

            //Gameover?
            if (snake.isDead)
            {
                if(!gameOver)
                {
                    GameOver("Du hast dich selbst gefressen!", snake.tail.Count, "snake");
                    gameOver = true;
                }

                deadTime--;
                if (Mathf.IsOdd(deadTime))
                    SetPixel(snake.position.x, snake.position.y, new PixelColor(174, 53, 31));
                if (deadTime <= 0)
                    Close();
            }
        }

        public override void OnInputMade(InputKey key)
        {
            snake.OnInput(key);
        }

        public override void OnTouchUpdated(TouchManager manager)
        {
            Program.tableAppManager.LaunchApp(new TableAppCha0s());
        }

        public class Food
        {
            public Position position;
            public PixelColor color = new PixelColor(171, 62, 83);

            public Food(Position pos)
            {   
                position = pos;
            }

            public static Food Spawn(Random rand, Snake snake)
            {
                Position pos = snake.position;
                while (snake.IsClaiming(pos))
                    pos = new Position(rand.Next(0, Program.TableWidth), rand.Next(0, Program.TableHeight));
                return new Food(pos);
            }
        }

        public class Snake
        {
            public Position position = new Position(5, 5);
            public PixelColor color = new PixelColor(229, 229, 229);
            public PixelColor tailColor = new PixelColor(171, 166, 156);
            Position speed = new Position(0, -1);

            public List<Position> tail = new List<Position>();

            public bool isDead = false;

            public bool Eat(Food food)
            {
                if (position.IsTheSame(food.position))
                {
                    tail.Add(position);
                    return true;
                }
                return false;
            }

            public bool IsClaiming(Position pos)
            {
                if (position.IsTheSame(pos))
                    return true;
                foreach (Position p in tail)
                    if (p.IsTheSame(pos))
                        return true;
                return false;
            }

            public void Update()
            {
                Position gotoPos = position + speed;
                if (gotoPos.x < 0)
                    gotoPos.x = Program.TableWidth - 1;
                if (gotoPos.y < 0)
                    gotoPos.y = Program.TableHeight - 1;
                if (gotoPos.x >= Program.TableWidth)
                    gotoPos.x = 0;
                if (gotoPos.y >= Program.TableHeight)
                    gotoPos.y = 0;

                if(IsClaiming(gotoPos))
                    isDead = true;

                //Update Tail and Position
                tail.Add(position);
                tail.RemoveAt(0);
                position = gotoPos;
            }

            public void OnInput(InputKey key)
            {
                if (key == InputKey.XPadUp)
                {
                    speed = new Position(0, -1);
                }
                else if (key == InputKey.XPadLeft)
                {
                    speed = new Position(-1, 0);
                }
                else if (key == InputKey.XPadRight)
                {
                    speed = new Position(1, 0);
                }
                else if (key == InputKey.XPadDown)
                {
                    speed = new Position(0, 1);
                }
            }
        }
    }
}
