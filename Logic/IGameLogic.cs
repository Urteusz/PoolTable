using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Logic
{
    public interface IGameLogic
    {
        ITable getTable();

        bool AddBallCheck(IBall ball);
        List<IBall> getBalls();

        IBall getBall(Guid id);
        void Move(object sender, ElapsedEventArgs e);
        bool CheckAllCollision();

        bool CheckCollision(IBall ball);
        int getCountBall();

        Timer getTimer();

        bool CreateBalls(int count);

        void StopTimer();
    }
}
