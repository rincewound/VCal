using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.VariableResolving;

namespace vcal.AST
{
    public class VarRefNode: Node
    {
        public string varName { get; private set; }

        public VarRefNode(string varName)
        {
            this.varName = varName;
        }

        public override object Eval(SymbolTable vartable)
        {
            return vartable.GetSymbolResolver(varName).Resolve(new List<Node>());
        }

        public override string ToString()
        {
            return "@" + varName;
        }
    }
}
                                      