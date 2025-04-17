using System.Windows.Media;
using System.Windows.Shapes;

namespace Model
{
    public class BallModel
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Radius { get; }
        public string Color { get; }
        public Guid Id { get; }

        public Ellipse Shape { get; }

        public BallModel(float x, float y, float radius, Guid id, string color = "FF0000" )
        {
            X = x;
            Y = y;
            Radius = radius;
            Color = color;
            Id = id;

            Shape = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = (Brush)new BrushConverter().ConvertFromString("#" + color),
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Tag = Id
            };
        }
    }
}
