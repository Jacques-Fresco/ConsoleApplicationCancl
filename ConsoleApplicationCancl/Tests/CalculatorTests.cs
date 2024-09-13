using ConsoleApplicationCancl.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Tests
{
    [TestClass]
    public class CalculatorServiceTests
    {
        [TestInitialize]
        public void TestSetup()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        }

        [TestMethod]
        [DataRow("3 + 4", 7)]
        [DataRow("10 - -2", 12)]
        [DataRow("-6 * 7", -42)]
        [DataRow("-8 / 2", -4)]
        [DataRow("(3 + 4) * 2", 14)]
        [DataRow("10 / (2 + 3)", 2)]
        [DataRow("(-3 + 5) * 2", 4)]
        [DataRow("-(3 + 4)", -7)]
        [DataRow("10 - (2 * 3)", 4)]
        [DataRow("5 + (3 - 2)", 6)]
        [DataRow("2 * (3 + 4) - 5", 9)]
        [DataRow("2 + 3 * 4 - 5", 9)]
        [DataRow("2 + 3 * -4", -10)]
        [DataRow("2 + -3", -1)]
        [DataRow("3 * -2 + 4", -2)]
        public void Evaluate_ShouldReturnCorrectResult(string expression, double expected)
        {
            // Arrange
            var calculatorService = new CalculatorService();

            // Act
            var result = calculatorService.Evaluate(expression);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow("3.5 + 4.2", 7.7)]
        [DataRow("10.5 - 2.2", 8.3)]
        [DataRow("6.1 * 3.0", 18.3)]
        [DataRow("12.6 / 3.0", 4.2)]
        [DataRow("(3.1 + 4.2) * 2.0", 14.6)]
        [DataRow("10.0 / (2.5 + 2.5)", 2.0)]
        [DataRow("(-3.3 + 5.5) * 2.0", 4.4)]
        [DataRow("-(3.7 + 4.3)", -8.0)]
        [DataRow("10.0 - (2.2 * 3.0)", 3.4)]
        [DataRow("5.5 + (3.3 - 2.2)", 6.6)]
        [DataRow("2.0 * (3.0 + 4.5) - 5.5", 9.5)]
        public void Evaluate_ShouldReturnCorrectResult_WithDecimals(string expression, double expected)
        {
            // Arrange
            var calculatorService = new CalculatorService();

            // Act
            var result = calculatorService.Evaluate(expression);

            // Assert
            Assert.AreEqual(expected, result, 0.00001); // Указываем допустимую погрешность для вещественных чисел
        }

        [TestMethod]
        public void Evaluate_DivideByZero_ShouldThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService();

            // Act & Assert
            Assert.ThrowsException<DivideByZeroException>(() => calculatorService.Evaluate("10 / 0"));
        }

        [TestMethod]
        public void Evaluate_UnsupportedOperation_ShouldThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService();

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => calculatorService.Evaluate("10 % 2"));
        }

        [TestMethod]
        public void Evaluate_InvalidExpression_ShouldThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService();

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => calculatorService.Evaluate("10 +"));
            Assert.ThrowsException<InvalidOperationException>(() => calculatorService.Evaluate("+ 10"));
            Assert.ThrowsException<InvalidOperationException>(() => calculatorService.Evaluate("10 + (2"));
            Assert.ThrowsException<InvalidOperationException>(() => calculatorService.Evaluate("(10 + 5))"));
            Assert.ThrowsException<InvalidOperationException>(() => calculatorService.Evaluate("10 + 2 ) ("));
        }

        [TestMethod]
        public void Evaluate_EmptyExpression_ShouldThrowException()
        {
            // Arrange
            var calculatorService = new CalculatorService();

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => calculatorService.Evaluate(""));
        }

        [TestMethod]
        public void Evaluate_SingleNumber_ShouldReturnNumber()
        {
            // Arrange
            var calculatorService = new CalculatorService();

            // Act
            var result = calculatorService.Evaluate("42");

            // Assert
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void Evaluate_SingleDecimalNumber_ShouldReturnNumber()
        {
            // Arrange
            var calculatorService = new CalculatorService();

            // Act
            var result = calculatorService.Evaluate("42.5");

            // Assert
            Assert.AreEqual(42.5, result);
        }
    }
}
