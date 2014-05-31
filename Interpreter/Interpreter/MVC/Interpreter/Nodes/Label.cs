using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Label : Node
    {
        public Variable Identificator { get; set; }
        public Statement NextStatement { get; set; }
    }
}
