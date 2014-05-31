using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class For : Statement
    {
        public Assignment Initialization { get; set; }
        public LogicExpression LogicCondition { get; set; }
        public Assignment ExprForAction { get; set; }
        public StatementList Body { get; set; }
    }
}
