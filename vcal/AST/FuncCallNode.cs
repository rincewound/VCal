using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.VariableResolving;

namespace vcal.AST
{
    public class FuncCallNode : Node
    {
        string funcName;
        List<Node> parameters;

        public FuncCallNode(string funcName, List<Node> parameters)
        {
            this.funcName = funcName;
            this.parameters = parameters;
        }

        public override object Eval(SymbolTable vartable)
        {
            var funcRef = vartable.GetSymbolResolver(funcName) as IFunctionResolver;
            return funcRef.Resolve(parameters);
        }

        public override string ToString()
        {
            return "Call " + funcName;
        }
    }
}
