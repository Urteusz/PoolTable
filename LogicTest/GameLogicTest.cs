using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using Data;
using Logic;
using Moq;

namespace LogicTests
{
    [TestClass]
    public class GameLogicTests
    {
        private GameLogic logic;
        private Table table;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void Setup()
        {
            table = new Table(500, 500);
            mockLogger = new Mock<ILogger>();
            logic = new GameLogic(table, mockLogger.Object);
        }

        [TestMethod]
        public void CreateBalls_ShouldAddCorrectNumber_WhenSpaceIsAvailable()
        {
            bool result = logic.CreateBalls(5);

            Assert.IsTrue(result);
            Assert.AreEqual(5, logic.getCountBall());
            mockLogger.Verify(l => l.LogBallCreate(It.IsAny<IBall>()), Times.Exactly(5));
        }

        [TestMethod]
        public void CreateBalls_ShouldReturnFalse_WhenCountIsZeroOrNegative()
        {
            bool result = logic.CreateBalls(0);
            Assert.IsFalse(result);

            result = logic.CreateBalls(-3);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddBallCheck_ShouldReturnTrue_WhenNoCollisionAndWithinBounds()
        {
            IBall ball = new Ball(100, 100, 10, 0, 0);
            bool result = logic.AddBallCheck(ball);

            Assert.IsTrue(result);
            Assert.AreEqual(1, logic.getCountBall());
        }

        [TestMethod]
        public void AddBallCheck_ShouldReturnFalse_WhenBallCollides()
        {
            IBall b1 = new Ball(100, 100, 25, 0, 0);
            IBall b2 = new Ball(110, 100, 25, 0, 0);

            logic.AddBallCheck(b1);
            bool result = logic.AddBallCheck(b2);

            Assert.IsFalse(result);
            Assert.AreEqual(1, logic.getCountBall());
        }

        [TestMethod]
        public void getBalls_ShouldReturnAllBalls()
        {
            logic.CreateBalls(3);

            List<IBall> balls = logic.getBalls();
            Assert.AreEqual(3, balls.Count);
        }

        [TestMethod]
        public void getBall_ShouldReturnBallById()
        {
            IBall ball = new Ball(100, 100, 10, 0, 0);
            logic.AddBallCheck(ball);

            IBall result = logic.getBall(ball.Id_ball);
            Assert.IsNotNull(result);
            Assert.AreEqual(ball.Id_ball, result.Id_ball);
        }

        [TestMethod]
        public void getBall_ShouldReturnNull_WhenNotFound()
        {
            IBall result = logic.getBall(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Move_ShouldUpdateBallPositions()
        {
            IBall ball = new Ball(100, 100, 10, 5, 5);
            logic.AddBallCheck(ball);

            logic.Move(null, EventArgs.Empty);

            Assert.AreEqual(105, ball.x);
            Assert.AreEqual(105, ball.y);
        }

        [TestMethod]
        public void Move_ShouldBounceBallFromWall()
        {
            IBall ball = new Ball(10, 10, 10, -5, -5); // zderzenie z lewą/górną
            logic.AddBallCheck(ball);

            logic.Move(null, EventArgs.Empty);

            Assert.AreEqual(10, ball.x);
            Assert.AreEqual(10, ball.y);
            Assert.AreEqual(5, ball.vx);
            Assert.AreEqual(5, ball.vy);

            mockLogger.Verify(l => l.LogBallColisionWall(ball, "Left"), Times.Once());
            mockLogger.Verify(l => l.LogBallColisionWall(ball, "Top"), Times.Once());
        }

        [TestMethod]
        public void CheckCollision_ShouldReturnTrue_WhenCollisionExists()
        {
            IBall b1 = new Ball(100, 100, 30, 0, 0);
            IBall b2 = new Ball(120, 100, 30, 0, 0);

            logic.AddBallCheck(b1);
            bool collides = logic.CheckCollision(b2);

            Assert.IsTrue(collides);
        }

        [TestMethod]
        public void CheckCollision_ShouldReturnFalse_WhenNoCollision()
        {
            IBall b1 = new Ball(100, 100, 20, 0, 0);
            IBall b2 = new Ball(300, 300, 20, 0, 0);

            logic.AddBallCheck(b1);
            bool collides = logic.CheckCollision(b2);

            Assert.IsFalse(collides);
        }

        [TestMethod]
        public void getTable_ShouldReturnSameTableReference()
        {
            var result = logic.getTable();
            Assert.AreSame(table, result);
        }

        [TestMethod]
        public void Move_ShouldLogCollision_WhenBallsCollide()
        {
            IBall ball1 = new Ball(100, 100, 20, 5, 0);  
            IBall ball2 = new Ball(160, 100, 20, -5, 0); 
            logic.AddBallCheck(ball1);
            logic.AddBallCheck(ball2);

            for (int i = 0; i < 10; i++)
                logic.Move(null, EventArgs.Empty);

            mockLogger.Verify(l => l.LogBallColision(It.IsAny<IBall>(), It.IsAny<IBall>()), Times.AtLeastOnce());
        }

    }
}
