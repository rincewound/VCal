using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.VariableResolving;

namespace vcal.AST
{
    public class ListNode : Node
    {
        public List<Node> entries{get; private set;}

        public ListNode(List<Node> listEntries)
        {
            entries = listEntries;
        }

        public override object Eval(SymbolTable vartable)
        {
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach(var n in entries)
            {
                sb.Append(n);
                sb.Append(",");
            }
            sb.Append("}");
            return sb.ToString();
        }

    }
}
