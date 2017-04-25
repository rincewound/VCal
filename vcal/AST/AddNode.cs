using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.VariableResolving;

namespace vcal.AST
{
    public class AddNode: OpNode
    {
      public override object Eval(SymbolTable vartable)
      {
          return (float)Left.Eval(vartable) + (float)Right.Eval(vartable);
      }
    }
}
