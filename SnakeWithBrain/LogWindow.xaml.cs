using SnakeWithBrain.Brain;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SnakeWithBrain
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
            Console.logWindow = this;
        }

        public static class Console
        {
            private static StringBuilder stringBuilder = new StringBuilder();
            public static LogWindow logWindow;

            public static void WriteLine(string text = "", params object[] args)
            {
                stringBuilder.AppendLine(string.Format(text, args));

                Flush();
            }

            private static void Flush()
            {
                if (logWindow != null)
                {
                    logWindow.ConsoleTextBox.Text = stringBuilder.ToString();
                }
            }
        }
    }
}
