using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.AST;

namespace vcal.VariableResolving
{
    class StaticValueResolver : IFunctionResolver
    {
        float val;

        public StaticValueResolver(float value)
        {
            val = value;
        }

        public object Resolve()
        {
            return val;
        }

        public object Resolve(List<Node> parameters)
        {
            return val;
        }

        public override string ToString()
        {
            return "Static Value (" + val + ")";
        }
    }
}
