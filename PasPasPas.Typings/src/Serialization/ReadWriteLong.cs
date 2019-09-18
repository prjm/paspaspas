using System;
using System.Runtime.InteropServices;

namespace PasPasPas.Typings.Serialization {
    internal partial class TypeReader {

        /// <summary>
        ///     read an unsigned integer
        /// </summary>
        /// <returns></returns>
        public long ReadLong() {
            Span<byte> data = stackalloc byte[sizeof(long)];

            var count = ReadableStream.Read(data);
            if (count != data.Length)
                throw new UnexpectedEndOfFileException();

            return MemoryMarshal.Read<long>(data);
        }
    }

    internal partial class TypeWriter {

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteLong(ref long value) {
            Span<byte> data = stackalloc byte[sizeof(long)];
            MemoryMarshal.Write(data, ref value);
            WritableStream.Write(data);
        }

    }
}
