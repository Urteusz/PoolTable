using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            Table table = new Table(width, height);

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
            _ = new Table(-10, 50);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Table_Constructor_ShouldThrow_WhenHeightIsNegative()
        {
            _ = new Table(100, -50);
        }

        [TestMethod]
        public void Table_SetTableSize_ShouldUpdateSize()
        {
            Table table = new Table(10, 10);
            table.SetTableSize(200, 300);

            Assert.AreEqual(200, table.width);
            Assert.AreEqual(300, table.height);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Table_SetTableSize_ShouldThrow_WhenNegative()
        {
            Table table = new Table(10, 10);
            table.SetTableSize(-1, 10);
        }

        [TestMethod]
        public void Table_AddBall_ShouldAdd_WhenInsideBoundsAndUnique()
        {
            Table table = new Table(100, 100);
            IBall ball = new Ball(50, 50, 10, 0, 0);

            bool result = table.AddBall(ball);

            Assert.IsTrue(result);
            Assert.AreEqual(1, table.CountBalls());
            Assert.AreEqual(ball, table.GetBall(ball));
        }

        [TestMethod]
        public void Table_AddBall_ShouldReject_WhenBallOutOfBounds()
        {
            Table table = new Table(100, 100);
            IBall ball = new Ball(5, 5, 10, 0, 0); // overlapping border

            bool result = table.AddBall(ball);

            Assert.IsFalse(result);
            Assert.AreEqual(0, table.CountBalls());
        }

        [TestMethod]
        public void Table_AddBall_ShouldReject_Duplicate()
        {
            Table table = new Table(100, 100);
            IBall ball = new Ball(50, 50, 10, 0, 0);

            bool first = table.AddBall(ball);
            bool second = table.AddBall(ball);

            Assert.IsTrue(first);
            Assert.IsFalse(second);
            Assert.AreEqual(1, table.CountBalls());
        }

        [TestMethod]
        public void Table_RemoveBall_ShouldRemove_WhenExists()
        {
            Table table = new Table(100, 100);
            IBall ball = new Ball(50, 50, 10, 0, 0);
            table.AddBall(ball);

            bool removed = table.RemoveBall(ball);

            Assert.IsTrue(removed);
            Assert.AreEqual(0, table.CountBalls());
        }

        [TestMethod]
        public void Table_RemoveBall_ShouldFail_WhenNotExists()
        {
            Table table = new Table(100, 100);
            IBall ball = new Ball(50, 50, 10, 0, 0);

            bool removed = table.RemoveBall(ball);

            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void Table_GetBall_ShouldReturnCorrectBall()
        {
            Table table = new Table(100, 100);
            IBall ball = new Ball(50, 50, 10, 0, 0);
            table.AddBall(ball);

            var result = table.GetBall(ball);

            Assert.IsNotNull(result);
            Assert.AreEqual(ball.Id_ball, result.Id_ball);
        }

        [TestMethod]
        public void Table_GetBall_ShouldReturnNull_WhenBallNotFound()
        {
            Table table = new Table(100, 100);
            IBall ball = new Ball(50, 50, 10, 0, 0);

            var result = table.GetBall(ball);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Table_CreateTable_ShouldReturnNewTableWithGivenSize()
        {
            Table original = new Table(1, 1);
            Table newTable = original.createTable(200, 300);

            Assert.AreEqual(200, newTable.width);
            Assert.AreEqual(300, newTable.height);
            Assert.AreNotEqual(original.Id_table, newTable.Id_table);
        }
    }
}
