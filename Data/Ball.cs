public class Ball
{
    private string _color;
    private double _x, _y, _r;
    private double _vx, _vy;

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

    public double x { get { return _x; } set { _x = value; } }
    public double y { get { return _y; } set { _y = value; } }
    public double r { get { return _r; } set { _r = value; } }
    public double vx { get { return _vx; } set { _vx = value; } }
    public double vy { get { return _vy; } set { _vy = value; } }

    public Ball(double x, double y, double r, double vx, double vy)
    {
        LosujKolor();
        this.x = x;
        this.y = y;
        this.r = r;
        this.vx = vx;
        this.vy = vy;
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
