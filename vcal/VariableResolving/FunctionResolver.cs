using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcal.VariableResolving
{
    public class BuiltInFunctionResolver : IFunctionResolver
    {
        public delegate object FunctionCall(List<AST.Node> parameters, SymbolTable symTable);

        FunctionCall call;
        SymbolTable symTable;

        public BuiltInFunctionResolver(FunctionCall call, SymbolTable table)
        {
            this.call = call;
            this.symTable = table;
        }

        public object Resolve(List<AST.Node> parameters)
        {
            return call(parameters, symTable);
        }

        public object Resolve()
        {
            throw new InvalidOperationException("Cannot use a function in this context.");
        }

        public override string ToString()
        {
            return "BuiltIn Function";
        }
    }
}
