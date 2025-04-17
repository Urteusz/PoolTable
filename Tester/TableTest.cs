using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var table = new Table(width, height);

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
            new Table(-10, 50);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Table_Constructor_ShouldThrow_WhenHeightIsNegative()
        {
            new Table(100, -50);
        }

        [TestMethod]
        public void Table_SetTableSize_ShouldUpdateSize()
        {
            var table = new Table(10, 10);
            table.SetTableSize(200, 300);

            Assert.AreEqual(200, table.width);
            Assert.AreEqual(300, table.height);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Table_SetTableSize_ShouldThrow_WhenNegative()
        {
            var table = new Table(10, 10);
            table.SetTableSize(-5, 10);
        }

        [TestMethod]
        public void Table_AddBall_ShouldAdd_WhenInsideBoundsAndUnique()
        {
            var table = new Table(100, 100);
            var ball = new Ball(50, 50, 10, 0, 0);

            bool result = table.AddBall(ball);

            Assert.IsTrue(result);
            Assert.AreEqual(1, table.CountBalls());
            Assert.AreEqual(ball, table.GetBall(ball));
        }

        [TestMethod]
        public void Table_AddBall_ShouldReject_WhenBallOutOfBounds()
        {
            var table = new Table(100, 100);
            var ball = new Ball(5, 5, 10, 0, 0); // poza lewym górnym rogiem

            bool result = table.AddBall(ball);

            Assert.IsFalse(result);
            Assert.AreEqual(0, table.CountBalls());
        }

        [TestMethod]
        public void Table_AddBall_ShouldReject_Duplicate()
        {
            var table = new Table(100, 100);
            var ball = new Ball(50, 50, 10, 0, 0);

            bool first = table.AddBall(ball);
            bool second = table.AddBall(ball);

            Assert.IsTrue(first);
            Assert.IsFalse(second);
            Assert.AreEqual(1, table.CountBalls());
        }

        [TestMethod]
        public void Table_RemoveBall_ShouldRemove_WhenExists()
        {
            var table = new Table(100, 100);
            var ball = new Ball(50, 50, 10, 0, 0);
            table.AddBall(ball);

            bool removed = table.RemoveBall(ball);

            Assert.IsTrue(removed);
            Assert.AreEqual(0, table.CountBalls());
        }

        [TestMethod]
        public void Table_RemoveBall_ShouldFail_WhenNotExists()
        {
            var table = new Table(100, 100);
            var ball = new Ball(50, 50, 10, 0, 0);

            bool removed = table.RemoveBall(ball);

            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void Table_GetBall_ShouldReturnCorrectBall()
        {
            var table = new Table(100, 100);
            var ball = new Ball(50, 50, 10, 0, 0);
            table.AddBall(ball);

            var result = table.GetBall(ball);

            Assert.IsNotNull(result);
            Assert.AreEqual(ball.Id_ball, result.Id_ball);
        }

        [TestMethod]
        public void Table_GetBall_ShouldReturnNull_WhenBallNotFound()
        {
            var table = new Table(100, 100);
            var ball = new Ball(50, 50, 10, 0, 0);

            var result = table.GetBall(ball);

            Assert.IsNull(result);
        }
    }
}
