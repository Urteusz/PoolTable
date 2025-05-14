using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Data;

namespace DataTests
{
    [TestClass]
    public class BallTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            float x = 10, y = 20, r = 5, vx = 1, vy = 2;

            // Act
            Ball ball = new Ball(x, y, r, vx, vy);

            // Assert
            Assert.AreEqual(x, ball.x);
            Assert.AreEqual(y, ball.y);
            Assert.AreEqual(r, ball.r);
            Assert.AreEqual(vx, ball.vx);
            Assert.AreEqual(vy, ball.vy);
            Assert.IsNotNull(ball.color);
            Assert.AreEqual(6, ball.color.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SettingNegativeX_ShouldThrow()
        {
            Ball ball = new Ball(1, 1, 1, 0, 0);
            ball.x = -1;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SettingNegativeY_ShouldThrow()
        {
            Ball ball = new Ball(1, 1, 1, 0, 0);
            ball.y = -10;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SettingZeroRadius_ShouldThrow()
        {
            Ball ball = new Ball(1, 1, 1, 0, 0);
            ball.r = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SettingTooShortColor_ShouldThrow()
        {
            Ball ball = new Ball(1, 1, 1, 0, 0);
            ball.color = "12345";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SettingTooLongColor_ShouldThrow()
        {
            Ball ball = new Ball(1, 1, 1, 0, 0);
            ball.color = "1234567";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SettingInvalidHexCharacterColor_ShouldThrow()
        {
            Ball ball = new Ball(1, 1, 1, 0, 0);
            ball.color = "12GHIJ";
        }

        [TestMethod]
        public void SetPosition_ShouldUpdateCoordinates()
        {
            Ball ball = new Ball(10, 20, 5, 2, 3);
            ball.SetPosition();

            Assert.AreEqual(12, ball.x);
            Assert.AreEqual(23, ball.y);
        }

        [TestMethod]
        public void LosujKolor_ShouldGenerateValidHexColor()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);
            ball.LosujKolor();
            string color = ball.color;

            StringAssert.Matches(color, new System.Text.RegularExpressions.Regex("^[A-F0-9]{6}$"));
        }

        [TestMethod]
        public void CreateBall_ShouldReturnNewBallWithGivenProperties()
        {
            Ball original = new Ball(0, 0, 1, 0, 0);
            Ball created = original.createBall(5, 10, 2, 1, -1);

            Assert.AreEqual(5, created.x);
            Assert.AreEqual(10, created.y);
            Assert.AreEqual(2, created.r);
            Assert.AreEqual(1, created.vx);
            Assert.AreEqual(-1, created.vy);
        }

        [TestMethod]
        public void Id_ShouldBeUnique()
        {
            Ball ball1 = new Ball(1, 1, 1, 1, 1);
            Ball ball2 = new Ball(1, 1, 1, 1, 1);

            Assert.AreNotEqual(ball1.Id_ball, ball2.Id_ball);
        }
    }
}
