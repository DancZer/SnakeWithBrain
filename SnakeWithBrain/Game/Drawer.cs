using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SnakeWithBrain
{
    class Drawer
    {
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Pen gridPen;

        private DrawingContext _contex;
        private double _itemWidth;
        private double _itemHeight;
        private double _actualWidth;
        private double _actualHeight;

        public Drawer(int mapWidth, int mapHeight, Pen gridPen)
        {
            this._mapWidth = mapWidth;
            this._mapHeight = mapHeight;
            this.gridPen = gridPen;
        }

        public void Update(DrawingContext context, double actualWidth, double actualHeight)
        {
            _contex = context;
            _itemWidth = actualWidth / _mapWidth;
            _itemHeight = actualHeight / _mapHeight;

            _actualWidth = actualWidth;
            _actualHeight = actualHeight;
        }

        public void FillRect(int x, int y, Brush brush)
        {
            _contex.DrawRectangle(brush, new Pen(), new Rect(new Point(x* _itemWidth, y * _itemHeight), new Size(_itemWidth, _itemHeight)));
        }

        public void DrawGrid()
        {
            var pen = new Pen(Brushes.Black, 1);

            for (int x = 0; x < _mapWidth; x++)
            {
                double lv = x * _itemWidth;

                _contex.DrawLine(pen, new Point(lv, 0), new Point(lv, _actualHeight));
            }

            for (int y = 0; y < _mapWidth; y++)
            {
                double lv = y * _itemWidth;

                _contex.DrawLine(pen, new Point(0, lv), new Point(_actualHeight, lv));
            }
        }
    }
}
