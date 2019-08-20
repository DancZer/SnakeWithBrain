using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnakeWithBrain
{
    class Map : IDrawable
    {
        public Item FruitPos
        {
            get { return _fruit; }
        }

        public int Width { get; }
        public int Height { get; }

        private readonly Random _generator;

        private Fruit _fruit;

        private int[,] _fruitGeneratorCache;

        public int EmptyPlacesCount;
        
        public Map(int width, int height, int seed = 0)
        {
            Width = width;
            Height = height;

            _generator = new Random(seed);

            _fruitGeneratorCache = new int[Width,Height];
        }
        
        public void Draw(Drawer drawer)
        {
            if (_fruit != null)
            {
                _fruit.Draw(drawer);
            }
        }

        public bool GenerateNewFruit(IReadOnlyList<Item> exclude)
        {
            //reset cache
            var i = 0;
            for (int iX = 0; iX < Width; iX++)
            {
                for (int iY = 0; iY < Height; iY++)
                {
                    _fruitGeneratorCache[iX, iY] = i++;
                }
            }

            //remove used positions
            foreach (var item in exclude)
            {
                if (item.X >= Width || item.X < 0 || item.Y < 0 || item.Y >= Height)
                {
                    throw new SnakeException();
                }
                _fruitGeneratorCache[item.X, item.Y] = -1;
            }


            //collect not used positions
            var availablePositions = new List<int>();

            for (int iX = 0; iX < Width; iX++)
            {
                for (int iY = 0; iY < Height; iY++)
                {
                    int tmpIdx = _fruitGeneratorCache[iX, iY];

                    if (tmpIdx >= 0)
                    {
                        availablePositions.Add(tmpIdx);
                    }
                }
            }

            EmptyPlacesCount = availablePositions.Count;

            //no more empty space available
            if (availablePositions.Count == 0) return false;

            //get random positon
            int idx = availablePositions[_generator.Next() % availablePositions.Count];

            //calculate 2D indexes
            var x = idx / Height;
            var y = idx % Height;
            
            _fruit = new Fruit(x, y);

            return true;
        }
    }
}
