using System;
using System.Windows;
using System.Windows.Threading;
using ModelView;

namespace Presentation
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private MainViewModel view;

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BallCountTextBox.Text, out int ballCount) && ballCount > 0)
            {
                view = new MainViewModel(800, 600);
                bool success = view.CreateBalls(ballCount);

                if (!success)
                {
                    MessageBox.Show("Nie udało się utworzyć wszystkich piłek. Spróbuj ponownie.");
                    return;
                }

                CanvasContainer.Content = view.canvas.canvas;

                timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
                timer.Tick += view.UpdateBallMove;
                timer.Start();
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną liczbę piłek.");
            }
        }
    }
}