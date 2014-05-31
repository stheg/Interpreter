using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter
{
    internal sealed class Number : Expression
    {
        public float Value { get; set; }
    }
}
