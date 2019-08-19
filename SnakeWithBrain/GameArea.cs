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
        private static int GameSize = 10;
        private static int GameTick = 200;

        private Timer _timer = new Timer(GameTick);

        private Pen mainPen = new Pen(Brushes.Black, 1.0);

        private Logic _logic = new Logic(GameSize, GameSize);
        private GameDrawer _drawer = new GameDrawer(GameSize, GameSize, new Pen(Brushes.Black, 2));
        
        private int _iteration = 0;

        public GameArea()
        {
            KeyDown+= HandleKeyPress;

            _timer.AutoReset = true;
            _timer.Elapsed += TimerOnElapsed;
            _timer.Enabled = true;

            _logic.MoveLeft();

            Dispatcher.ShutdownStarted += (object s, EventArgs e) =>
            {
                _timer.Stop();
                _timer = null;
            };
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (_timer == null) return;

            switch (e.Key)
            {
                case Key.R:
                    _logic.Reset();
                    _logic.MoveLeft();
                    _timer.Start();
                    break;
                case Key.P:
                    _timer.Enabled = !_timer.Enabled;
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
                case Key.D:
                    _logic.DebugMode = !_logic.DebugMode;
                    break;
                case Key.I:
                    TimerOnElapsed(this, null);
                    break;
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_timer == null) return;

            if (_logic.State != Logic.GameState.InProgress) return;

            try
            {
                _iteration++;
                _logic.Iterate();
                Dispatcher.Invoke(() =>
                {
                    InvalidateVisual();
                });
            }
            catch (SnakeException exception)
            {
                Console.WriteLine(exception);
                Dispatcher.Invoke(() =>
                {
                    InvalidateVisual();
                });
                _timer.Stop();
            }
         }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (_logic.State == Logic.GameState.InProgress)
            {
                drawingContext.DrawRectangle(Brushes.Black, mainPen, new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));
            }
            else if(_logic.State == Logic.GameState.Failed)
            {
                drawingContext.DrawRectangle(Brushes.DarkRed, mainPen, new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));
            }
            

            _drawer.Update(drawingContext, ActualWidth, ActualHeight);

            _logic.Draw(_drawer);

            if (_logic.State == Logic.GameState.Success)
            {
                drawingContext.DrawRectangle(Brushes.DarkGreen, mainPen, new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));
            }

            //drawingContext.DrawRectangle(Brushes.White, mainPen, new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));
            _drawer.DrawGrid();

            DrawHUD(drawingContext);
        }

        
        private void DrawHUD(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brushes.Black, mainPen, new Rect(new Point(0, 0), new Size(400, 20)));
            var formated = new FormattedText($"G: {_logic.DebugMode}, {_logic.State}, {_iteration};S: {_logic.Snake.Head}, {_logic.Snake.GetPoints().Count};M:{_logic.Map.Width}x{_logic.Map.Height}, {_logic.Map.EmptyPlacesCount}", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 10, Brushes.White);
            drawingContext.DrawText(formated, new Point(0, 10));
        }
    }
}
