#nullable disable
using System;
using System.Collections.Immutable;
using System.IO;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     read from an immutable byte array
    /// </summary>
    public class ImmutableByteArrayStream : Stream {

        private readonly ImmutableArray<byte> input;
        private int position;

        /// <summary>
        ///     create a new reader
        /// </summary>
        /// <param name="inputData"></param>
        public ImmutableByteArrayStream(ImmutableArray<byte> inputData)
           => input = inputData;

        /// <summary>
        ///     can always read
        /// </summary>
        public override bool CanRead
            => true;

        /// <summary>
        ///     can always seek
        /// </summary>
        public override bool CanSeek
            => true;

        /// <summary>
        ///     can never write
        /// </summary>
        public override bool CanWrite
            => false;

        /// <summary>
        ///     input length
        /// </summary>
        public override long Length
            => input.Length;

        /// <summary>
        ///     stream position
        /// </summary>
        public override long Position {
            get => position;

            set {
                if (value < 0 || value >= input.Length)
                    throw new ArgumentOutOfRangeException(nameof(value));
                position = (int)value;
            }
        }

        /// <summary>
        ///     doest nothing
        /// </summary>
        public override void Flush() { }

        /// <summary>
        ///     read byte from the input array
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count) {
            var len = Math.Min(count, input.Length - position);
            input.CopyTo(position, buffer, offset, len);
            position += len;
            return len;
        }

        /// <summary>
        ///     seek to a position
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin) {
            var newPosition = 0L;

            switch (origin) {
                case SeekOrigin.Begin:
                    newPosition = offset;
                    break;

                case SeekOrigin.Current:
                    newPosition = checked(position + offset);
                    break;

                case SeekOrigin.End:
                    newPosition = checked(offset + input.Length);
                    break;

            }

            Position = newPosition;
            return newPosition;
        }

        /// <summary>
        ///     unsupported
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
            => throw new NotSupportedException();

        /// <summary>
        ///     unsupported
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count) => throw new System.NotImplementedException();
    }
}
