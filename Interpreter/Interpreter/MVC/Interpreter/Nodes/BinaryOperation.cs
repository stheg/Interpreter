using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class BinaryOperation : Expression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public BinaryOperationType Type { get; set; }

        public enum BinaryOperationType
        {
            Add,
            Sub,
            Mult,
            Div,
            Deg,
        }
    }
}
