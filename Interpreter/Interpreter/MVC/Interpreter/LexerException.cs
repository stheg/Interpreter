using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    class LexerException : Exception
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public LexerException()
            : base()
        {
        }
        public LexerException(string message, int row, int column)
            : base(message)
        {
            Row = row;
            Column = column;
        }
    }
}
