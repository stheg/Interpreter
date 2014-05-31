using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Program : Node
    {
        public StatementList StatementList { get; set; }
    }
}
