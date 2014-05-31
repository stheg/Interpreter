using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    public class InterpretationEventArgs : EventArgs
    {
        public string ProgramCode { get; set; }
        public BreakpointPosition BreakpointPosition { get; set; }
        public InterpretationEventArgs()
        {
        }
        public InterpretationEventArgs(string programCode)
        {
            ProgramCode = programCode;
        }
        public InterpretationEventArgs(string programCode, BreakpointPosition breakpointPosition)
        {
            ProgramCode = programCode;
            BreakpointPosition = breakpointPosition;
        }
    }

    public class BreakpointEventArgs : EventArgs
    {
        public int Row { get; set; }
        public int ColStartPos { get; set; }
        public int ColLastPos { get; set; }

        public BreakpointEventArgs(int row, int colStartPos)
        {
            Row = row;
            ColStartPos = colStartPos;
        }
    }

    public class ErrorEventArgs : EventArgs
    {
        public string Reason { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public ErrorEventArgs()
        {
        }

        public ErrorEventArgs(string reason, int row, int column)
        {
            Reason = reason;
            Row = row;
            Column = column;
        }
    }

    public class OutputEventArgs : EventArgs
    {
        public OutputEventArgs()
        {
        }

        //public OutputEventArgs(
    }
}
