using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeWithBrain
{
    class Logic : IDrawable, IMoveable, IControl
    {
        public int Width => Map.Width;
        public int Height => Map.Height;

        public Map Map;
        public Snake Snake;

        public GameState State { get; private set; }

        public bool DebugMode { get; set; }
 
        public enum GameState
        {
            InProgress, Success, Failed
        }
        
        public Logic(int width, int height)
        {
            Map = new Map(width, height);
            Snake = new Snake(width / 2, height / 2);

            State = GameState.InProgress;
        }

        public void Reset()
        {
            Map = new Map(Width, Height);
            Snake = new Snake(Width / 2, Height / 2);

            State = GameState.InProgress;
        }

        public void Draw(GameDrawer drawer)
        {
            Map.Draw(drawer);
            Snake.Draw(drawer);
        }

        public void Iterate()
        {
            if(State != GameState.InProgress) return;

            IterateSnake();

            if (DebugMode)
            {
                if (Snake.Head.X < 0)
                {
                    Snake.RollBack();
                    Snake.MoveUp();
                    IterateSnake();
                }
                else if (Snake.Head.X >= Width)
                {
                    Snake.RollBack();
                    Snake.MoveDown();
                    IterateSnake();
                }
                if (Snake.Head.Y < 0)
                {
                    Snake.RollBack();
                    Snake.MoveRight();
                    IterateSnake();
                }
                else if (Snake.Head.Y >= Height)
                {
                    Snake.RollBack();
                    Snake.MoveLeft();
                    IterateSnake();
                }
            }
            else
            { 
                //end of map
                if (Snake.Head.X < 0 || Snake.Head.X >= Width || Snake.Head.Y < 0 || Snake.Head.Y >= Height)
                {
                    Snake.RollBack();
                    State = GameState.Failed;
                    return;
                }

                var snakeHead = Snake.Head;
                var snakePoint = Snake.GetPoints();

                //skip head
                for (int i = 1; i < snakePoint.Count; i++)
                {
                    if (snakeHead.AreEqualPos(snakePoint[i]))
                    {
                        Snake.RollBack();
                        State = GameState.Failed;
                        return;
                    }
                }
            }
        }

        private void IterateSnake()
        {
            Snake.Iterate();

            var fruitPos = Map.FruitPos;

            if (fruitPos == null || fruitPos != null && Snake.Head.AreEqualPos(fruitPos))
            {
                if (fruitPos != null)
                {
                    Snake.Increase();
                }

                if (!Map.GenerateNewFruit(Snake.GetPoints()))
                {
                    State = GameState.Success;
                }
            }
        }

        public void MoveDown()
        {
            Snake.MoveDown();
        }

        public void MoveLeft()
        {
            Snake.MoveLeft();
        }

        public void MoveRight()
        {
            Snake.MoveRight();
        }

        public void MoveUp()
        {
            Snake.MoveUp();
        }
       
    }
}
