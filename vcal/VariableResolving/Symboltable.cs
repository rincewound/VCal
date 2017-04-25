using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vcal.BuiltIn;

namespace vcal.VariableResolving
{
    public class SymbolTable
    {
        Dictionary<string, IFunctionResolver> symbols = new Dictionary<string, IFunctionResolver>();

        public Dictionary<string, IFunctionResolver> Symbols { get { return symbols; } }

        public SymbolTable()
        {
            var importer = new PackageImporter();
            importer.ImportPackage(typeof(BuiltIn.Lists), this);
            importer.ImportPackage(typeof(BuiltIn.Arith), this);
            importer.ImportPackage(typeof(BuiltIn.Misc), this);
            Register("PI", (float) Math.PI);
        }

        public void RegFunc(string name, BuiltInFunctionResolver.FunctionCall call)
        {
            RegisterResolver(name, new BuiltInFunctionResolver(call, this));
        }

        public void Register(string symbolName, float value)
        {
            symbols[symbolName] = new StaticValueResolver(value);
        }

        public void RegisterResolver(string symbolName, IFunctionResolver resolver)
        {
            symbols[symbolName] = resolver;
        }

        public IFunctionResolver GetSymbolResolver(string symbolName)
        {
            if (!symbols.ContainsKey(symbolName))
            {
                throw new KeyNotFoundException("Symbol: " + symbolName + " not defined.");
            }

            return symbols[symbolName];
        }

        public object Resolve(string symbolName)
        {
            if(!symbols.ContainsKey(symbolName))
            {
                throw new KeyNotFoundException("Symbol: " + symbolName + " not defined.");
            }

            return symbols[symbolName].Resolve();
        }
    }
}
