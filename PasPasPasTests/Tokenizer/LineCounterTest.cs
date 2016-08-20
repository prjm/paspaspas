using System;
using PasPasPas.Parsing.Tokenizer;
using Xunit;

namespace PasPasPasTests.Tokenizer {


    public class LineCounterTest {

        [Fact]
        public void TestSimple() {
            Assert.AreEqual(NewLineStyle.Undefined, TestLineCounterMode(""));
            Assert.AreEqual(NewLineStyle.Undefined, TestLineCounterMode("asdasddsaddas\tsdsdsaas\tasdas"));
            Assert.AreEqual(NewLineStyle.CrLf, TestLineCounterMode("sdadsdnm\r\nsdfdsfsf\r\n"));
            Assert.AreEqual(NewLineStyle.Lf, TestLineCounterMode("sdadsdnm\nsdfdsfsf\n"));
            Assert.AreEqual(NewLineStyle.Cr, TestLineCounterMode("sdadsdnm\rsdfdsfsf\r"));
            Assert.AreEqual(NewLineStyle.LfCr, TestLineCounterMode("sdadsdnm\n\rsdfdsfsf\n\r"));
            Assert.AreEqual(NewLineStyle.Mixed, TestLineCounterMode("sdadsdnm\rsdfdsfsf\n\r"));
            Assert.AreEqual(NewLineStyle.Mixed, TestLineCounterMode("sdadsdnm\r\nsdfdsfsf\n\r"));
        }

        private object TestLineCounterMode(string input)
            => TestLineCounter(input).Item1;


        private Tuple<NewLineStyle, int> TestLineCounter(string input) {
            var counter = new LineCounter();

            for (int i = 0; i < input.Length; i++) {
                counter.ProcessChar(input[i]);
            }

            return new Tuple<NewLineStyle, int>(counter.Style, counter.Line);
        }
    }
}
