using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using Data;
using Logic;

namespace LogicTests
{
    [TestClass]
    public class GameLogicTests
    {
        private GameLogic logic;
        private Table table;

        [TestInitialize]
        public void Setup()
        {
            table = new Table(500, 500);
            logic = new GameLogic(table);
        }

        [TestMethod]
        public void CreateBalls_ShouldAddCorrectNumber_WhenSpaceIsAvailable()
        {
            bool result = logic.CreateBalls(5);

            Assert.IsTrue(result);
            Assert.AreEqual(5, logic.getCountBall());
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
            IBall b2 = new Ball(110, 100, 25, 0, 0); // Zderza się z b1

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
            IBall ball = new Ball(10, 10, 10, -5, -5); // Powinien się odbić od lewej/górnej krawędzi
            logic.AddBallCheck(ball);

            logic.Move(null, EventArgs.Empty);

            Assert.AreEqual(10, ball.x); // zostaje na granicy
            Assert.AreEqual(10, ball.y);
            Assert.AreEqual(5, ball.vx); // odbicie
            Assert.AreEqual(5, ball.vy);
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
    }
}
