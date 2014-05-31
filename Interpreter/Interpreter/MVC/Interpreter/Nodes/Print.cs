using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Print : Statement
    {
        public string TextToPrint { get; set; }
        public Expression ExprToPrint { get; set; }
    }
}
