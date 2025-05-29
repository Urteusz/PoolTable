using Data;
using System;
using System.Numerics;

public class Ball : IBall
{
    private readonly Guid id;
    private string _color;
    private float _x, _y, _r;
    private float _vx, _vy;

    // Domyślny konstruktor generuje nowe GUID
    public Ball(float x, float y, float r, float vx, float vy)
    {
        id = Guid.NewGuid();
        LosujKolor();
        this.x = x;
        this.y = y;
        this.r = r;
        this.vx = vx;
        this.vy = vy;
    }

    // 🔧 Nowy konstruktor z ID jako int
    public Ball(int idInt, float x, float y, float r, float vx, float vy)
    {
        id = CreateGuidFromInt(idInt);
        LosujKolor();
        this.x = x;
        this.y = y;
        this.r = r;
        this.vx = vx;
        this.vy = vy;
    }

    public Guid Id_ball => id;

    public Ball createBall(int x, int y, int r, int vx, int vy)
    {
        return new Ball(x, y, r, vx, vy);
    }

    public string color
    {
        get => _color;
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
        get => _x;
        set
        {
            if (value >= 0) _x = value;
            else throw new ArgumentOutOfRangeException("x", "Współrzędna x nie może być ujemna");
        }
    }

    public float y
    {
        get => _y;
        set
        {
            if (value >= 0) _y = value;
            else throw new ArgumentOutOfRangeException("y", "Współrzędna y nie może być ujemna");
        }
    }

    public float r
    {
        get => _r;
        set
        {
            if (value > 0) _r = value;
            else throw new ArgumentOutOfRangeException("r", "Promień musi być większy od zera");
        }
    }

    public float vx { get => _vx; set => _vx = value; }
    public float vy { get => _vy; set => _vy = value; }

    public void SetPosition()
    {
        x += vx;
        y += vy;
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
        color = new string(znakiHex);
    }

    // 🧠 Tworzy Guid z int-a – np. na potrzeby testów
    private Guid CreateGuidFromInt(int value)
    {
        byte[] bytes = new byte[16];
        BitConverter.GetBytes(value).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}
