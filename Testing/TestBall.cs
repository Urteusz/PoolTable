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
            // Arrange & Act
            Ball ball = new Ball(10, 20, 5, 1, -1);

            // Assert
            Assert.That(ball.x, Is.EqualTo(10));
            Assert.That(ball.y, Is.EqualTo(20));
            Assert.That(ball.r, Is.EqualTo(5));
            Assert.That(ball.vx, Is.EqualTo(1));
            Assert.That(ball.vy, Is.EqualTo(-1));
        }

        [Test]
        public void Ball_SetsAndGetsColorCorrectly()
        {
            // Arrange
            Ball ball = new Ball(0, 0, 1, 0, 0);

            // Act
            ball.color = "FFAABB";

            // Assert
            Assert.That(ball.color, Is.EqualTo("FFAABB"));
        }

        [Test]
        public void Ball_ThrowsExceptionForInvalidColor_Length()
        {
            // Arrange
            Ball ball = new Ball(0, 0, 1, 0, 0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ball.color = "FFAA"); // Za krótki
            Assert.Throws<ArgumentException>(() => ball.color = "FFAABBC"); // Za długi
        }

        [Test]
        public void Ball_ThrowsExceptionForInvalidColor_Characters()
        {
            // Arrange
            Ball ball = new Ball(0, 0, 1, 0, 0);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => ball.color = "ZZZZZZ"); // Niedozwolone znaki
            Assert.Throws<ArgumentException>(() => ball.color = "12345G"); // G nie jest dozwolone
        }

        [Test]
        public void Ball_LosujKolor_GeneratesValidHexColor()
        {
            // Arrange
            Ball ball = new Ball(0, 0, 1, 0, 0);

            // Act
            ball.LosujKolor();

            // Assert
            Assert.That(ball.color, Does.Match("^[0-9A-F]{6}$"));
        }
    }
}
