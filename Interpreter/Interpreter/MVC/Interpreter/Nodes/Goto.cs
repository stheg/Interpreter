using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Goto : Statement
    {
        public Variable Identificator { get; set; }
    }
}