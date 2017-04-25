using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.AST;

namespace vcal.VariableResolving
{
    public class TermResolver : IFunctionResolver
    {
        Node termNode;
        SymbolTable sourceVt;

        public TermResolver(Node term, SymbolTable sourceVt)
        {
            if (term == null)
                throw new InvalidOperationException("Bad term!");
            termNode = term;
            this.sourceVt = sourceVt;
        }

        public object Resolve()
        {
            return termNode.Eval(sourceVt);
        }

        public object Resolve(List<AST.Node> parameters)
        {
            return termNode.Eval(sourceVt);
        }

        public override string ToString()
        {
            return "Term";
        }
    }
}
