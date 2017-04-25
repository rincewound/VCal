using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcal.VariableResolving
{
    public class UserFunctionResolver: IFunctionResolver
    {
        public delegate object FunctionCall(List<AST.Node> parameters, SymbolTable symTable);

        AST.Node call;
        SymbolTable symTable;
        List<string> paramList;

        public UserFunctionResolver(List<string> parameters, AST.Node function, SymbolTable table)
        {
            this.call = function;
            this.symTable = table;
            paramList = parameters;
        }

        public object Resolve(List<AST.Node> parameters)
        {
            if (parameters.Count != paramList.Count)
                throw new InvalidOperationException("To few parameters for call.");

            // Mirror parameterlist into symboltable -> note that this actually pollutes
            // the global scope, we should have something like a local table that contains only
            // the locals...
            for(int i = 0; i < paramList.Count; i++)
            {
                symTable.RegisterResolver(paramList[i], new TermResolver(parameters[i], symTable));
            }

            return call.Eval(symTable);
        }

        public object Resolve()
        {
            throw new InvalidOperationException("Cannot use a function in this context.");
            return 0f;
        }

        public override string ToString()
        {
            return "User Function";
        }
    }
}
