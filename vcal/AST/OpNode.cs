using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcal.AST
{
    public abstract class OpNode: Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
    }
}
