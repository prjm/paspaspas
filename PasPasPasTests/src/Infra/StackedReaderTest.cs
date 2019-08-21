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
            var env = Factory.CreateEnvironment();
            var api = Factory.CreateReaderApi(env);
            var file = env.CreateFileReference("test.pas");
            var resolver = CreateResolver(file, Content1);

            using (var reader = api.CreateReader(resolver, file)) {
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
                var env = Factory.CreateEnvironment();
                var api = Factory.CreateReaderApi(env);
                var file = env.CreateFileReference(path);
                var data = api.CreateInputForPath(file);
                using (var reader = api.CreateReader(CreateResolver(), file)) {
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
            var env = Factory.CreateEnvironment();
            var api = Factory.CreateReaderApi(env);
            var path1 = env.CreateFileReference(GenerateTempFile(Content1));
            var path2 = env.CreateFileReference(GenerateTempFile(Content2));
            var r = CreateResolver();

            try {
                using (var reader = api.CreateReader(r, path1)) {
                    while (!reader.AtEof && result.Length < splitIndex) {
                        result.Append(reader.NextChar());
                    }
                    var len = splitIndex;
                    reader.AddInputToRead(path2);
                    while (reader.CurrentFile != null && !reader.AtEof) {
                        result.Append(reader.NextChar());
                        len++;

                        if (len == Content2.Length + splitIndex || len == Content1.Length + Content2.Length)
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
                File.Delete(path1.Path);
                File.Delete(path2.Path);
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
            var env = Factory.CreateEnvironment();
            var api = Factory.CreateReaderApi(env);
            var path1 = env.CreateFileReference(GenerateTempFile(Content1));
            var path2 = env.CreateFileReference(GenerateTempFile(Content2));
            using (var reader = api.CreateReader(CreateResolver(), path1)) {

                while (!reader.AtEof && result.Length < 5) {
                    result.Append(reader.NextChar());
                }

                reader.AddInputToRead(path2);
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
