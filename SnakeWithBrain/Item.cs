
namespace SnakeWithBrain
{
    abstract class Item
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Item(Item p)
        {
            X = p.X;
            Y = p.Y;
        }
        public Item(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveTo(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveTo(Item p)
        {
            X = p.X;
            Y = p.Y;
        }

        public bool AreEqualPos(int x, int y)
        {
            return X == x && Y == y;
        }

        public bool AreEqualPos(Item p)
        {
            return AreEqualPos(p.X, p.Y);
        }

        public override string ToString()
        {
            return X + ":" + Y;
        }
    }
}
