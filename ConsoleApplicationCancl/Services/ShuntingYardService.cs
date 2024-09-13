using ConsoleApplicationCancl.Interfaces;
using System.Globalization;

namespace ConsoleApplicationCancl.Services
{
    public class ShuntingYardService
    {
        public List<string> ConvertToRPN(List<string> tokens, IBaseCalculatorService calculatorService)
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
                else if (calculatorService.IsOperator(token) || token == "-u")
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

        private int Precedence(string operation) => operation switch
        {
            "+" or "-" => 1,
            "*" or "/" => 2,
            "-u" => 3,
            _ => 0
        };
    }
}
