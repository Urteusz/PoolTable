using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;

namespace Tester
{
    [TestClass]
    public class TableUnitTests
    {
        [TestMethod]
        public void Table_Constructor_ShouldInitializeCorrectly()
        {
            // Arrange
            int width = 100;
            int height = 50;

            // Act
            ITable table = new Table(width, height); // Teraz używamy ITable

            // Assert
            Assert.AreEqual(width, table.width);
            Assert.AreEqual(height, table.height);
            Assert.IsNotNull(table.balls);
            Assert.AreEqual(0, table.CountBalls());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Table_Constructor_ShouldThrow_WhenWidthIsNegative()
        {
            ITable table = new Table(-10, 50); // Teraz używamy ITable
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Table_Constructor_ShouldThrow_WhenHeightIsNegative()
        {
            ITable table = new Table(100, -50); // Teraz używamy ITable
        }

        [TestMethod]
        public void Table_AddBall_ShouldAdd_WhenInsideBoundsAndUnique()
        {
            ITable table = new Table(100, 100); // Teraz używamy ITable
            IBall ball = new Ball(50, 50, 10, 0, 0); // Teraz używamy IBall

            bool result = table.AddBall(ball);

            Assert.IsTrue(result);
            Assert.AreEqual(1, table.CountBalls());
            Assert.AreEqual(ball, table.GetBall(ball));
        }

        [TestMethod]
        public void Table_AddBall_ShouldReject_WhenBallOutOfBounds()
        {
            ITable table = new Table(100, 100); // Teraz używamy ITable
            IBall ball = new Ball(5, 5, 10, 0, 0); // poza lewym górnym rogiem

            bool result = table.AddBall(ball);

            Assert.IsFalse(result);
            Assert.AreEqual(0, table.CountBalls());
        }

        [TestMethod]
        public void Table_AddBall_ShouldReject_Duplicate()
        {
            ITable table = new Table(100, 100); // Teraz używamy ITable
            IBall ball = new Ball(50, 50, 10, 0, 0); // Teraz używamy IBall

            bool first = table.AddBall(ball);
            bool second = table.AddBall(ball);

            Assert.IsTrue(first);
            Assert.IsFalse(second);
            Assert.AreEqual(1, table.CountBalls());
        }

        [TestMethod]
        public void Table_RemoveBall_ShouldRemove_WhenExists()
        {
            ITable table = new Table(100, 100); // Teraz używamy ITable
            IBall ball = new Ball(50, 50, 10, 0, 0); // Teraz używamy IBall
            table.AddBall(ball);

            bool removed = table.RemoveBall(ball);

            Assert.IsTrue(removed);
            Assert.AreEqual(0, table.CountBalls());
        }

        [TestMethod]
        public void Table_RemoveBall_ShouldFail_WhenNotExists()
        {
            ITable table = new Table(100, 100); // Teraz używamy ITable
            IBall ball = new Ball(50, 50, 10, 0, 0); // Teraz używamy IBall

            bool removed = table.RemoveBall(ball);

            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void Table_GetBall_ShouldReturnCorrectBall()
        {
            ITable table = new Table(100, 100); // Teraz używamy ITable
            IBall ball = new Ball(50, 50, 10, 0, 0); // Teraz używamy IBall
            table.AddBall(ball);

            var result = table.GetBall(ball);

            Assert.IsNotNull(result);
            Assert.AreEqual(ball.Id_ball, result.Id_ball);
        }

        [TestMethod]
        public void Table_GetBall_ShouldReturnNull_WhenBallNotFound()
        {
            ITable table = new Table(100, 100); // Teraz używamy ITable
            IBall ball = new Ball(50, 50, 10, 0, 0); // Teraz używamy IBall

            var result = table.GetBall(ball);

            Assert.IsNull(result);
        }
    }
}
