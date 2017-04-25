using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.AST;
using vcal.VariableResolving;

namespace vcal.BuiltIn
{
    public static class Lists
    {
        [Export]
        public static object Max(List<Node> parameters, SymbolTable symTable)
        {
            return parameters.Max(x => (float) x.Eval(symTable));
        }

        [Export]
        public static object Min(List<Node> parameters, SymbolTable symTable)
        {
            return parameters.Min(x => (float)x.Eval(symTable));
        }

        [Export]
        public static object Sum(List<Node> parameters, SymbolTable symTable)
        {
            var lst = parameters[0].Eval(symTable) as ListNode;
            if (lst == null)
                throw new Exception("SUM can only work with lists");
            return lst.entries.Select(x => (float)x.Eval(symTable)).Sum();
        }

        [Export]
        public static Node Merge(List<Node> parameters, SymbolTable symTable)
        {
            var a = parameters[0].Eval(symTable) as ListNode;
            var b = parameters[1].Eval(symTable) as ListNode;
            var newList = new List<Node>();
            newList.AddRange(a.entries);
            newList.AddRange(b.entries);            
            return new ListNode(newList);
        }

    }
}
