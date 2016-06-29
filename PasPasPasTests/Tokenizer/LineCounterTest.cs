using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPasTests.Tokenizer {


    [TestClass]
    public class LineCounterTest {

        [TestMethod]
        public void TestSimple() {
            Assert.AreEqual(NewlineStyle.Undefined, TestLineCounterMode(""));
            Assert.AreEqual(NewlineStyle.Undefined, TestLineCounterMode("asdasddsaddas\tsdsdsaas\tasdas"));
            Assert.AreEqual(NewlineStyle.CrLf, TestLineCounterMode("sdadsdnm\r\nsdfdsfsf\r\n"));
        }

        private object TestLineCounterMode(string input)
            => TestLineCounter(input).Item1;


        private Tuple<NewlineStyle, int> TestLineCounter(string input) {
            var counter = new LineCounter();

            for (int i = 0; i <= input.Length; i++) {
                counter.ProcessChar(input[i]);
            }

            return new Tuple<NewlineStyle, int>(counter.Style, counter.Line);
        }
    }
}
