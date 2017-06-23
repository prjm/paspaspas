﻿using PasPasPas.DesktopPlatform;
using System.IO;
using System.Text;
using Xunit;
using PasPasPas.Api;

namespace PasPasPasTests.Infra {

    /// <summary>
    ///     test for stacked reader
    /// </summary>
    public class StackedReaderTest {

        private string Content1
            => "X1X1X1|||X1X1X1------------$";

        private string Content2
            => "ZZZ$44345845784875DSDFDDFDS";

        [Fact]
        public void TestSimpleRead() {

            var readerApi = new ReaderApi(new StandardFileAccess());
            var reader = readerApi.CreateReaderForString("test.pas", Content1);
            var result = new StringBuilder();

            while (!reader.AtEof) {
                result.Append(reader.Value);
                reader.NextChar();
            }

            Assert.AreEqual(Content1, result.ToString());
        }


        [Fact]
        public void TestSimpleFileRead() {

            var path = GenerateTempFile(Content1);
            var readerApi = new ReaderApi(new StandardFileAccess());
            var reader = readerApi.CreateReaderForPath(path);
            var result = new StringBuilder();

            try {
                while (!reader.CurrentFile.AtEof) {
                    result.Append(reader.CurrentFile.Value);
                    reader.CurrentFile.NextChar();
                }
                Assert.AreEqual(Content1, result.ToString());
            }
            finally {
                File.Delete(path);
            }
        }

        [Fact]
        public void TestStackedRead() {

            var splitIndex = 5;
            var result = new StringBuilder();
            var path1 = GenerateTempFile(Content1);
            var path2 = GenerateTempFile(Content2);
            var readerApi = new ReaderApi(new StandardFileAccess());
            var reader = readerApi.CreateReaderForPath(path1);

            try {
                while (!reader.AtEof && result.Length < splitIndex) {
                    result.Append(reader.Value);
                    reader.NextChar();
                }
                var len = splitIndex;
                readerApi.SwitchToPath(reader, path2);
                while (reader.CurrentFile != null && !reader.AtEof) {
                    result.Append(reader.Value);
                    reader.NextChar();
                    len++;

                    if (len == (Content2.Length + splitIndex) || len == (Content1.Length + Content2.Length))
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

        [Fact]
        public void TestNestedRead() {

            var result = new StringBuilder();
            var path1 = GenerateTempFile(Content1);
            var path2 = GenerateTempFile(Content2);
            var readerApi = new ReaderApi(new StandardFileAccess());
            var reader = readerApi.CreateReaderForPath(path1);

            while (!reader.AtEof && result.Length < 5) {
                result.Append(reader.Value);
                reader.NextChar();
            }

            readerApi.SwitchToPath(reader, path2);
            while (reader.CurrentFile != null && !reader.AtEof) {
                result.Append(reader.Value);
                reader.NextChar();

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
