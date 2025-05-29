using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.IO;
using System.Threading;
using System.Linq;
using System;

namespace LoggerTests
{
    [TestClass]
    public class LoggerUnitTests
    {
        private string logFilePath;

        [TestInitialize]
        public void Setup()
        {
            logFilePath = Path.Combine(Path.GetTempPath(), $"test_log_{Guid.NewGuid()}.txt");
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }

        private IBall CreateFakeBall(int idInt)
        {
            return new Ball(idInt, 10.5f, 20.5f, 1.0f, -1.0f, 5.0f);
        }

        [TestMethod]
        public void Logger_LogsBallCreation()
        {
            // Arrange
            var logger = new Logger(logFilePath);
            var ball = CreateFakeBall(42); // ID = 42

            // Act
            logger.LogBallCreate(ball);
            logger.Stop();

            // Assert
            var lines = File.ReadAllLines(logFilePath);
            Assert.IsTrue(lines.Any(line => line.Contains("[CREATE]") && line.Contains(ball.Id_ball.ToString())));
        }

        [TestMethod]
        public void Logger_LogsWallCollision()
        {
            // Arrange
            var logger = new Logger(logFilePath);
            var ball = CreateFakeBall(99); // ID = 99

            // Act
            logger.LogBallColisionWall(ball, "LEFT");
            logger.Stop();

            // Assert
            var lines = File.ReadAllLines(logFilePath);
            Assert.IsTrue(lines.Any(line => line.Contains("[COLLISION_WALL_LEFT]") && line.Contains(ball.Id_ball.ToString())));
        }

        [TestMethod]
        public void Logger_LogsBallCollision()
        {
            // Arrange
            var logger = new Logger(logFilePath);
            var ball1 = CreateFakeBall(1);
            var ball2 = CreateFakeBall(2);

            // Act
            logger.LogBallColision(ball1, ball2);
            logger.Stop();

            // Assert
            var lines = File.ReadAllLines(logFilePath);
            string expected = $"ID1={ball1.Id_ball};ID2={ball2.Id_ball}";
            Assert.IsTrue(lines.Any(line => line.Contains("[COLLISION]") && line.Contains(expected)));
        }
    }
}
