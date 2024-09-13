using ConsoleApplicationCancl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationCancl.Operations
{
    public class Multiplication : IOperation
    {
        public double Calculate(double leftOperand, double rightOperand) => leftOperand * rightOperand;

        public bool CanExecute(string operation) => operation == "*";
    }
}
