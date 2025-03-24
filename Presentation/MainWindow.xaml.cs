using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Data;
using Logic;
using Table = Data.Table;

namespace Presentation
{
    public partial class MainWindow : Window
    {
        private GameLogic gameLogic;
        private Ellipse ballShape;
        private Canvas canvas;
        private DispatcherTimer timer;
        private Table table;

        public MainWindow()
        {
            InitializeComponent();
            table = new Table(800, 600);
            gameLogic = new GameLogic(table);
            gameLogic.Start();
            Ball ball = gameLogic.getBall(0);

            // Tworzymy interfejs graficzny
            canvas = new Canvas()
            {
                Width = table.width,
                Height = table.height,
                Background = Brushes.LightGreen
            };
            Content = canvas;

            Rectangle tableBorder = new Rectangle
            {
                Width = table.width,
                Height = table.height,
                Stroke = Brushes.Black,
                StrokeThickness = 5
            };
            canvas.Children.Add(tableBorder);

            // Tworzymy kółko
            ballShape = new Ellipse
            {
                Width = ball.r,
                Height = ball.r,
                Fill = (Brush)new BrushConverter().ConvertFromString("#"+ball.color),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            canvas.Children.Add(ballShape);
            UpdateBallPosition(gameLogic.getBall(0));

            // Ustawiamy timer dla ruchu piłki
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += UpdateBallFall;
            timer.Start();
        }

        private void UpdateBallFall(object sender, EventArgs e)
        {
            Ball ball = gameLogic.getBall(0);

           
            if (gameLogic.Move(ball, 0, 5))
            {
                timer.Stop();
            }
            else
            {
                UpdateBallPosition(ball);
            }

            
        }

        private void UpdateBallPosition(Ball ball)
        {
            Canvas.SetLeft(ballShape, ball.x);
            Canvas.SetTop(ballShape, ball.y);
        }
    }
}
