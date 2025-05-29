using System;

namespace Data
{
    public interface ILogger
    {
        void LogBallCreate(IBall ball);
        void LogBallColisionWall(IBall ball, string Wall);
        void LogBallColision(IBall ball1, IBall ball2);

        void LoggerMessage(string type,string text);
        void Stop();
    }
}
