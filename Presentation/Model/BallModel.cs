using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Model
{
    public class BallModel : INotifyPropertyChanged
    {
        private float _x;
        private float _y;

        public double X
        {
            get => (double)_x;
            set
            {
                float newVal = (float)value;
                if (_x != newVal)
                {
                    _x = newVal;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        public double Y
        {
            get => (double)_y;
            set
            {
                float newVal = (float)value;
                if (_y != newVal)
                {
                    _y = newVal;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        public float Radius { get; }
        public string Color { get; }
        public Guid Id { get; }

        public Ellipse Shape { get; }

        public BallModel(float x, float y, float radius, Guid id, string color = "FF0000")
        {
            _x = x;
            _y = y;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
