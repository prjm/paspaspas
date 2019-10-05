using System.IO;
using System.Text;
using PasPasPas.Infrastructure.Files;
using PasPasPasTests.Common;

namespace PasPasPasTests.Infra {

    /// <summary>
    ///     buffer tests
    /// </summary>
    public class BufferTest {

        private readonly string[] utf8Samples = new string[] {
            "1234567890",
            "κόσμε",
        };

        /// <summary>
        ///     test buffer forward reading
        /// </summary>
        [TestMethod]
        public void TestUTf8BufferForwardRead() {
            for (var bufferSize = 4; bufferSize < 20; bufferSize++) {
                var data = "1234567890";
                var bytes = Encoding.UTF8.GetBytes(data);
                var result = "";

                using (var stream = new MemoryStream(bytes))
                using (var source = new Utf8StreamBufferSource(stream, bufferSize, bufferSize))
                using (var buffer = new FileBuffer(source, bufferSize)) {

                    Assert.IsTrue(buffer.IsAtBeginning);
                    Assert.AreEqual(-1, buffer.Position);

                    while (!buffer.IsAtEnd) {
                        buffer.Position++;
                        result += buffer.Content[buffer.BufferIndex];
                    }

                    Assert.IsTrue(buffer.IsAtEnd);
                    Assert.AreEqual(data.Length - 1, buffer.Position);
                    Assert.AreEqual(data, result);
                }
            }
        }

        /// <summary>
        ///     test buffer backward reading
        /// </summary>
        [TestMethod]
        public void TestUtf8ForwardAndBackwardRead() {
            for (var bufferSize = 4; bufferSize < 20; bufferSize++) {
                var data = "1234567890";
                var bytes = Encoding.UTF8.GetBytes(data);
                using (var stream = new MemoryStream(bytes))
                using (var source = new Utf8StreamBufferSource(stream, bufferSize, bufferSize))
                using (var buffer = new FileBuffer(source, bufferSize)) {

                    var result1 = string.Empty;
                    var result2 = string.Empty;

                    Assert.IsTrue(buffer.IsAtBeginning);
                    Assert.AreEqual(-1, buffer.Position);

                    while (!buffer.IsAtEnd) {
                        buffer.Position++;
                        result1 += buffer.Content[buffer.BufferIndex];
                        buffer.Position--;
                        buffer.Position++;
                        result2 += buffer.Content[buffer.BufferIndex];
                    }

                    Assert.AreEqual(data.Length - 1, buffer.Position);
                    Assert.IsTrue(buffer.IsAtEnd);
                    Assert.AreEqual(data, result1);
                    Assert.AreEqual(data, result2);
                }
            }
        }

        /// <summary>
        ///     test buffer forward read
        /// </summary>
        [TestMethod]
        public void TestSimpleBufferForwardRead() {
            for (var bufferSize = 1; bufferSize < 20; bufferSize++) {
                var data = "1234567890";
                var result = "";

                using (var source = new StringBufferSource(data))
                using (var buffer = new FileBuffer(source, bufferSize)) {

                    Assert.IsTrue(buffer.IsAtBeginning);
                    Assert.AreEqual(-1, buffer.Position);

                    while (!buffer.IsAtEnd) {
                        buffer.Position++;
                        result += buffer.Content[buffer.BufferIndex];
                    }

                    Assert.AreEqual(data.Length - 1, buffer.Position);
                    Assert.IsTrue(buffer.IsAtEnd);
                    Assert.AreEqual(data, result);
                }
            }
        }

        /// <summary>
        ///     test forward and backward read
        /// </summary>
        [TestMethod]
        public void TestSimpleBufferForwardAndBackwardRead() {
            for (var bufferSize = 1; bufferSize < 20; bufferSize++) {
                var data = "1234567890";

                using (var source = new StringBufferSource(data))
                using (var buffer = new FileBuffer(source, bufferSize)) {
                    var result1 = string.Empty;
                    var result2 = string.Empty;

                    Assert.IsTrue(buffer.IsAtBeginning);
                    Assert.AreEqual(-1, buffer.Position);

                    while (!buffer.IsAtEnd) {
                        buffer.Position++;
                        result1 += buffer.Content[buffer.BufferIndex];
                        buffer.Position--;
                        buffer.Position++;
                        result2 += buffer.Content[buffer.BufferIndex];
                    }

                    Assert.AreEqual(data.Length - 1, buffer.Position);
                    Assert.IsTrue(buffer.IsAtEnd);
                    Assert.AreEqual(data, result1);
                    Assert.AreEqual(data, result2);
                }
            }
        }

        /// <summary>
        ///     test an utf8 stream
        /// </summary>
        [TestMethod]
        public void TestUtf8StreamSource() {
            for (var inputBufferSize = 4; inputBufferSize < 20; inputBufferSize++) {
                for (var outputBufferSize = 4; outputBufferSize < 20; outputBufferSize++) {
                    foreach (var data in utf8Samples) {
                        var bytes = Encoding.UTF8.GetBytes(data);
                        var read = new char[data.Length];
                        var chr = new char[outputBufferSize];

                        using (var stream = new MemoryStream(bytes))
                        using (var source = new Utf8StreamBufferSource(stream, inputBufferSize, outputBufferSize)) {
                            var idx = 0L;
                            while (idx < source.Length) {
                                var l = source.GetContent(chr, chr.Length, idx);
                                for (var i = 0; i < l; i++) {
                                    read[idx] = chr[i];
                                    idx++;
                                };

                                if (idx > 1) {
                                    source.GetContent(chr, chr.Length, idx - 2);
                                }
                                else if (idx > 0) {
                                    source.GetContent(chr, chr.Length, idx - 1);
                                }
                            }
                        }

                        Assert.AreEqual(data, new string(read));
                    }
                }
            }
        }

        /// <summary>
        ///     test utf8 samples
        /// </summary>
        [TestMethod]
        public void TestUtf8Samples() {
            for (var bufferSize = 4; bufferSize < 20; bufferSize++) {
                foreach (var data in utf8Samples) {
                    var bytes = Encoding.UTF8.GetBytes(data);

                    using (var stream = new MemoryStream(bytes))
                    using (var source = new Utf8StreamBufferSource(stream, bufferSize, bufferSize))
                    using (var buffer = new FileBuffer(source, bufferSize)) {

                        var result1 = string.Empty;
                        var result2 = string.Empty;

                        Assert.AreEqual(-1, buffer.Position);
                        Assert.IsTrue(buffer.IsAtBeginning);

                        while (!buffer.IsAtEnd) {
                            buffer.Position++;
                            result1 += buffer.Content[buffer.BufferIndex];
                            buffer.Position--;
                            buffer.Position++;
                            result2 += buffer.Content[buffer.BufferIndex];
                        }

                        Assert.IsTrue(buffer.IsAtEnd);
                        Assert.AreEqual(data.Length - 1, buffer.Position);
                        Assert.AreEqual(data, result1);
                        Assert.AreEqual(data, result2);
                    }
                }
            }
        }
    }
}
