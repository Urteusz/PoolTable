using Logic;
using Model;
using Data;
using System;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Controls;

namespace ModelView
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private TableModel tableModel;
        private IGameLogic gameLogicAPI;
        private readonly int canvasWidth = 800;
        private readonly int canvasHeight = 600;
        private bool _isDisposed = false;

        public ICommand StartCommand { get; }
        public ICommand CleanupCommand { get; }
        public string BallCountInput { get; set; }
        public object CanvasContent => tableModel?.canvas;

        public MainViewModel()
        {
            StartCommand = new RelayCommand(StartSimulation);
            CleanupCommand = new RelayCommand(Cleanup);

            // Subscribe to application exit event
            Application.Current.Exit += (s, e) => Dispose();

            // Alternative if you're using a window directly
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.Closing += (s, e) => Dispose();
            }
        }

        private void StartSimulation()
        {
            // Clean up existing resources first
            Cleanup();

            if (int.TryParse(BallCountInput, out int ballCount) && ballCount > 0)
            {
                tableModel = new TableModel(canvasWidth, canvasHeight);
                Table t = new Table(canvasWidth, canvasHeight);
                gameLogicAPI = new GameLogic(t, 0.995f);
                gameLogicAPI.getTimer().Elapsed += UpdateBallMove;
                bool success = CreateBalls(ballCount);
                if (!success)
                {
                    MessageBox.Show("Nie udało się utworzyć wszystkich piłek. Spróbuj ponownie.");
                    return;
                }

                gameLogicAPI.StartTimer();

                OnPropertyChanged(nameof(CanvasContent)); // odświeżenie widoku
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną liczbę piłek.");
            }
        }

        private void Cleanup()
        {
            if (gameLogicAPI != null)
            {
                var timer = gameLogicAPI.getTimer();
                if (timer != null)
                {
                    timer.Elapsed -= UpdateBallMove;
                    timer.Stop();
                    timer.Dispose();
                }
            }
        }

        public bool CreateBalls(int count)
        {
            if (gameLogicAPI.CreateBalls(count))
            {
                for (int i = 0; i < gameLogicAPI.getTable().balls.Count; i++)
                {
                    IBall ball = gameLogicAPI.getTable().balls[i];
                    BallModel ballModel = new BallModel(ball.x, ball.y, ball.r, ball.Id_ball, ball.color);
                    tableModel.AddBall(ballModel);
                }
                return true;
            }
            return false;
        }

        private void UpdateBallMove(object sender, EventArgs e)
        {
            if (_isDisposed) return;



            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var ballModel in tableModel.Balls)
                {
                    IBall logicBall = gameLogicAPI.getBall(ballModel.Id);
                    if (logicBall != null)
                    {
                        ballModel.X = logicBall.x;
                        ballModel.Y = logicBall.y;
                    }
                }
            });
        }

       

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Cleanup();
                }

                _isDisposed = true;
            }
        }

        ~MainViewModel()
        {
            Dispose(false);
        }
    }
}