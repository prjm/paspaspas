using System;
using System.Runtime.InteropServices;

namespace PasPasPas.Typings.Serialization {

    internal partial class TypeReader {

        /// <summary>
        ///     read an unsigned long
        /// </summary>
        /// <returns></returns>
        public ulong ReadUlong() {
            Span<byte> data = stackalloc byte[sizeof(ulong)];
            var count = ReadSpan(data);

            if (count != data.Length)
                throw new UnexpectedEndOfFileException();

            return MemoryMarshal.Read<ulong>(data);
        }

    }

    internal partial class TypeWriter {

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteUlong(ulong value) {
            Span<byte> data = stackalloc byte[sizeof(ulong)];
            MemoryMarshal.Write(data, ref value);
            WriteSpan(data);
        }


    }
}
