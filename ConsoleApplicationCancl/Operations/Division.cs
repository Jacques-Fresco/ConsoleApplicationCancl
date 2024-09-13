using ConsoleApplicationCancl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationCancl.Operations
{
    public class Division : IOperation
    {
        public double Calculate(double leftOperand, double rightOperand)
        {
            if (rightOperand == 0) throw new DivideByZeroException("Деление на ноль не допускается.");
            return leftOperand / rightOperand;
        }

        public bool CanExecute(string operation) => operation == "/";
    }
}
