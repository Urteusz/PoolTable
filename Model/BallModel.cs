
using System.Windows.Shapes;
using System.Windows.Media;
using Data;

namespace Model
{
    public class BallModel
    {
        private IBall _ball;
        private Ellipse _ballShape;

        public BallModel(float x, float y, float r, float vx, float vy)
        {
            this._ball = new Ball(x, y, r, vx, vy);
            this._ballShape = new Ellipse
            {
                Width = _ball.r * 2,
                Height = _ball.r * 2,
                Fill = (Brush)new BrushConverter().ConvertFromString("#" + _ball.color),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            _ballShape.Tag = _ball.Id_ball;
        }
        public BallModel(IBall ball)
        {
            this._ball = ball; // Explicit cast to resolve CS0266
            this._ballShape = new Ellipse
            {
                Width = _ball.r * 2,
                Height = _ball.r * 2,
                Fill = (Brush)new BrushConverter().ConvertFromString("#" + _ball.color),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            _ballShape.Tag = _ball.Id_ball;
        }

        public Ellipse ballShape
        {
            get { return _ballShape; }
        }

        public IBall ball
        {
            get { return _ball; }
        }
    }
}
