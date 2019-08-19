using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SnakeWithBrain
{
    class Head : Body
    {
        public Head(int x, int y) : base(x, y)
        {
        }

        public override void Draw(GameDrawer drawer)
        {
            drawer.FillRect(X, Y, Brushes.White);
        }
    }
}
