using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IBall
    {
        Ball createBall(int x, int y, int r, int vx, int vy);


        Guid Id_ball { get; }

        float x { get; set; }
        float y { get; set; }

        float vx { get; set; }
        float vy { get; set; }

        string color { get; set; }

        float r { get; set; }





    }
}
