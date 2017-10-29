using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;
using Xunit;
using Assert = PasPasPasTests.Common.Assert;

namespace PasPasPasTests.Infra {

    public class FileBufferTest {

        [Fact]
        public void TestMemFileReading() {
            var pool = new StringPool();
            var data = "this is a simple test";
            var buffer = new FileBuffer();
            var reference = new FileReference(pool, "test.pas");
            var content = new StringBufferReadable(data);

            buffer.Add(reference, content);

            Assert.AreEqual(buffer[reference].Data.ToString(), data);
            Assert.AreEqual(buffer[reference].Length, data.Length);
        }

        [Fact]
        public void TestDiskFileReading() {
            var pool = new StringPool();
            var data = "this is a simple test";
            var buffer = new FileBuffer();
            var reference = new FileReference(pool, "test.pas");
            var tempPath = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(tempPath, data);
            var content = new FileBufferReadable(new FileReference(pool, tempPath));

            buffer.Add(reference, content);

            Assert.AreEqual(buffer[reference].Data.ToString(), data);
            Assert.AreEqual(buffer[reference].Length, data.Length);
        }

    }
}
