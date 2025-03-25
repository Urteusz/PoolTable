using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private List<Ellipse> ballShapes = new();
        private Canvas canvas;
        private DispatcherTimer timer;
        private Table table;
        private int selectedBallIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
            table = new Table(800, 600);
            gameLogic = new GameLogic(table, 0.99f);
            gameLogic.Start();
            Ball ball = gameLogic.getBall(0);

          
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

            for(int i = 0; i<gameLogic.getBallsCount();i++)
            {
                Ball pilka = gameLogic.getBall(i);
                Ellipse ballShape = new Ellipse
                {
                    Width = pilka.r*2,
                    Height = pilka.r*2,
                    Fill = (Brush)new BrushConverter().ConvertFromString("#" + pilka.color),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                ballShape.Tag = i;
                ballShape.MouseDown += Ball_Clicked;
                ballShapes.Add(ballShape);
                canvas.Children.Add(ballShape);
                UpdateBallPosition(pilka, ballShape);
            }
           

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += UpdateBallMove;
            timer.Start();
        }

        private void Ball_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is Ellipse clickedBall)
            {
                selectedBallIndex = (int)clickedBall.Tag;
            }
        }

        private void UpdateBallMove(object sender, EventArgs e)
        {
            if(selectedBallIndex != -1)
            {
                Ball ball = gameLogic.getBall(selectedBallIndex);

                gameLogic.Move(ball);
                if (ball.vy != 0 || ball.vx != 0)
                {
                    UpdateBallPosition(ball, ballShapes[selectedBallIndex]);
                }
            }
        }

        private void UpdateBallPosition(Ball ball, Ellipse shape)
        {
            Canvas.SetLeft(shape, ball.x - ball.r);
            Canvas.SetTop(shape, ball.y - ball.r);
        }
    }
}
