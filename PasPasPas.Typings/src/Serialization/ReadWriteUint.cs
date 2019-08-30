using System;
using System.Runtime.InteropServices;

namespace PasPasPas.Typings.Serialization {
    internal partial class TypeReader {

        /// <summary>
        ///     read an unsigned integer
        /// </summary>
        /// <returns></returns>
        public uint ReadUint() {
            Span<byte> data = stackalloc byte[4];
            ReadableStream.Read(data);
            return MemoryMarshal.Read<uint>(data);
        }
    }

    internal partial class TypeWriter {

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteUint(ref uint value) {
            Span<byte> data = stackalloc byte[4];
            MemoryMarshal.Write(data, ref value);
            WritableStream.Write(data);
        }

    }
}
