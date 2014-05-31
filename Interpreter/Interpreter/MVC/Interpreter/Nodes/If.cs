using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class If : Statement
    {
        public LogicExpression LogicCondition { get; set; }
        public StatementList ThenBody { get; set; }
        public StatementList ElseBody { get; set; }
    }
}
