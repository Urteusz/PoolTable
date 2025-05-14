using Logic;
using Model;
using Data;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Reactive;

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

        // Observable dla aktualizacji pozycji kulek
        private readonly Subject<Unit> _updateSubject = new Subject<Unit>();
        private IDisposable _updateSubscription;
        private CancellationTokenSource _cancellationTokenSource;

        public ICommand StartCommand { get; }
        public ICommand CleanupCommand { get; }
        public string BallCountInput { get; set; }
        public object CanvasContent => tableModel?.canvas;

        public MainViewModel()
        {
            StartCommand = new RelayCommand(StartSimulation);
            CleanupCommand = new RelayCommand(Cleanup);

            // Subscribe to application exit event
            if (Application.Current != null)
            {
                Application.Current.Exit += OnApplicationExit;
            }

            // Alternative if you're using a window directly
            if (Application.Current?.MainWindow != null)
            {
                Application.Current.MainWindow.Closing += OnWindowClosing;
            }
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            Dispose();
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dispose();
        }

        private void StartSimulation()
        {
            // Clean up existing resources first
            Cleanup();

            if (int.TryParse(BallCountInput, out int ballCount) && ballCount > 0)
            {
                tableModel = new TableModel(canvasWidth, canvasHeight);
                Table t = new Table(canvasWidth, canvasHeight);
                gameLogicAPI = new GameLogic(t);

                bool success = CreateBalls(ballCount);
                if (!success)
                {
                    MessageBox.Show("Nie udało się utworzyć wszystkich piłek. Spróbuj ponownie.");
                    return;
                }

                // Zamiast timera używamy Observable z intervalem
                _cancellationTokenSource = new CancellationTokenSource();

                // Observable emitujące co 16ms (60 FPS)
                _updateSubscription = Observable
                    .Interval(TimeSpan.FromMilliseconds(16))
                    .TakeUntil(_updateSubject)
                    .Subscribe(_ => UpdateBallMove());

                // Uruchamianie logiki gry bez timera
                StartGameLogicWithoutTimer();

                OnPropertyChanged(nameof(CanvasContent)); // odświeżenie widoku
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną liczbę piłek.");
            }
        }

        private void StartGameLogicWithoutTimer()
        {
            // Uruchamiamy logikę w osobnym tasku
            Task.Run(async () =>
            {
                try
                {
                    while (!_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        if (gameLogicAPI != null)
                        {
                            // Wywołujemy Move bezpośrednio zamiast czekać na timer
                            gameLogicAPI.Move(null, null);
                        }
                        await Task.Delay(16, _cancellationTokenSource.Token); // 60 FPS
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                }
            }, _cancellationTokenSource.Token);
        }

        private void Cleanup()
        {
            if (_isDisposed) return;

            // Zatrzymujemy Observable
            _updateSubscription?.Dispose();
            _updateSubject?.OnNext(Unit.Default);

            // Anulujemy wszystkie asynchroniczne operacje
            _cancellationTokenSource?.Cancel();

            // Give a moment for tasks to complete
            Thread.Sleep(50);

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;

            if (gameLogicAPI != null)
            {
                // Już nie ma timera do zatrzymania
                gameLogicAPI = null;
            }

            _updateSubscription = null;
        }

        public bool CreateBalls(int count)
        {
            if (gameLogicAPI?.CreateBalls(count) == true)
            {
                var table = gameLogicAPI.getTable();
                if (table?.balls != null)
                {
                    for (int i = 0; i < table.balls.Count; i++)
                    {
                        IBall ball = table.balls[i];
                        if (ball != null)
                        {
                            BallModel ballModel = new BallModel(ball.x, ball.y, ball.r, ball.Id_ball, ball.color);
                            tableModel?.AddBall(ballModel);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private void UpdateBallMove()
        {
            if (_isDisposed || tableModel?.Balls == null || gameLogicAPI == null)
                return;

            try
            {
                // Check if Dispatcher is not null and not shut down
                if (Application.Current?.Dispatcher != null && !Application.Current.Dispatcher.HasShutdownStarted)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (_isDisposed || tableModel?.Balls == null) return;

                        foreach (var ballModel in tableModel.Balls)
                        {
                            if (ballModel == null) continue;

                            IBall logicBall = gameLogicAPI?.getBall(ballModel.Id);
                            if (logicBall != null)
                            {
                                ballModel.X = logicBall.x - logicBall.r;
                                ballModel.Y = logicBall.y - logicBall.r;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                System.Diagnostics.Debug.WriteLine($"Error in UpdateBallMove: {ex.Message}");
            }
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
                    // Unsubscribe event handlers to prevent memory leaks
                    if (Application.Current != null)
                    {
                        Application.Current.Exit -= OnApplicationExit;
                    }

                    if (Application.Current?.MainWindow != null)
                    {
                        Application.Current.MainWindow.Closing -= OnWindowClosing;
                    }

                    Cleanup();
                    _updateSubject?.Dispose();
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