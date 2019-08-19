using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SnakeWithBrain
{
    class Fruit : Item, IDrawable
    {
        public Fruit(Item p) : base(p)
        {
        }
        public Fruit(int x, int y) : base(x, y)
        {
        }

        public void Draw(GameDrawer drawer)
        {
            drawer.FillRect(X, Y, Brushes.Red);
        }
    }
}
