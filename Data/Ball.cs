using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Ball
    {
        private string _color;
        private int _x;
        private int _y;
        private int _r;

        public string color
        {
            get
            {
                return _color;
            }
            set
            {
                if (value == null || value.Length != 6)
                    throw new ArgumentException("Kod hex musi składać się dokładnie z 6 znaków");
                foreach (char c in value)
                {
                    if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')))
                        throw new ArgumentException("Kod hex może zawierać tylko cyfry (0-9) i litery (A-F)");
                }
                _color = value.ToUpper();
            }
        }

        public int x
        {
            get
            {
                return _x;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("x", "Współrzędna x nie może być ujemna");
                }
                _x = value;
            }
        }

        public int y
        {
            get
            {
                return _y; 
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("y", "Współrzędna y nie może być ujemna");
                }
                _y = value; 
            }
        }

        public int r
        {
            get
            {
                return _r;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("r", "Promień nie może być ujemny");
                }
                _r = value;
            }
        }

        public Ball(int x, int y, int r)
        {
            LosujKolor();
            this.x = x;
            this.y = y;
            this._r = r;
        }

        public void LosujKolor()
        {
            Random _random = new Random();
            char[] znakiHex = new char[6];
            string dozwoloneZnaki = "0123456789ABCDEF";
            for (int i = 0; i < 6; i++)
            {
                znakiHex[i] = dozwoloneZnaki[_random.Next(dozwoloneZnaki.Length)];
            }
            this.color = new string(znakiHex);
        }
    }
}