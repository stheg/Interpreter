using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class LogicExpression : Node
    {
        public Expression LeftExpr { get; set; }
        public Expression RightExpr { get; set; }
        public LogicOperation LogicOpearation { get; set; }

        public enum LogicOperation
        {
            More,
            MoreEqual,
            Less,
            LessEqual,
            Equal,
            NotEqual,
        }
    }
}
