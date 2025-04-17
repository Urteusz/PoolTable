using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Ball : IBall
    {
        // Właściwości z interfejsu IBall
        public Guid Id_ball { get; private set; }
        private float _x, _y, _r, _vx, _vy;
        private string _color;

        public float x
        {
            get => _x;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(x), "X cannot be negative.");
                _x = value;
            }
        }

        public float y
        {
            get => _y;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(y), "Y cannot be negative.");
                _y = value;
            }
        }

        public float vx
        {
            get => _vx;
            set => _vx = value;
        }

        public float vy
        {
            get => _vy;
            set => _vy = value;
        }

        public string color
        {
            get => _color;
            set
            {
                if (value.Length != 6) throw new ArgumentException("Color must be exactly 6 characters.");
                if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^[A-F0-9]+$"))
                    throw new ArgumentException("Color must contain valid hexadecimal characters.");
                _color = value.ToUpper();
            }
        }

        public float r
        {
            get => _r;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(r), "Radius cannot be negative.");
                _r = value;
            }
        }

        // Konstruktor
        public Ball(float x, float y, float r, float vx, float vy)
        {
            Id_ball = Guid.NewGuid();  // Generowanie unikalnego Id
            this.x = x;
            this.y = y;
            this.r = r;
            this.vx = vx;
            this.vy = vy;
            this.color = "FFFFFF";  // Domyślny kolor (możesz zmienić)
        }

        // Metoda do tworzenia piłki (zdefiniowana w interfejsie)
        public Ball createBall(int x, int y, int r, int vx, int vy)
        {
            return new Ball(x, y, r, vx, vy);
        }

        global::Ball IBall.createBall(int x, int y, int r, int vx, int vy)
        {
            throw new NotImplementedException();
        }
    }
}
