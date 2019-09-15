using System;
using System.Text;

namespace PasPasPas.Typings.Serialization {

    internal partial class TypeReader {

        public string ReadString() {
            var len = ReadUint();

            if (len == 0)
                return string.Empty;

            if (len <= StringPool.MaximalStringLength)
                return ReadSmallString((int)len);
            else
                return ReadLongString(len);
        }

        private string ReadLongString(uint len) {
            var buffer = new byte[len * 2];
            var readLen = ReadableStream.Read(buffer);

            if (readLen != buffer.Length)
                throw new UnexpectedEndOfFileException();

            return Encoding.Unicode.GetString(buffer);
        }

        private string ReadSmallString(int len) {
            Span<byte> buffer = stackalloc byte[len * 2];
            var readLen = ReadableStream.Read(buffer);

            if (readLen != buffer.Length)
                throw new UnexpectedEndOfFileException();

            return StringPool.PoolString(buffer);
        }
    }

    internal partial class TypeWriter {

        public void WriteString(string text) {
            var len = (uint)text.Length;
            WriteUint(ref len);

            if (len <= StringPool.MaximalStringLength)
                WriteSmallString(text);
            else
                WriteLongString(text);
        }

        private void WriteSmallString(string text) {
            Span<byte> buffer = stackalloc byte[text.Length * 2];
            Encoding.Unicode.GetBytes(text, buffer);
            WritableStream.Write(buffer);
        }

        private void WriteLongString(string text) {
            var buffer = new byte[2 * text.Length];
            Encoding.Unicode.GetBytes(text, buffer);
            WritableStream.Write(buffer);
        }

    }

}
