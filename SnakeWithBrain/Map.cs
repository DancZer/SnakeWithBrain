using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeWithBrain
{
    class Map : IDrawable
    {
        public readonly Random _generator;

        private Fruit _fruit;

        public int Width { get; }
        public int Height { get; }

        public Map(int width, int height, int seed = 0)
        {
            Width = width;
            Height = height;

            _generator = new Random(seed);
        }
        
        public void Draw(GameDrawer drawer)
        {
            if (_fruit != null)
            {
                _fruit.Draw(drawer);
            }
        }

        public Item GetFruitPos()
        {
            return _fruit;
        }

        public void GenerateNewFruit(IReadOnlyList<Item> exclude)
        {
            bool retry;
            int x, y;

            do
            {
                x = _generator.Next() % Width;
                y = _generator.Next() % Height;

                retry = false;

                foreach (var point in exclude)
                {
                    if (point.AreEqualPos(x, y))
                    {
                        retry = true;
                        break;
                    }
                }
            } while (retry);

            _fruit = new Fruit(x, y);
        }
    }
}
