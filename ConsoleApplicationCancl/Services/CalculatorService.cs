using ConsoleApplicationCancl.Interfaces;
using ConsoleApplicationCancl.Operations;
using System.Collections.Generic;

namespace ConsoleApplicationCancl.Services
{
    public class CalculatorService : BaseCalculatorService, ICalculatorService
    {
        private readonly TokenizerService _tokenizerService;
        private readonly ShuntingYardService _shuntingYardService;
        private readonly RpnEvaluatorService _rpnEvaluatorService;

        public CalculatorService() : base(new List<IOperation>
        {
            new Addition(),
            new Subtraction(),
            new Multiplication(),
            new Division()
        })
        {
            _tokenizerService = new TokenizerService();
            _shuntingYardService = new ShuntingYardService();
            _rpnEvaluatorService = new RpnEvaluatorService(Operations);
        }

        public double Evaluate(string expression)
        {
            var tokens = _tokenizerService.Tokenize(expression, this);
            var output = _shuntingYardService.ConvertToRPN(tokens, this);
            return _rpnEvaluatorService.EvaluateRPN(output);
        }
    }
}
