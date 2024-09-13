using ConsoleApplicationCancl.Interfaces;
using ConsoleApplicationCancl.Operations;
using System.Text;
using System.Globalization;

namespace ConsoleApplicationCancl.Services
{
    public class CalculatorService : BaseCalculatorService, ICalculatorService
    {
        private readonly List<IOperation> _operations;

        public CalculatorService() : base(new List<IOperation>
        {
            new Addition(),
            new Subtraction(),
            new Multiplication(),
            new Division()
        }) { }

        public double Evaluate(string expression)
        {
            var tokens = Tokenize(expression);
            var output = ShuntingYard(tokens);
            return EvaluateRPN(output);
        }

        private List<string> Tokenize(string expression)
        {
            var tokens = new List<string>();
            var number = new StringBuilder();
            bool expectUnary = true;

            foreach (char c in expression)
            {
                if (char.IsWhiteSpace(c)) continue;

                if (char.IsDigit(c) || (c == '.' && number.Length > 0 && !number.ToString().Contains('.')))
                {
                    number.Append(c);
                    expectUnary = false;
                }
                else
                {
                    if (number.Length > 0)
                    {
                        tokens.Add(number.ToString());
                        number.Clear();
                    }

                    if (c == '-' && expectUnary)
                    {
                        tokens.Add("-u");
                    }
                    else
                    {
                        tokens.Add(c.ToString());
                    }

                    expectUnary = (c == '(' || IsOperator(c.ToString()));
                }
            }

            if (number.Length > 0)
            {
                tokens.Add(number.ToString());
            }

            return tokens;
        }

        private List<string> ShuntingYard(List<string> tokens)
        {
            var output = new List<string>();
            var operators = new Stack<string>();
            int openBrackets = 0;

            foreach (var token in tokens)
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    output.Add(token);
                }
                else if (IsOperator(token) || token == "-u")
                {
                    while (operators.Count > 0 && Precedence(operators.Peek()) >= Precedence(token))
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                    openBrackets++;
                }
                else if (token == ")")
                {
                    openBrackets--;
                    while (operators.Count > 0 && operators.Peek() != "(")
                    {
                        output.Add(operators.Pop());
                    }
                    if (operators.Count == 0)
                    {
                        throw new InvalidOperationException("Несоответствие скобок.");
                    }
                    operators.Pop();
                }
            }

            if (openBrackets != 0)
            {
                throw new InvalidOperationException("Несоответствие скобок.");
            }

            while (operators.Count > 0)
            {
                var op = operators.Pop();
                if (op == "(" || op == ")")
                {
                    throw new InvalidOperationException("Несоответствие скобок.");
                }
                output.Add(op);
            }

            return output;
        }

        private double EvaluateRPN(List<string> tokens)
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
                else if (IsOperator(token))
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

        private bool IsOperator(string token) => _operations.Any(o => o.CanExecute(token));

        private int Precedence(string operation) => operation switch
        {
            "+" or "-" => 1,
            "*" or "/" => 2,
            "-u" => 3,
            _ => 0
        };
    }
}
