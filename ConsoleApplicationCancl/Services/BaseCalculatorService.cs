using ConsoleApplicationCancl.Interfaces;

namespace ConsoleApplicationCancl.Services
{
    public abstract class BaseCalculatorService : IBaseCalculatorService
    {
        protected readonly List<IOperation> Operations;

        protected BaseCalculatorService(List<IOperation> operations)
        {
            Operations = operations;
        }

        public bool IsOperator(string token) => Operations.Any(o => o.CanExecute(token));
    }
}
