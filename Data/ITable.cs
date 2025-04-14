using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface ITable
    {
        Table createTable(int width, int height);

        Guid Id_table { get; }

        Boolean AddBall(IBall ball);
        Boolean RemoveBall(IBall ball);

        int width { get; set; }
        int height { get; set; }

        List<IBall> balls { get; }

        int CountBalls();

        IBall GetBall(IBall ball);

    }
}
