using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcal.VariableResolving
{
    public interface IFunctionResolver
    {
        object Resolve(List<AST.Node> parameters);
        object Resolve();

        //void Inspect(System.IO.StreamWriter targetOutput);
    }
}
