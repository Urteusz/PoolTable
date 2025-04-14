using Data;
using System.Numerics;

public class Ball : IBall
{
    private readonly Guid id = Guid.NewGuid(); // Fixed placement of 'readonly' modifier  
    private string _color;
    private float _x, _y, _r;
    private float _vx, _vy;

    public Ball(float x, float y, float r, float vx, float vy)
    {
        LosujKolor();
        this.x = x;
        this.y = y;
        this.r = r;
        this.vx = vx;
        this.vy = vy;
    }

    public Guid Id_ball
    {
        get { return id; }
    }

    public Ball createBall(int x, int y, int r, int vx, int vy)
    {
        Ball b = new Ball(x, y, r, vx, vy);
        return b;
    }

    public string color
    {
        get { return _color; }
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

    public float x
    {
        get { return _x; }
        set
        {
            if (value >= 0)
            {
                _x = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("x", "Współrzędna x nie może być ujemna");
            }
        }
    }

    public float y
    {
        get { return _y; }
        set
        {
            if (value >= 0)
            {
                _y = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("y", "Współrzędna y nie może być ujemna");
            }
        }
    }

    public float r
    {
        get { return _r; }
        set
        {
            if (value > 0)
            {
                _r = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("r", "Współrzędna r nie może być ujemna");
            }
        }
    }

    public float vx { get { return _vx; } set { _vx = value; } }
    public float vy { get { return _vy; } set { _vy = value; } }

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
