using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vcal.VariableResolving;
using vcal.BuiltIn;
using vcal.AST;
using System.Collections.Generic;

namespace VCalTests
{
    public static class TestPackage
    {
        [Export]
        public static object TestImport(List<Node> parameters, SymbolTable symTable)
        {
            return 0f;
        }
    }

    [TestClass]
    public class PackageImport
    {
        [TestMethod]
        public void Import_ImportsAllMarkedMethods()
        {
            var st = new SymbolTable();
            st.Symbols.Clear();

            var imp = new PackageImporter();
            imp.ImportPackage(typeof(TestPackage), st);

            Assert.AreEqual(1, st.Symbols.Count);

        }
    }
}
