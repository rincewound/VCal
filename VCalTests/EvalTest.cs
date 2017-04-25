using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vcal.Parser;
using vcal;
using vcal.VariableResolving;
using System.Collections.Generic;

namespace VCalTests
{
    [TestClass]
    public class EvalTest
    {
        TermParse tp = new TermParse();
        SymbolTable vt;

        [TestInitialize]
        public void Init()
        {
            vt = new SymbolTable();
        }

        [TestMethod]
        public void AddEval()
        {
            Assert.AreEqual(3, (float)tp.Parse("2+1").Eval(null));
        }

        [TestMethod]
        public void SubEval()
        {
            Assert.AreEqual(1, (float)tp.Parse("2-1").Eval(null));
        }

        [TestMethod]
        public void MulEval()
        {
            Assert.AreEqual(6, (float)tp.Parse("2*3").Eval(null));
        }

        [TestMethod]
        public void DivEval()
        {
            Assert.AreEqual(2, (float)tp.Parse("6/3").Eval(null));
        }

        [TestMethod]
        public void DivEvalVariable()
        {
            vt.Register("z", 2.0f);
            Assert.AreEqual(4, (float)tp.Parse("8/z").Eval(vt));
        }

        [TestMethod]
        public void CrossEval_EvaulatesCorrectly()
        {
            vt.RegisterResolver("z", new TermResolver(tp.Parse("x + 7"), vt));       //Equals: z = x + 7
            vt.Register("x", 42);
            var theTerm = tp.Parse("z + 1");

            var result = theTerm.Eval(vt);
            Assert.AreEqual(50, (float)result);
        }

