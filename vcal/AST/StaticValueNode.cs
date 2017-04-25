using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.VariableResolving;

namespace vcal.AST
{
    public class StaticValueNode : Node
    {
        private float val;

        public StaticValueNode(float value)
        {
            val = value;
        }

        public override object Eval(SymbolTable vartable)
        {
            return val;
        }

        public override string ToString()
        {
            return val.ToString();
        }
    }
}
