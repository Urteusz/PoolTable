using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace Data
{
    public class Logger : ILogger
    {
        private readonly string _filePath;
        private readonly BlockingCollection<string> _logQueue = new(new ConcurrentQueue<string>());
        private readonly Thread _loggingThread;
        private bool _isRunning = true;

        public Logger(string filePath)
        {
            _filePath = filePath;
            _loggingThread = new Thread(ProcessQueue)
            {
                IsBackground = true
            };
            _loggingThread.Start();
        }

        public void LogBallCreate(IBall ball)
        {
            if (!_isRunning) return;
            string logEntry = $"{DateTime.UtcNow:O};[CREATE];ID={ball.Id_ball};X={ball.x:F2};Y={ball.y:F2};VX={ball.vx:F2};VY={ball.vy:F2};R={ball.r:F2};COLOR={ball.color}";
            try { _logQueue.Add(logEntry); }
            catch (InvalidOperationException) { /* kolejka zamknięta */ }
        }

        public void LogBallColisionWall(IBall ball, string Wall) { 
            if (!_isRunning) return;
            string logEntry = $"{DateTime.UtcNow:O};[COLLISION_WALL_{Wall}];ID={ball.Id_ball};X={ball.x:F2};Y={ball.y:F2};VX={ball.vx:F2};VY={ball.vy:F2}";
            try { _logQueue.Add(logEntry); }
            catch (InvalidOperationException) { /* kolejka zamknięta */ }
        }

        public void LogBallColision(IBall ball1, IBall ball2)
        {
            if (!_isRunning) return;
            string logEntry = $"{DateTime.UtcNow:O};[COLLISION];ID1={ball1.Id_ball};ID2={ball2.Id_ball};X1={ball1.x:F2};Y1={ball1.y:F2};X2={ball2.x:F2};Y2={ball2.y:F2}";
            try { _logQueue.Add(logEntry); }
            catch (InvalidOperationException) { /* kolejka zamknięta */ }
        }

        public void LoggerMessage(string type, string text)
        {
            if (!_isRunning) return;
            string logEntry = $"{DateTime.UtcNow:O};[{type}];{text}";
            try { _logQueue.Add(logEntry); }
            catch (InvalidOperationException) { /* kolejka zamknięta */ }
        }

        private string SerializeBall(IBall ball)
        {
            return $"{DateTime.UtcNow:O};ID={ball.Id_ball};X={ball.x:F2};Y={ball.y:F2};VX={ball.vx:F2};VY={ball.vy:F2};R={ball.r:F2};COLOR={ball.color}";
        }

        private void ProcessQueue()
        {
            using StreamWriter writer = new(_filePath, append: true, Encoding.ASCII);
            foreach (var entry in _logQueue.GetConsumingEnumerable())
            {
                try
                {
                    writer.WriteLine(entry);
                    writer.Flush();
                }
                catch (IOException)
                {
                    Thread.Sleep(100); // chwilowy brak przepustowości
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _logQueue.CompleteAdding();
            _loggingThread.Join();
        }
    }
}
