using System;
using System.Runtime.InteropServices;

namespace PasPasPas.Typings.Serialization {

    internal partial class TypeReader {

        /// <summary>
        ///     read an unsigned integer
        /// </summary>
        /// <returns></returns>
        public ushort ReadUshort() {
            Span<byte> data = stackalloc byte[sizeof(ushort)];
            var count = ReadSpan(data);

            if (count != data.Length)
                throw new UnexpectedEndOfFileException();

            return MemoryMarshal.Read<ushort>(data);
        }
    }

    internal partial class TypeWriter {

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteUShort(ushort value) {
            Span<byte> data = stackalloc byte[sizeof(ushort)];
            MemoryMarshal.Write(data, ref value);
            WriteSpan(data);
        }

    }

}
