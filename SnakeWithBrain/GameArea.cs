using System;
using System.Globalization;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SnakeWithBrain
{
    class GameArea : FrameworkElement
    {
        private static int GameSize = 50;
        private static int GameTick = 100;

        private Timer timer = new Timer(GameTick);

        private Pen mainPen = new Pen(Brushes.Black, 1.0);

        private Logic _logic = new Logic(GameSize, GameSize);
        private GameDrawer _drawer = new GameDrawer(GameSize, GameSize, new Pen(Brushes.Black, 2));
        
        private int _iteration = 0;

        public GameArea()
        {
            KeyDown+= HandleKeyPress;

            timer.AutoReset = true;
            timer.Elapsed += TimerOnElapsed;
            timer.Enabled = true;

            _logic.MoveUp();
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.P:
                    timer.Enabled = !timer.Enabled;
                    break;
                case Key.Left:
                    _logic.MoveLeft();
                    break;
                case Key.Right:
                    _logic.MoveRight();
                    break;
                case Key.Up:
                    _logic.MoveUp();
                    break;
                case Key.Down:
                    _logic.MoveDown();
                    break;
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _iteration++;
            _logic.Iterate();
            Dispatcher.Invoke(() =>
            {
                InvalidateVisual();
            });
         }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.Black, mainPen, new Rect(new Point(0,0),new Size(ActualWidth, ActualHeight)));

            _drawer.Update(drawingContext, ActualWidth, ActualHeight);

            _logic.Draw(_drawer);

            //drawingContext.DrawRectangle(Brushes.White, mainPen, new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));
            _drawer.DrawGrid();

            DrawHUD(drawingContext);
        }

        
        private void DrawHUD(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brushes.Black, mainPen, new Rect(new Point(0, 0), new Size(100, 20)));
            var formated = new FormattedText($"Iteration: {_iteration}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 10, Brushes.White);
            drawingContext.DrawText(formated, new Point(0, 10));
        }
    }
}
