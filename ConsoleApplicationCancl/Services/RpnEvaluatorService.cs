using ConsoleApplicationCancl.Interfaces;
using System.Globalization;

namespace ConsoleApplicationCancl.Services
{
    public class RpnEvaluatorService
    {
        private readonly List<IOperation> _operations;

        public RpnEvaluatorService(List<IOperation> operations)
        {
            _operations = operations;
        }

        public double EvaluateRPN(List<string> tokens)
        {
            var stack = new Stack<double>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    stack.Push(number);
                }
                else if (token == "-u")
                {
                    if (stack.Count < 1)
                    {
                        throw new InvalidOperationException("Недостаточно операндов для унарного минуса.");
                    }
                    var operand = stack.Pop();
                    stack.Push(-operand);
                }
                else if (_operations.Any(o => o.CanExecute(token)))
                {
                    if (stack.Count < 2)
                    {
                        throw new InvalidOperationException("Недостаточно операндов для операции.");
                    }

                    var rightOperand = stack.Pop();
                    var leftOperand = stack.Pop();
                    var operation = _operations.FirstOrDefault(o => o.CanExecute(token));

                    if (operation == null)
                    {
                        throw new InvalidOperationException($"Операция '{token}' не поддерживается.");
                    }

                    stack.Push(operation.Calculate(leftOperand, rightOperand));
                }
            }

            if (stack.Count != 1)
            {
                throw new InvalidOperationException("Неверное выражение. Проверьте ввод.");
            }

            return stack.Pop();
        }
    }
}
