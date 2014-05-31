using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class StatementList : Node
    {
        public LinkedList<Statement> List { get; set; }
    }
}
