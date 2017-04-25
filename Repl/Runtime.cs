using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.AST;
using vcal.VariableResolving;

namespace Repl
{
    public static class Runtime
    {
        public static object Let(List<Node> parameters, SymbolTable symTable)
        {
            var name = (parameters[0] as VarRefNode).varName;
            symTable.RegisterResolver(name, new TermResolver(parameters[1], symTable));
            return 0f;
        }

        public static object Env(List<Node> parameters, SymbolTable symTable)
        {
            foreach(var kv in symTable.Symbols)
            {
                Console.WriteLine(kv.Key + ":       " + kv.Value);
            }
            return 0f;
        }
    }
}
