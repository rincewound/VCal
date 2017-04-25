using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.AST;
using vcal.VariableResolving;

namespace vcal.BuiltIn
{
    public static class Arith
    {
        [Export]
        public static object Cos(List<Node> parameters, SymbolTable symTable)
        {
            if (parameters.Count != 1)
                throw new InvalidOperationException("Cos needs exactly 1 parameter.");
            return (float)Math.Cos((float)parameters[0].Eval(symTable));
        }

        [Export]
        public static object Sin(List<Node> parameters, SymbolTable symTable)
        {
            if (parameters.Count != 1)
                throw new InvalidOperationException("Sin needs exactly 1 parameter.");
            return (float)Math.Sin((float)parameters[0].Eval(symTable));
        }

        [Export]
        public static object Pow(List<Node> parameters, SymbolTable symTable)
        {
            if (parameters.Count != 2)
                throw new InvalidOperationException("Pow needs exactly 2 parameters.");
            return (float)Math.Pow((float)parameters[0].Eval(symTable), (float)parameters[1].Eval(symTable));
        }

        [Export]
        public static object Sqrt(List<Node> parameters, SymbolTable symTable)
        {
            if (parameters.Count != 1)
                throw new InvalidOperationException("Sqrt needs exactly 1 parameter.");
            return (float)Math.Sqrt((float)parameters[0].Eval(symTable));
        }

    }
}
