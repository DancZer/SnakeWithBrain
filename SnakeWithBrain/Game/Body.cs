using System;
using System.Windows.Media;

namespace SnakeWithBrain
{
    class Body : Item, IDrawable
    {
        public Body(Item p) : base(p)
        {
        }
        public Body(int x, int y) : base(x, y)
        {
        }

        public virtual void Draw(Drawer drawer)
        {
            drawer.FillRect(X, Y, Brushes.White);
        }
    }
}
