using System;
using System.Threading;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace PresentationTest
{
    // Atrybut do uruchamiania testów w trybie STA (wymagane dla WPF)
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class STATestMethodAttributeBall : TestMethodAttribute
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
    public class BallModelTests
    {
        [STATestMethod] // Zmieniono z TestMethod
        public void TestBallModelProperties()
        {
            float initialX = 5f;
            float initialY = 10f;
            float radius = 15f;
            Guid id = Guid.NewGuid();
            string color = "FF0000";

            BallModel ball = new BallModel(initialX, initialY, radius, id, color);

            Assert.AreEqual(initialX, ball.X, "X coordinate mismatch.");
            Assert.AreEqual(initialY, ball.Y, "Y coordinate mismatch.");
            Assert.AreEqual(radius, ball.Radius, "Radius mismatch.");
            Assert.AreEqual(id, ball.Id, "Id mismatch.");
            Assert.AreEqual(color, ball.Color, "Color mismatch.");
        }

        [STATestMethod] // Zmieniono z TestMethod
        public void TestBallShapeCreation()
        {
            float radius = 10f;
            Guid id = Guid.NewGuid();

            BallModel ball = new BallModel(0f, 0f, radius, id);

            Assert.IsNotNull(ball.Shape, "Shape is null.");
            Assert.AreEqual(radius * 2, ball.Shape.Width, "Shape width mismatch.");
            Assert.AreEqual(radius * 2, ball.Shape.Height, "Shape height mismatch.");
            Assert.AreEqual(Brushes.Black, ball.Shape.Stroke, "Shape stroke color mismatch.");
        }

        [STATestMethod] // Zmieniono z TestMethod
        public void TestPropertyChangedEvent()
        {
            bool eventFired = false;
            BallModel ball = new BallModel(0f, 0f, 10f, Guid.NewGuid());

            ball.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(BallModel.X))
                    eventFired = true;
            };

            ball.X = 20f;

            Assert.IsTrue(eventFired, "PropertyChanged event was not fired for X.");
        }
    }
}
