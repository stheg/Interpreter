using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal class Statement : Node
    {
        public bool EndThenBody { get; set; }
        public bool EndElseBody { get; set; }
        public bool EndForBody { get; set; }
        public bool EndWhile { get; set; }
        public bool IsBreakpoint { get; set; }
    }
}
