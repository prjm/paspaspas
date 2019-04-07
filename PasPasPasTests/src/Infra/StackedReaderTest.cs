using System.IO;
using System.Text;
using PasPasPas.Api;
using PasPasPasTests.Common;


namespace PasPasPasTests.Infra {

    /// <summary>
    ///     test for stacked reader
    /// </summary>
    public class StackedReaderTest : CommonTest {

        private static string Content1
            => "X1X1X1|||X1X1X1------------$";

        private static string Content2
            => "ZZZ$44345845784875DSDFDDFDS";

        [TestMethod]
        public void TestSimpleRead() {
            using (var reader = ReaderApi.CreateReaderForString("test.pas", Content1)) {
                var result = new StringBuilder();

                while (!reader.AtEof) {
                    result.Append(reader.NextChar());
                }

                Assert.AreEqual(Content1, result.ToString());
            }
        }

        [TestMethod]
        public void TestSimpleFileRead() {

            var path = GenerateTempFile(Content1);
            var result = new StringBuilder();

            try {
                using (var reader = ReaderApi.CreateReaderForPath(path)) {
                    while (!reader.AtEof) {
                        result.Append(reader.NextChar());
                    }
                }
                Assert.AreEqual(Content1, result.ToString());
            }
            finally {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void TestStackedRead() {

            var splitIndex = 5;
            var result = new StringBuilder();
            var path1 = GenerateTempFile(Content1);
            var path2 = GenerateTempFile(Content2);
            var readerApi = new ReaderApi(CreateEnvironment());

            try {
                using (var reader = ReaderApi.CreateReaderForPath(path1)) {
                    while (!reader.AtEof && result.Length < splitIndex) {
                        result.Append(reader.NextChar());
                    }
                    var len = splitIndex;
                    ReaderApi.SwitchToPath(reader, path2);
                    while (reader.CurrentFile != null && !reader.AtEof) {
                        result.Append(reader.NextChar());
                        len++;

                        if (len == (Content2.Length + splitIndex) || len == Content1.Length + Content2.Length)
                            Assert.IsTrue(reader.AtEof);
                        else
                            Assert.IsFalse(reader.AtEof);

                        if (reader.AtEof)
                            reader.FinishCurrentFile();
                    }

                    Assert.AreEqual(//
                        Content1.Substring(0, splitIndex) + //
                        Content2 + //
                        Content1.Substring(5), result.ToString());
                }
            }
            finally {
                File.Delete(path1);
                File.Delete(path2);
            }
        }

        private static string GenerateTempFile(string content) {
            var result = Path.GetTempFileName();
            File.AppendAllText(result, content);
            return result;
        }

        [TestMethod]
        public void TestNestedRead() {

            var result = new StringBuilder();
            var path1 = GenerateTempFile(Content1);
            var path2 = GenerateTempFile(Content2);
            using (var reader = ReaderApi.CreateReaderForPath(path1)) {

                while (!reader.AtEof && result.Length < 5) {
                    result.Append(reader.NextChar());
                }

                ReaderApi.SwitchToPath(reader, path2);
                while (reader.CurrentFile != null && !reader.AtEof) {
                    result.Append(reader.NextChar());

                    if (reader.AtEof)
                        reader.FinishCurrentFile();
                }

                Assert.AreEqual(//
                    Content1.Substring(0, 5) + //
                    Content2 + //
                    Content1.Substring(5), result.ToString());
            }
        }
    }
}
