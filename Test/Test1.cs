using Data;
using System;

namespace Test
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void Ball_CreatesWithCorrectValues()
        {
            Ball ball = new Ball(10, 20, 5, 1, -1);

            Assert.AreEqual(10, ball.x);
            Assert.AreEqual(20, ball.y);
            Assert.AreEqual(5, ball.r);
            Assert.AreEqual(1, ball.vx);
            Assert.AreEqual(-1, ball.vy);
        }

        [TestMethod]
        public void Ball_ThrowsExceptionForNegativeX()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Ball(-10, 20, 5, 1, -1));
        }

        [TestMethod]
        public void Ball_ThrowsExceptionForNegativeY()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Ball(10, -20, 5, 1, -1));
        }

        [TestMethod]
        public void Ball_ThrowsExceptionForNegativeRadius()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Ball(10, 20, -5, 1, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Ball(10, 20, 0, 1, -1));
        }

        [TestMethod]
        public void Ball_ThrowsExceptionWhenSettingNegativeX()
        {
            Ball ball = new Ball(10, 10, 5, 1, 1);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ball.x = -1);
        }

        [TestMethod]
        public void Ball_ThrowsExceptionWhenSettingNegativeY()
        {
            Ball ball = new Ball(10, 10, 5, 1, 1);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ball.y = -1);
        }

        [TestMethod]
        public void Ball_ThrowsExceptionWhenSettingNegativeRadius()
        {
            Ball ball = new Ball(10, 10, 5, 1, 1);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ball.r = -1);
        }

        [TestMethod]
        public void Ball_SetsAndGetsColorCorrectly()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);
            ball.color = "FFAABB";

            Assert.AreEqual("FFAABB", ball.color);
        }

        [TestMethod]
        public void Ball_ThrowsExceptionForInvalidColor_Length()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);

            Assert.ThrowsException<ArgumentException>(() => ball.color = "FFAA"); // Za krótki
            Assert.ThrowsException<ArgumentException>(() => ball.color = "FFAABBC"); // Za długi
        }

        [TestMethod]
        public void Ball_ThrowsExceptionForInvalidColor_Characters()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);

            Assert.ThrowsException<ArgumentException>(() => ball.color = "ZZZZZZ"); // Niedozwolone znaki
            Assert.ThrowsException<ArgumentException>(() => ball.color = "12345G"); // G nie jest dozwolone
        }

        [TestMethod]
        public void Ball_LosujKolor_GeneratesValidHexColor()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);
            ball.LosujKolor();

            StringAssert.Matches(ball.color, new System.Text.RegularExpressions.Regex("^[0-9A-F]{6}$"));
        }
    }
}
