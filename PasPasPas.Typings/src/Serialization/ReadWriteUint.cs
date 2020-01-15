using System;
using System.Runtime.InteropServices;

namespace PasPasPas.Typings.Serialization {

    internal partial class TypeReader {

        /// <summary>
        ///     read an unsigned integer
        /// </summary>
        /// <returns></returns>
        public uint ReadUint() {
            Span<byte> data = stackalloc byte[sizeof(uint)];
            var count = ReadSpan(data);

            if (count != data.Length)
                throw new UnexpectedEndOfFileException();

            return MemoryMarshal.Read<uint>(data);
        }

        /// <summary>
        ///     read a signed integer
        /// </summary>
        /// <returns></returns>
        public int ReadInt() {
            Span<byte> data = stackalloc byte[sizeof(int)];
            var count = ReadSpan(data);

            if (count != data.Length)
                throw new UnexpectedEndOfFileException();

            return MemoryMarshal.Read<int>(data);
        }
    }

    internal partial class TypeWriter {

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteUint(uint value) {
            Span<byte> data = stackalloc byte[sizeof(uint)];
            MemoryMarshal.Write(data, ref value);
            WriteSpan(data);
        }


        /// <summary>
        ///     write a signed integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt(int value) {
            Span<byte> data = stackalloc byte[sizeof(int)];
            MemoryMarshal.Write(data, ref value);
            WriteSpan(data);
        }

    }
}
