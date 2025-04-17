using Model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

public class TableModel
{
    private List<BallModel> _balls = new();
    private Rectangle _tableBorder;
    private Canvas _canvas;

    public int Width { get; }
    public int Height { get; }

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

    public List<BallModel> Balls => _balls;

    public void AddBall(BallModel ballModel)
    {
        if (ballModel != null)
        {
            _balls.Add(ballModel);
        }
        AddObject(ballModel.Shape);
    }

    public void AddObject(UIElement obj)
    {
        _canvas.Children.Add(obj);
    }

    public Canvas canvas => _canvas;
    public Rectangle TableBorder => _tableBorder;
}
