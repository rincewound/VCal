using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vcal.Parser;
using vcal.AST;
using vcal;
using vcal.VariableResolving;

namespace VCalTests
{
    [TestClass]
    public class TermParseTest
    {
        [TestMethod]
        public void ParseAdd_CreatesAdderNode()
        {
            var t = new TermParse();
            var result = t.Parse("1 + 1");

            Assert.IsTrue(result is AddNode);
        }

        [TestMethod]
        public void ParseNoOperators_YieldsStaticValue()
        {
            var t = new TermParse();
            var result = t.Parse("11");

            Assert.IsTrue(result is StaticValueNode);
        }

        [TestMethod]
        public void ParseJustAString_YieldsVariableLookupNode()
        {
            var t = new TermParse();
            var result = t.Parse("ThisIsAVariable");

            Assert.IsTrue(result is VarRefNode);
        }

        [TestMethod]
        public void ParseMultipleOps_OperatorPrecedence_AddMul()
        {
            var t = new TermParse();
            var result = t.Parse("1 + 1 * 4" );

            Assert.IsTrue(result is AddNode);

            var n = result as AddNode;

            Assert.IsTrue(n.Right is MulNode);
            Assert.AreEqual(result.Eval(null), 5f);
        }

        [TestMethod]
        public void ParseMultipleOps_OperatorPrecedence_MulAdd()
        {
            var t = new TermParse();
            var result = t.Parse("2 * 2 + 4");
            Assert.AreEqual(result.Eval(null), 8f);
        }

        [TestMethod]
        public void ParseMultipleOps_Brackets_EvaluatedCorrectly()
        {
            var t = new TermParse();
            var result = t.Parse("2 * (2 + 4)");
            Assert.AreEqual(result.Eval(null), 12f);
        }

        [TestMethod]
        public void Parse_VarIsEvaluated()
        {
            var vt = new SymbolTable();
            vt.Register("x", 2.0f);
            var t = new TermParse();
            var result = t.Parse("x * 2");
            Assert.AreEqual(result.Eval(vt), 4.0f);
        }

        [TestMethod]
        public void Parse_FuncCall_YieldsFuncCallNode()
        {
            var result = new TermParse().Parse("fnord()");
            Assert.IsInstanceOfType(result, typeof(FuncCallNode));
        }

        [TestMethod]
        public void Parse_NestedFuncCall_YieldsFuncCallNode()
        {
            var result = new TermParse().Parse("fnord(fark())");
            Assert.IsInstanceOfType(result, typeof(FuncCallNode));
        }

        [TestMethod]
        public void Parse_MultipleParameters_YieldsFuncCallNode()
        {
            var result = new TermParse().Parse("fnord(1, 3, 5 + foo())");
            Assert.IsInstanceOfType(result, typeof(FuncCallNode));
        }

        [TestMethod]
        public void Parse_RandomGarbage_ThrowsException()
        {
            try
            {
                new TermParse().Parse("2, 2, 3)");
                Assert.Fail("Above call did not throw!");
            }
            catch (Exception)
            {

            }
        }

        [TestMethod]
        public void Parse_List_YieldsListNode()
        {
            var lst = new TermParse().Parse("{a, 1, 2, 5}");
            Assert.IsInstanceOfType(lst, typeof(ListNode));
        }

    }
}
