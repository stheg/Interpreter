using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    class ParserException : Exception
    {
        public int ColStartPos { get; set; }
        public int ColLastPos { get; set; }
        public int Row { get; set; }

        public ParserException()
            : base()
        {
        }

        public ParserException(string message, int row, int colStartPos, int colLastPos)
            : base(message)
        {
            ColStartPos = colStartPos;
            ColLastPos = colLastPos;
            Row = row;
        }
    }
}
