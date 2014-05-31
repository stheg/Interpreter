using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class While : Statement
    {
        public LogicExpression LogicCondition { get; set; }
        public StatementList Body { get; set; }
    }
}
