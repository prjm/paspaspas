using System;
using System.Runtime.InteropServices;
#if DESKTOP
using PasPasPas.Globals.Environment;
#endif

namespace PasPasPas.Typings.Serialization {
    internal partial class TypeReader {

        private int ReadSpan(in Span<byte> data, int len)
#if DESKTOP
            => ReadableStream.ReadSpan(data, len);
#else
            => ReadableStream.Read(data);
#endif

        /// <summary>
        ///     read an unsigned integer
        /// </summary>
        /// <returns></returns>
        public long ReadLong() {
            Span<byte> data = stackalloc byte[sizeof(long)];
            var count = ReadSpan(data, sizeof(long));

            if (count != data.Length)
                throw new UnexpectedEndOfFileException();

            return MemoryMarshal.Read<long>(data);
        }
    }

    internal partial class TypeWriter {


        private void WriteSpan(Span<byte> data)
#if DESKTOP
            => WritableStream.WriteSpan(data);
#else
            => WritableStream.Write(data);
#endif

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteLong(ref long value) {
            Span<byte> data = stackalloc byte[sizeof(long)];
            MemoryMarshal.Write(data, ref value);
            WriteSpan(data);
        }

    }
}
