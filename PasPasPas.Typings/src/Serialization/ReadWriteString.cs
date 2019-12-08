using System;
using System.Buffers;
using System.Text;

#if DESKTOP
using PasPasPas.Desktop.BackwardCompatibility;
#endif

namespace PasPasPas.Typings.Serialization {

    internal partial class TypeReader {

        public string ReadString() {
            var len = ReadUint();

            if (len == 0)
                return string.Empty;

            if (len <= StringPool.MaximalStringLength)
                return ReadSmallString((int)len);
            else
                return ReadLongString((int)len);
        }

        private string ReadLongString(int len) {
            var buffer = ArrayPool<byte>.Shared.Rent(len * 2);
            try {
                var readLen = ReadableStream.Read(buffer, 0, len * 2);

                if (readLen != 2 * len)
                    throw new UnexpectedEndOfFileException();

                return Encoding.Unicode.GetString(buffer, 0, len * 2);
            }
            finally {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        private string ReadSmallString(int len) {
            Span<byte> buffer = stackalloc byte[len * 2];
            var readLen = ReadSpan(buffer);

            if (readLen != buffer.Length)
                throw new UnexpectedEndOfFileException();

            return StringPool.PoolString(buffer);
        }
    }

    internal partial class TypeWriter {


        private static void GetBytes(string text, Span<byte> buffer)
            => Encoding.Unicode.GetBytes(text, buffer);

        private static void GetBytes(string text, byte[] buffer)
            => Encoding.Unicode.GetBytes(text, buffer);

        public void WriteString(string text) {
            var len = (uint)text.Length;
            WriteUint(len);

            if (len <= StringPool.MaximalStringLength)
                WriteSmallString(text);
            else
                WriteLongString(text);
        }

        private void WriteSmallString(string text) {
            Span<byte> buffer = stackalloc byte[text.Length * 2];
            GetBytes(text, buffer);
            WriteSpan(buffer);
        }

        private void WriteLongString(string text) {
            var buffer = ArrayPool<byte>.Shared.Rent(2 * text.Length);
            try {
                GetBytes(text, buffer);
                WritableStream.Write(buffer, 0, 2 * text.Length);
            }
            finally {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

    }

}
