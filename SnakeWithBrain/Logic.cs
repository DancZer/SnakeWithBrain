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
        public int Width => _map.Width;
        public int Height => _map.Height;

        private Map _map;
        private Snake _snake;

        public Logic(int width, int height)
        {
            _map = new Map(width, height);
            _snake = new Snake(width / 2, height / 2);
        }

        public void Draw(GameDrawer drawer)
        {
            _map.Draw(drawer);
            _snake.Draw(drawer);
        }

        public void Iterate()
        {
            _snake.Iterate();

            var fruitPos = _map.GetFruitPos();

            if (fruitPos != null && _snake.GetHeadPos().AreEqualPos(fruitPos))
            {
                _map.GenerateNewFruit(_snake.GetPoints());
                _snake.Increase();
            }
        }

        public void MoveDown()
        {
            _snake.MoveDown();
        }

        public void MoveLeft()
        {
            _snake.MoveLeft();
        }

        public void MoveRight()
        {
            _snake.MoveRight();
        }

        public void MoveUp()
        {
            _snake.MoveUp();
        }
       
    }
}
