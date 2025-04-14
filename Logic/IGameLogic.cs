using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface IGameLogic
    {
        ITable getTable();

        bool AddBallCheck(IBall ball);
        List<IBall> getBalls();
        void Move(IBall ball);
        bool CheckAllCollision();

        bool CheckCollision(IBall ball);
        int getCountBall();
    }
}
