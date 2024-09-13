using ConsoleApplicationCancl.Interfaces;
using System.Text;

namespace ConsoleApplicationCancl.Services
{
    public class TokenizerService
    {
        public List<string> Tokenize(string expression, IBaseCalculatorService calculatorService)
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

                    expectUnary = (c == '(' || calculatorService.IsOperator(c.ToString()));
                }
            }

            if (number.Length > 0)
            {
                tokens.Add(number.ToString());
            }

            return tokens;
        }
    }
}
