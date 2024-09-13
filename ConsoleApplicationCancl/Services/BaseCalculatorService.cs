using ConsoleApplicationCancl.Interfaces;

namespace ConsoleApplicationCancl.Services
{
    public abstract class BaseCalculatorService
    {
        protected readonly List<IOperation> Operations;

        protected BaseCalculatorService(List<IOperation> operations)
        {
            Operations = operations;
        }

        protected bool IsOperator(string token) => Operations.Any(o => o.CanExecute(token));
    }
}
