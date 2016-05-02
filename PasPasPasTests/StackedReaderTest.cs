using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Input;
using System;
using System.Collections.Generic;
using System.IO;
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
            using (var input = new StringInput(Content1, "test.pas"))
            using (var reader = new StackedFileReader()) {
                reader.AddFile(input);
                StringBuilder result = new StringBuilder();
                while (!reader.AtEof) {
                    result.Append(reader.FetchChar());
                }
                Assert.AreEqual(Content1, result.ToString());
            }
        }

        [TestMethod]
        public void TestSimpleFileRead() {
            var path = GenerateTempFile(Content1);
            try {
                using (var reader = new StackedFileReader()) {
                    reader.AddFile(new FileInput(path));
                    StringBuilder result = new StringBuilder();
                    while (!reader.AtEof) {
                        result.Append(reader.FetchChar());
                    }
                    Assert.AreEqual(Content1, result.ToString());
                }
            }
            finally {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void TestStackedRead() {
            var path1 = GenerateTempFile(Content1);
            var path2 = GenerateTempFile(Content2);
            try {
                using (var reader = new StackedFileReader())
                using (var file1 = new FileInput(path1))
                using (var file2 = new FileInput(path2)) {
                    reader.AddFile(file1);
                    StringBuilder result = new StringBuilder();
                    while (!reader.AtEof && result.Length < 5) {
                        result.Append(reader.FetchChar());
                    }
                    reader.AddFile(file2);
                    while (!reader.AtEof) {
                        result.Append(reader.FetchChar());
                    }

                    Assert.AreEqual(//
                        Content1.Substring(0, 5) + //
                        Content2 + //
                        Content1.Substring(5), result.ToString());
                }

            }
            finally {
                File.Delete(path1);
                File.Delete(path2);
            }
        }

        private string GenerateTempFile(string content) {
            var result = Path.GetTempFileName();
            File.AppendAllText(result, content);
            return result;
        }

        [TestMethod]
        public void TestNestedRead() {
            using (var input1 = new StringInput(Content1, "c1"))
            using (var input2 = new StringInput(Content2, "c2"))
            using (var reader = new StackedFileReader()) {
                reader.AddFile(input1);
                StringBuilder result = new StringBuilder();
                while (!reader.AtEof && result.Length < 5) {
                    result.Append(reader.FetchChar());
                }
                reader.AddFile(input2);
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
}
