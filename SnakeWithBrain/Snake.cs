using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeWithBrain
{
    class Snake : IMoveable, IDrawable, IControl
    {
        private int _dirX;
        private int _dirY;

        private readonly Head _head;
        private readonly List<Body> _segments = new List<Body>();

        private int _increaseWith;

        public Snake(int x, int y, int startCount = 6)
        {
            _head = new Head(x, y);

            _segments.Add(_head);

            for (int i = 0; i < startCount; i++)
            {
                CreateNewTail(1);
            }
        }

        public void Increase()
        {
            _increaseWith = 1;
        }

        public void Draw(GameDrawer drawer)
        {
            foreach (var point in _segments)
            {
                point.Draw(drawer);
            }
        }

        public IReadOnlyList<Item> GetPoints()
        {
            var list = new List<Item>();

            list.Add(_head);
            list.AddRange(_segments);

            return _segments;
        }

        public Item GetHeadPos()
        {
            return _head;
        }

        public void Iterate()
        {
            if (_increaseWith > 0)
            {
                CreateNewTail();
                _increaseWith--;
            }

            if (_segments.Count > 1) { 
                for (int i = _segments.Count - 1; i > 0; i--)
                {
                    _segments[i].MoveTo(_segments[i-1]);
                }
            }
            
            _head.MoveTo(_head.X + _dirX, _head.Y + _dirY);
        }

        private void CreateNewTail(int offsetX = 0, int offsetY = 0)
        {
            var tail = _segments.Last();


            _segments.Add(new Body(tail.X+offsetX, tail.Y + offsetY));
        }

        public void MoveLeft()
        {
            if (_dirX == 1 && _dirY == 0) return;

            SetDir(-1, 0);
        }

        public void MoveRight()
        {
            if (_dirX == -1 && _dirY == 0) return;

            SetDir(1, 0);
        }

        public void MoveUp()
        {
            if (_dirX == 0 && _dirY == 1) return;

            SetDir(0, -1);
        }

        public void MoveDown()
        {
            if (_dirX == 0 && _dirY == -1) return;

            SetDir(0, 1);
        }

        private void SetDir(int x, int y)
        {
            _dirX = x;
            _dirY = y;
        }
    }
}
