using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace VCalTests
{
    [TestClass]
    public class TokenizerTests
    {
        [TestMethod]
        public void SingleSignSplits_CreateDistinctElements()
        {
            var tk = new vcal.Parser.Tokenizer(new[] { " ", "." });

            var result = tk.Tokenize("This is.ATest").ToArray();

            Assert.AreEqual(result.Length, 5);
        }

        [TestMethod]
        public void MultiSignSplits_CreateDistinctElements()
        {
            var tk = new vcal.Parser.Tokenizer(new[] { "==", "=>" });

            var result = tk.Tokenize("This==is=>ATest").ToArray();

            Assert.AreEqual(result.Length, 5);
        }

        [TestMethod]
        public void MixedDistinctSplits_CreateDistinctElements()
        {
            var tk = new vcal.Parser.Tokenizer(new[] { "!", "=>" });

            var result = tk.Tokenize("This!is=>ATest").ToArray();

            Assert.AreEqual(result.Length, 5);
        }
    }
}
