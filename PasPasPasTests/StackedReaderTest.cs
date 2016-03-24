using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Infrastructure.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPasTests {

    /// <summary>
    ///     test for stacked reader
    /// </summary>
    [TestClass]
    public class StackedReaderTest {

        private string Content1 => "X1X1X1|||X1X1X1------------$";

        private string Content2 = "ZZZ$44345845784875DSDFDDFDS";

        [TestMethod]
        public void TestSimpleRead() {
            var reader = new StackedFileReader();
            reader.AddFile(new StringInput(Content1));
            StringBuilder result = new StringBuilder();
            while (!reader.AtEof) {
                result.Append(reader.FetchChar());
            }
            Assert.AreEqual(Content1, result.ToString());
        }

        [TestMethod]
        public void TestNestedRead() {
            var reader = new StackedFileReader();
            reader.AddFile(new StringInput(Content1));
            StringBuilder result = new StringBuilder();
            while (!reader.AtEof && result.Length < 5) {
                result.Append(reader.FetchChar());
            }
            reader.AddFile(new StringInput(Content2));
            while (!reader.AtEof) {
                result.Append(reader.FetchChar());
            }
            Assert.AreEqual(//
                Content1.Substring(0, 5) + //
                Content2 + //
                Content1.Substring(5), result.ToString());
        }

    }
}
