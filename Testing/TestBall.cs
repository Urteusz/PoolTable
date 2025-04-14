using NUnit.Framework;
using System;
using Data;

namespace Testing
{
    public class BallTests
    {
        [Test]
        public void Ball_CreatesWithCorrectValues()
        {
            Ball ball = new Ball(10, 20, 5, 1, -1);


            Assert.That(ball.x, Is.EqualTo(10));
            Assert.That(ball.y, Is.EqualTo(20));
            Assert.That(ball.r, Is.EqualTo(5));
            Assert.That(ball.vx, Is.EqualTo(1));
            Assert.That(ball.vy, Is.EqualTo(-1));
        }

        [Test]
        public void Ball_ThrowsExceptionForNegativeX()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ball(-10, 20, 5, 1, -1));
        }

        [Test]
        public void Ball_ThrowsExceptionForNegativeY()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ball(10, -20, 5, 1, -1));
        }

        [Test]
        public void Ball_ThrowsExceptionForNegativeRadius()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ball(10, 20, -5, 1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Ball(10, 20, 0, 1, -1));
        }

        [Test]
        public void Ball_ThrowsExceptionWhenSettingNegativeX()
        {
            Ball ball = new Ball(10, 10, 5, 1, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => ball.x = -1);
        }

        [Test]
        public void Ball_ThrowsExceptionWhenSettingNegativeY()
        {
            Ball ball = new Ball(10, 10, 5, 1, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => ball.y = -1);
        }

        [Test]
        public void Ball_ThrowsExceptionWhenSettingNegativeRadius()
        {
            Ball ball = new Ball(10, 10, 5, 1, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => ball.r = -1);
        }

        [Test]
        public void Ball_SetsAndGetsColorCorrectly()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);
            ball.color = "FFAABB";

            Assert.That(ball.color, Is.EqualTo("FFAABB"));
        }

        [Test]
        public void Ball_ThrowsExceptionForInvalidColor_Length()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);

            Assert.Throws<ArgumentException>(() => ball.color = "FFAA"); // Za krótki
            Assert.Throws<ArgumentException>(() => ball.color = "FFAABBC"); // Za długi
        }

        [Test]
        public void Ball_ThrowsExceptionForInvalidColor_Characters()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);

            Assert.Throws<ArgumentException>(() => ball.color = "ZZZZZZ"); // Niedozwolone znaki
            Assert.Throws<ArgumentException>(() => ball.color = "12345G"); // G nie jest dozwolone
        }

        [Test]
        public void Ball_LosujKolor_GeneratesValidHexColor()
        {
            Ball ball = new Ball(0, 0, 1, 0, 0);
            ball.LosujKolor();

            Assert.That(ball.color, Does.Match("^[0-9A-F]{6}$"));
        }
    }
}
