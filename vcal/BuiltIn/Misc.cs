using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.AST;
using vcal.VariableResolving;

namespace vcal.BuiltIn
{
    public static class Misc
    {

        [Export]
        public static object Def(List<Node> parameters, SymbolTable symTable)
        {
            if (parameters.Count != 3)
                throw new InvalidOperationException("Def needs exactly 3 parameters.");

            var name = (parameters[0] as VarRefNode).varName;
            var callParams = (parameters[1].Eval(symTable) as ListNode).entries.Select(y => (y as VarRefNode).varName).ToList();
            var code = parameters[2];

            symTable.RegisterResolver(name, new UserFunctionResolver(callParams, code, symTable));
            return 0f;
        }
    }
}
