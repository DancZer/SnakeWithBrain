using SnakeWithBrain.Brain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakeWithBrain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BrainWindow brainWindow;
        private LogWindow logWindow;

        public MainWindow()
        {
            InitializeComponent();
            
            brainWindow = new BrainWindow();
            logWindow = new LogWindow();

            brainWindow.Show();
            logWindow.Show();

            this.Left = 10;
            this.Top = 50;

            brainWindow.Left = 10+800+50;
            brainWindow.Top = 50;

            logWindow.Left = 10 + 800 + 50+600+50;
            logWindow.Top = 50;

            Focus();

            KeyDown += HandleKeyPress;

            Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            logWindow.Close();
            brainWindow.Close();
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.B:
                    SimpleNeuralNet.Run(brainWindow);
                    break;
                default:
                    Game.HandleKeyPress(sender, e);
                    break;
            }
        }
    }
}
