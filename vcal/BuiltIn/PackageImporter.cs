using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.VariableResolving;

namespace vcal.BuiltIn
{
    public class PackageImporter
    {
        public void ImportPackage(Type packageType, SymbolTable target)
        {
            var methods = packageType.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Where(x => x.CustomAttributes.Any(y => y.AttributeType.Name == "Export"));
            foreach(var m in methods)
            {
                target.RegFunc(m.Name, (BuiltInFunctionResolver.FunctionCall) m.CreateDelegate(typeof(BuiltInFunctionResolver.FunctionCall)));
            }
        }
    }
}
