#nullable disable
namespace PasPasPas.Typings.Serialization {
    internal partial class TypeReader {

        /// <summary>
        ///     read an unsigned integer
        /// </summary>
        /// <returns></returns>
        public byte ReadByte() {
            var result = ReadableStream.ReadByte();

            if (result == -1)
                throw new UnexpectedEndOfFileException();

            return (byte)result;
        }
    }

    internal partial class TypeWriter {

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteByte(byte value)
            => WritableStream.WriteByte(value);

    }
}
