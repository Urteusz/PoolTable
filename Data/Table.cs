using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Table
    {
        private List<Ball> _pilki;
        private int _width;
        private int _height;
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

        public List<Ball> pilki
        {
            get
            {
                return _pilki;
            }
            set
            {
                _pilki = value;
            }
        }

        public Table(int w, int h)
        {
            this.width = w;
            this.height = h;
            _pilki = new List<Ball>();
        }

        
    }
}
