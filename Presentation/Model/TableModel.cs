using Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Model
{

    public class TableModel
    {
        private Rectangle _tableBorder;
        private Canvas _canvas;

        public int Width { get; }
        public int Height { get; }

        public ObservableCollection<BallModel> Balls { get; } = new();

        public TableModel(int width, int height)
        {
            Width = width;
            Height = height;

            _tableBorder = new Rectangle
            {
                Width = width,
                Height = height,
                Stroke = Brushes.Black,
                StrokeThickness = 5
            };

            _canvas = new Canvas
            {
                Width = width,
                Height = height,
                Background = Brushes.LightGreen
            };
        }


        public void AddBall(BallModel ballModel)
        {
            if (ballModel != null)
            {
                Balls.Add(ballModel);

                Binding leftBinding = new Binding("X") { Source = ballModel };
                Binding topBinding = new Binding("Y") { Source = ballModel };


                ballModel.Shape.SetBinding(Canvas.LeftProperty, leftBinding);
                ballModel.Shape.SetBinding(Canvas.TopProperty, topBinding);

                AddObject(ballModel.Shape);
            }
        }



        public void AddObject(UIElement obj)
        {
            _canvas.Children.Add(obj);
        }

        public Canvas canvas => _canvas;
        public Rectangle TableBorder => _tableBorder;
    }

}