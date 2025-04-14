using Data;
using Logic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Model
{
    public class TableModel
    {
        private Table _table;
        private List<BallModel> _balls = new List<BallModel>();
        private Rectangle _tableBorder;


        public Rectangle TableBorder
        {
            get { return _tableBorder; }
            set { _tableBorder = value; }
        }
        public Table Table
        {
            get { return _table; }
            set { _table = value; }
        }

        public TableModel(int width, int height)
        {
            this._table = new Table(width,height);
            for(int i = 0; i < _table.balls.Count; i++)
            {
                
                BallModel ballModel = new BallModel(_table.balls[i]);
                this._balls.Add(ballModel);
            }
            this._tableBorder = new Rectangle
            {
                Width = _table.width,
                Height = _table.height,
                Stroke = Brushes.Black,
                StrokeThickness = 5
            };
        }

        public List<BallModel> Balls
        {
            get { return _balls; }
        }

        public void AddBall(BallModel ballModel)
        {
            if (ballModel != null)
            {
                _table.balls.Add(ballModel.ball);
                _balls.Add(ballModel);
            }
        }
    }
}
