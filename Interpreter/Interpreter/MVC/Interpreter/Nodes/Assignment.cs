using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Assignment : Statement
    {
        public Variable Destination { get; set; }
        public Expression Source { get; set; }
    }
}
