using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    class InterpreterException : Exception
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public InterpreterException()
            : base()
        {
        }
        public InterpreterException(string message, int row, int column)
            : base(message)
        {
            Row = row;
            Column = column;
        }
    }
}
