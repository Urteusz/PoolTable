using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Table : ITable
    {
        private Guid  _id_table = Guid.NewGuid();
        private List<IBall> _balls;
        private int _width;
        private int _height;

        public Guid Id_table
        {
            get
            {
                return _id_table;
            }
        }
        public int width
        {
            get {
                return _width;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("width", "Szerokość nie może być ujemna");
                }
                _width = value;
            }
        }
        public int height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("height", "Wysokość nie może być ujemna");
                }
                _height = value;
            }
        }

        public List<IBall> balls
        {
            get
            {
                return _balls;
            }
        }

        public Table(int w, int h)
        {
            this.width = w;
            this.height = h;
            this._balls = new List<IBall>();
        }

        public void SetTableSize(int w, int h)
        {
            if (w < 0 || h < 0)
            {
                throw new ArgumentOutOfRangeException("width or height", "Szerokość i wysokość nie mogą być ujemne");
            }
            this.width = w;
            this.height = h;
        }

        public Table createTable(int width, int height)
        {
            Table t = new Table(width, height);
            return t;
        }

        public bool AddBall(IBall ball)
        {
            if (ball.x-ball.r < 0 || ball.x+ball.r > width || ball.y-ball.r < 0 || ball.y+ball.r > height)
            {
                return false;
            }
            if (_balls.Any(b => b.Id_ball == ball.Id_ball))
            {
                return false;
            }
            _balls.Add(ball);
            return true;
        }

        public bool RemoveBall(IBall ball)
        {
            if (_balls.Contains(ball))
            {
                _balls.Remove(ball);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IBall GetBall(IBall ball)
        {
            return _balls.FirstOrDefault(b => b.Id_ball == ball.Id_ball);
        }

        public int CountBalls()
        {
            return _balls.Count;
        }


    }
}
