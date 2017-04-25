using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.VariableResolving;

namespace vcal.AST
{
    public abstract class Node
    {
        public abstract object Eval(SymbolTable vartable);

        public virtual Node Expand()
        {
            return this;
        }
    }
}
