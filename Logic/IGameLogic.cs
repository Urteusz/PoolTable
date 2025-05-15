using Data;
using System;
using System.Collections.Generic;

namespace Logic
{
    public interface IGameLogic
    {
        ITable getTable();
        bool AddBallCheck(IBall ball);
        List<IBall> getBalls();
        IBall getBall(Guid id);
        void Move(object sender, EventArgs e); // Zmieniono z ElapsedEventArgs na EventArgs
        bool CheckCollision(IBall ball);
        int getCountBall();
        bool CreateBalls(int count);

    }
}