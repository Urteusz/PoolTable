using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace PresentationTest
{
 
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class STATestMethodAttributeTable : TestMethodAttribute
    {
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            TestResult[] result = null;

            var thread = new Thread(() =>
            {
                result = base.Execute(testMethod);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            return result;
        }
    }

    [TestClass]
    public class TableModelTests
    {
        [STATestMethod] // Zmieniono z TestMethod
        public void TestTableModelCreation()
        {
            // Arrange
            int width = 500;
            int height = 300;

            // Act
            TableModel table = new TableModel(width, height);

            // Assert
            Assert.AreEqual(width, table.Width, "Table width mismatch.");
            Assert.AreEqual(height, table.Height, "Table height mismatch.");
            Assert.IsNotNull(table.canvas, "Canvas is null.");
            Assert.IsNotNull(table.TableBorder, "Table border is null.");
            Assert.AreEqual(Brushes.Black, table.TableBorder.Stroke, "Table border stroke color mismatch.");
            Assert.AreEqual(5, table.TableBorder.StrokeThickness, "Table border stroke thickness mismatch.");
            Assert.AreEqual(Brushes.LightGreen, table.canvas.Background, "Canvas background color mismatch.");
        }

        [STATestMethod] // Zmieniono z TestMethod
        public void TestAddBall()
        {
            // Arrange
            TableModel table = new TableModel(500, 300);
            Guid ballId = Guid.NewGuid();
            BallModel ball = new BallModel(100f, 150f, 10f, ballId);

            // Act
            table.AddBall(ball);

            // Assert
            Assert.AreEqual(1, table.Balls.Count, "Ball was not added to the collection.");
            Assert.IsTrue(table.Balls.Any(b => b.Id == ballId), "Ball is not present in the Balls collection.");
            Assert.IsNotNull(ball.Shape, "Ball shape is null.");

            // Test if the ball shape is added to the canvas
            Assert.IsTrue(table.canvas.Children.Contains(ball.Shape), "Ball shape was not added to the canvas.");

            // Test binding for position (Left and Top should be set to X and Y values)
            Assert.AreEqual(100f, (float)Canvas.GetLeft(ball.Shape), "Ball X position mismatch.");
            Assert.AreEqual(150f, (float)Canvas.GetTop(ball.Shape), "Ball Y position mismatch.");
        }

        [STATestMethod] // Zmieniono z TestMethod
        public void TestBallBindingToCanvasPosition()
        {
            // Arrange
            TableModel table = new TableModel(500, 300);
            BallModel ball = new BallModel(50f, 50f, 10f, Guid.NewGuid());

            // Act
            table.AddBall(ball);
            ball.X = 200f; // Change X position
            ball.Y = 100f; // Change Y position

            // Assert
            Assert.AreEqual(200f, (float)Canvas.GetLeft(ball.Shape), "Ball X position did not update correctly.");
            Assert.AreEqual(100f, (float)Canvas.GetTop(ball.Shape), "Ball Y position did not update correctly.");
        }
    }
}

