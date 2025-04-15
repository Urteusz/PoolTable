using Logic;
using Model;
using Data;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ModelView
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private CanvasModel _canvas;
        private IGameLogic gameLogicAPI;
        private DispatcherTimer timer;
        private readonly int canvasWidth = 800;
        private readonly int canvasHeight = 600;

        public ICommand StartCommand { get; }
        public string BallCountInput { get; set; }

        public object CanvasContent => _canvas?.canvas;

        public MainViewModel()
        {
            StartCommand = new RelayCommand(_ => StartSimulation());
        }

        private void StartSimulation()
        {
            if (int.TryParse(BallCountInput, out int ballCount) && ballCount > 0)
            {
                TableModel tableModel = new TableModel(canvasWidth, canvasHeight);
                gameLogicAPI = new GameLogic(tableModel.Table, 0.995f);
                _canvas = new CanvasModel(tableModel);

                bool success = CreateBalls(ballCount);

                if (!success)
                {
                    MessageBox.Show("Nie udało się utworzyć wszystkich piłek. Spróbuj ponownie.");
                    return;
                }

                timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(10) };
                timer.Tick += UpdateBallMove;
                timer.Start();

                OnPropertyChanged(nameof(CanvasContent)); // odświeżenie widoku
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną liczbę piłek.");
            }
        }

        public bool CreateBalls(int count)
        {
            if (count <= 0) return false;

            int createdBalls = 0;
            int maxTriesPerBall = 100;

            while (createdBalls < count)
            {
                bool placed = false;
                int tries = 0;

                while (!placed && tries < maxTriesPerBall)
                {
                    float x = Random.Shared.Next(0, (int)_canvas.tableModel.Table.width);
                    float y = Random.Shared.Next(0, (int)_canvas.tableModel.Table.height);
                    float vx = Random.Shared.Next(-20, 20);
                    float vy = Random.Shared.Next(-20, 20);
                    BallModel ballModel = new BallModel(x, y, 25, vx, vy);

                    if (gameLogicAPI.AddBallCheck(ballModel.ball))
                    {
                        _canvas.tableModel.AddBall(ballModel);
                        UpdateBallPosition(ballModel.ball, ballModel.ballShape);
                        _canvas.addObject(ballModel.ballShape);
                        placed = true;
                        createdBalls++;
                    }

                    tries++;
                }

                if (!placed)
                {
                    Console.WriteLine($"Nie udało się dodać kuli numer {createdBalls + 1} po {maxTriesPerBall} próbach.");
                    return false;
                }
            }

            return true;
        }

        private void UpdateBallMove(object sender, EventArgs e)
        {
            foreach (var ballModel in _canvas.tableModel.Balls)
            {
                gameLogicAPI.Move(ballModel.ball);
                if (ballModel.ball.vx != 0 || ballModel.ball.vy != 0)
                {
                    UpdateBallPosition(ballModel.ball, ballModel.ballShape);
                }
            }
        }

        private void UpdateBallPosition(IBall ball, Ellipse shape)
        {
            Canvas.SetLeft(shape, ball.x - ball.r);
            Canvas.SetTop(shape, ball.y - ball.r);
        }
    }
}
