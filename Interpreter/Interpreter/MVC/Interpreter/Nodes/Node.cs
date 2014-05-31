using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal class Node
    {
        public int Row { get; set; }
        public int ColumnStartPos { get; set; }
        public int ColumnLastPos { get; set; }
        public Dictionary<string, Statement> ListOfLabels { get; set; }
    }
}