        [TestMethod]
        public void FuncEval_NoParameter_EvaulatesCorrectly()
        {
            BuiltInFunctionResolver.FunctionCall fc = ((x, y) => 4);
            var funcRes = new BuiltInFunctionResolver(fc, vt);
            vt.RegisterResolver("Test", funcRes);       //Register the function defined above as "Sqrt"
            var theTerm = tp.Parse("Test()");
            var result = theTerm.Eval(vt);
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void FuncEval_SingleParameter_EvaluatesCorrectly()
        {
            BuiltInFunctionResolver.FunctionCall fc = ((x, y) => (float)Math.Sqrt(((float)x[0].Eval(vt))));
            var funcRes = new BuiltInFunctionResolver(fc, vt);
            vt.RegisterResolver("Sqrt", funcRes);       //Register the function defined above as "Sqrt"
            var theTerm = tp.Parse("Sqrt(9)");
            var result = theTerm.Eval(vt);
            Assert.AreEqual(3, (float)result);
        }

        [TestMethod]
        public void FuncEval_SingleParameterNestedFunctionCall_EvaluatesCorrectly()
        {
            BuiltInFunctionResolver.FunctionCall fc = ((x, y) => (float)Math.Sqrt(((float)x[0].Eval(vt))));
            var funcRes = new BuiltInFunctionResolver(fc, vt);
            vt.RegisterResolver("Sqrt", funcRes);       //Register the function defined above as "Sqrt"
            var theTerm = tp.Parse("Sqrt(Sqrt(9*9))");
            var result = theTerm.Eval(vt);
            Assert.AreEqual(3, (float)result);
        }

        [TestMethod]
        public void FuncEval_MultipleParams_EvaluatesCorrectly()
        {
            BuiltInFunctionResolver.FunctionCall sqrt = ((x, y) => (float)Math.Sqrt(((float)x[0].Eval(vt))));
            var funcRes = new BuiltInFunctionResolver(sqrt, vt);
            vt.RegisterResolver("Sqrt", funcRes);       //Register the function defined above as "Sqrt"

            BuiltInFunctionResolver.FunctionCall pow = ((x, y) => (float)Math.Pow((float)x[0].Eval(vt), (float)x[1].Eval(vt)));
            var fr = new BuiltInFunctionResolver(pow, vt);
            vt.RegisterResolver("Pow", fr);       //Register the function defined above as "Pow"
            var theTerm = tp.Parse("Pow(4, Sqrt(4))");
            var result = theTerm.Eval(vt);
            Assert.AreEqual(16, (float)result);
        }

        [TestMethod]
        public void BuiltIn_Sqrt_EvaluatesCorrectly()
        {
            Assert.AreEqual(tp.EvalString("Sqrt(9)"), 3f);
        }

        [TestMethod]
        public void BuiltIn_Pow_EvaluatesCorrectly()
        {
            Assert.AreEqual(tp.EvalString("Pow(3, 2)"), 9f);
        }

        [TestMethod]
        public void BuiltIn_Pi_EvaluatesCorrectly()
        {
            Assert.AreEqual(tp.EvalString("PI"), (float) Math.PI);
        }

        [TestMethod]
        public void BuiltIn_Sin_EvaluatesCorrectly()
        {
            Assert.AreEqual(tp.EvalString("Sin(PI / 2)"), 1f);
        }

        [TestMethod]
        public void BuiltIn_Cos_EvaluatesCorrectly()
        {
            Assert.AreEqual(tp.EvalString("Cos(0)"), 1f);
        }

        [TestMethod]
        public void BuiltIn_Sum_EvaluatesCorrectly()
        {
            Assert.AreEqual(tp.EvalString("Sum({2,4,6})"), 12f);
        }

        [TestMethod]
        public void BuiltIn_Max_EvaluatesCorrectly()
        {
            Assert.AreEqual(tp.EvalString("Max(12, 10)"), 12f);
        }

        [TestMethod]
        public void BuiltIn_Min_EvaluatesCorrectly()
        {
            Assert.AreEqual(tp.EvalString("Min(12, 10)"), 10f);
        }

        [TestMethod]
        public void Eval_UnknownFunction_ThrowsException()
        {
            try
            {
                tp.EvalString("ThisShouldThrow(1,2,3)");
                Assert.Fail("Above call did not throw!");
            }
            catch(KeyNotFoundException)
            {

            }
        }

        [TestMethod]
        public void Eval_UnknownVariable_ThrowsException()
        {
            try
            {
                tp.EvalString("Max(a,2,3)");
                Assert.Fail("Above call did not throw!");
            }
            catch (KeyNotFoundException)
            {

            }
        }

        [TestMethod]
        public void Eval_AddTwoLists_ThrowsException()
        {
            tp.EvalString("{1, 2, 3} + {4, 5, 6}");
        }

        [TestMethod]
        public void Eval_ListFromVar_EvalsCorrectly()
        {
            vt.RegisterResolver("x", new TermResolver(tp.Parse("{1,2,3}"), vt));

            var result = tp.EvalString("Sum(x)", vt);

            Assert.AreEqual(result, 6f);
        }

        [TestMethod]
        public void BuiltIn_Merge_EvaluatesCorrectly()
        {
            var result = tp.EvalString("Sum(Merge({1,2},{3,4}))");

            Assert.AreEqual(result, 10f);
        }

        [TestMethod]
        public void BuiltIn_Def_CreatesUserFunction()
        {
            tp.EvalString("Def(TestFunc, {x,y,z}, x + y + z)", vt);
            var sym = vt.GetSymbolResolver("TestFunc");
            Assert.IsInstanceOfType(sym, typeof(UserFunctionResolver));
        }

        [TestMethod]
        public void CanCallUserFunction()
        {
            tp.EvalString("Def(TestFunc, {x}, x + 1)", vt);
            var result = tp.EvalString("TestFunc(1)", vt);
            Assert.AreEqual(result, 2f);
        }

        [TestMethod]
        public void HigherOrderFunction()
        {
            tp.EvalString("Def(Sq, {x}, x * x)", vt);
            tp.EvalString("Def(Double, {y, z}, 2 * y(6))", vt);
            var result = tp.EvalString("Double(Sq(z),2)", vt);  // Call to Sq is bound to z at parsetime -> crud.
            Assert.AreEqual(result, 8f);
            Assert.Fail("Syntax is rubbish!");
        }
    }
}
