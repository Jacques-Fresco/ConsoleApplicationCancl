using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationCancl.Interfaces
{
    public interface IOperation
    {
        double Calculate(double leftOperand, double rightOperand);
        bool CanExecute(string operation);
    }
}
