using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Variable : Expression
    {
        public string Value { get; set; }
    }
}
