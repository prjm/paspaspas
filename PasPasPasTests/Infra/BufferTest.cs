using System.IO;
using System.Text;
using PasPasPas.Infrastructure.Files;
using PasPasPasTests.Common;
using Buffer = PasPasPas.Infrastructure.Files.Buffer;

namespace PasPasPasTests.Infra {

    public class BufferTest {

        private readonly string[] utf8Samples = new string[] {
            //"1234567890",
            "κόσμε",
        };

        [TestCase]
        public void TestUTf8BufferForwardRead() {
            for (var bufferSize = 4; bufferSize < 20; bufferSize++) {
                var data = "1234567890";
                var bytes = Encoding.UTF8.GetBytes(data);
                var result = "";
                using (var source = new Utf8StreamBufferSource(new MemoryStream(bytes), bufferSize)) {
                    var buffer = new Buffer(source, bufferSize);
                    Assert.IsTrue(buffer.IsAtBeginning);

                    while (!buffer.IsAtEnd) {
                        result = result + buffer.Content[buffer.BufferIndex];
                        buffer.Position++;
                    }
                    Assert.IsTrue(buffer.IsAtEnd);
                    Assert.AreEqual(data, result);
                }
            }
        }

        [TestCase]
        public void TestUtf8ForwardAndBackwardRead() {
            for (var bufferSize = 4; bufferSize < 20; bufferSize++) {
                var data = "1234567890";
                var bytes = Encoding.UTF8.GetBytes(data);
                using (var source = new Utf8StreamBufferSource(new MemoryStream(bytes), bufferSize)) {
                    var buffer = new Buffer(source, bufferSize);
                    var result1 = string.Empty;
                    var result2 = string.Empty;
                    Assert.IsTrue(buffer.IsAtBeginning);

                    while (!buffer.IsAtEnd) {
                        result1 = result1 + buffer.Content[buffer.BufferIndex];
                        buffer.Position++;
                        buffer.Position--;
                        result2 = result2 + buffer.Content[buffer.BufferIndex];
                        buffer.Position++;
                    }
                    Assert.IsTrue(buffer.IsAtEnd);
                    Assert.AreEqual(data, result1);
                    Assert.AreEqual(data, result2);
                }
            }
        }

        [TestCase]
        public void TestSimpleBufferForwardRead() {
            for (var bufferSize = 1; bufferSize < 20; bufferSize++) {
                var data = "1234567890";
                var result = "";
                var source = new StringBufferSource(data);
                var buffer = new Buffer(source, bufferSize);
                Assert.IsTrue(buffer.IsAtBeginning);

                while (!buffer.IsAtEnd) {
                    result = result + buffer.Content[buffer.BufferIndex];
                    buffer.Position++;
                }
                Assert.IsTrue(buffer.IsAtEnd);
                Assert.AreEqual(data, result);
            }
        }

        [TestCase]
        public void TestSimpleBufferForwardAndBackwardRead() {
            for (var bufferSize = 1; bufferSize < 20; bufferSize++) {
                var data = "1234567890";
                var source = new StringBufferSource(data);
                var buffer = new Buffer(source, bufferSize);
                var result1 = string.Empty;
                var result2 = string.Empty;
                Assert.IsTrue(buffer.IsAtBeginning);

                while (!buffer.IsAtEnd) {
                    result1 = result1 + buffer.Content[buffer.BufferIndex];
                    buffer.Position++;
                    buffer.Position--;
                    result2 = result2 + buffer.Content[buffer.BufferIndex];
                    buffer.Position++;
                }
                Assert.IsTrue(buffer.IsAtEnd);
                Assert.AreEqual(data, result1);
                Assert.AreEqual(data, result2);
            }
        }

        [TestCase]
        public void TestUtf8Samples() {
            for (var bufferSize = 4; bufferSize < 20; bufferSize++) {
                foreach (var data in utf8Samples) {
                    var bytes = Encoding.UTF8.GetBytes(data);
                    using (var source = new Utf8StreamBufferSource(new MemoryStream(bytes), bufferSize)) {
                        var buffer = new Buffer(source, bufferSize);
                        var result1 = string.Empty;
                        var result2 = string.Empty;
                        Assert.IsTrue(buffer.IsAtBeginning);

                        while (!buffer.IsAtEnd) {
                            result1 = result1 + buffer.Content[buffer.BufferIndex];
                            buffer.Position++;
                            buffer.Position--;
                            result2 = result2 + buffer.Content[buffer.BufferIndex];
                            buffer.Position++;
                        }
                        Assert.IsTrue(buffer.IsAtEnd);
                        Assert.AreEqual(data, result1);
                        Assert.AreEqual(data, result2);
                    }
                }
            }
        }
    }
}
