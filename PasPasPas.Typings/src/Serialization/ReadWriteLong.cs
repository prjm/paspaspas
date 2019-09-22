using System;
using System.Runtime.InteropServices;

#if DESKTOP
using PasPasPas.Desktop.BackwardCompatibility;
#endif

namespace PasPasPas.Typings.Serialization {
    internal partial class TypeReader {

        private int ReadSpan(in Span<byte> data)
            => ReadableStream.Read(data);

        /// <summary>
        ///     read an unsigned integer
        /// </summary>
        /// <returns></returns>
        public long ReadLong() {
            Span<byte> data = stackalloc byte[sizeof(long)];
            var count = ReadSpan(data);

            if (count != data.Length)
                throw new UnexpectedEndOfFileException();

            return MemoryMarshal.Read<long>(data);
        }
    }

    internal partial class TypeWriter {

        private void WriteSpan(Span<byte> data)
           => WritableStream.Write(data);

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
