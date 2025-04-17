using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Data;

namespace Tester
{
    [TestClass]
    public class BallUnitTests
    {
        [TestMethod]
        public void Ball_Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            float x = 10, y = 20, r = 5, vx = 1, vy = -1;

            // Act
            var ball = new Ball(x, y, r, vx, vy);

            // Assert
            Assert.AreEqual(x, ball.x);
            Assert.AreEqual(y, ball.y);
            Assert.AreEqual(r, ball.r);
            Assert.AreEqual(vx, ball.vx);
            Assert.AreEqual(vy, ball.vy);
            Assert.IsFalse(string.IsNullOrWhiteSpace(ball.color));
            Assert.AreEqual(6, ball.color.Length);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Ball_SetNegativeX_ShouldThrowException()
        {
            var ball = new Ball(0, 0, 1, 0, 0);
            ball.x = -10;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Ball_SetNegativeY_ShouldThrowException()
        {
            var ball = new Ball(0, 0, 1, 0, 0);
            ball.y = -10;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Ball_SetNegativeRadius_ShouldThrowException()
        {
            var ball = new Ball(0, 0, 1, 0, 0);
            ball.r = -1;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ball_SetInvalidColor_ShouldThrowException_TooShort()
        {
            var ball = new Ball(0, 0, 1, 0, 0);
            ball.color = "FFF"; // za krótki
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ball_SetInvalidColor_ShouldThrowException_InvalidCharacters()
        {
            var ball = new Ball(0, 0, 1, 0, 0);
            ball.color = "GGGGGG"; // niepoprawne znaki
        }

        [TestMethod]
        public void Ball_SetValidColor_ShouldConvertToUpperCase()
        {
            var ball = new Ball(0, 0, 1, 0, 0);
            ball.color = "a1b2c3";
            Assert.AreEqual("A1B2C3", ball.color);
        }

        [TestMethod]
        public void Ball_Id_ShouldBeUnique()
        {
            var ball1 = new Ball(0, 0, 1, 0, 0);
            var ball2 = new Ball(0, 0, 1, 0, 0);
            Assert.AreNotEqual(ball1.Id_ball, ball2.Id_ball);
        }
    }

}
