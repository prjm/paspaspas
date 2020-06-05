#nullable disable
using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Text;
using PasPasPas.Globals.Files;

namespace PasPasPas.Infrastructure.Files {

    [DebuggerDisplay("{Position}:{StartIndex}-{Length} {string.Join(\", \", Content)}")]
    internal sealed class StreamData : IDisposable {
        internal int StartIndex;
        internal int Length;
        internal long Position;
        internal byte[] Content;

        public StreamData(int bufferSize)
            => Content = ArrayPool<byte>.Shared.Rent(bufferSize);

        private bool disposedValue = false;
        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    ArrayPool<byte>.Shared.Return(Content);
                    Content = null;
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose this resource
        /// </summary>
        public void Dispose()
            => Dispose(true);
    }

    /// <summary>
    ///     a buffered utf-8 stream source
    /// </summary>
    public sealed class Utf8StreamBufferSource : IBufferSource {

        private Stream input;
        private readonly int outputSize;
        private long charPosition;
        private int bufferIndex;
        private readonly long inputLength;
        private StreamData prev;
        private StreamData current;
        private StreamData next;
        private long length;
        private bool disposedValue = false;

        /// <summary>
        ///     create a new stream buffer source
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="inputBufferSize"></param>
        /// <param name="outputBufferSize"></param>
        public Utf8StreamBufferSource(Stream inputStream, int inputBufferSize, int outputBufferSize) {
            input = inputStream ?? throw new System.ArgumentNullException(nameof(inputStream));
            inputLength = input.Length;
            outputSize = outputBufferSize;
            var size = inputBufferSize - inputBufferSize % 4;

            if (size < 4)
                throw new ArgumentOutOfRangeException(nameof(inputBufferSize), inputBufferSize, $"Invalid input buffer size {inputBufferSize}");

            if (outputSize < 2)
                throw new ArgumentOutOfRangeException(nameof(outputBufferSize), outputBufferSize, $"Invalid output buffer size {outputBufferSize}");

            prev = new StreamData(size);
            current = new StreamData(size);
            next = new StreamData(size);
            charPosition = 0L;
            bufferIndex = 0;
            length = long.MaxValue;

            GetBytes(current, 0);
            GetBytes(next, current.Length);
        }

        private void GetBytes(StreamData data, long offset) {
            var index = (int)Math.Max(0, 0 - offset);
            input.Seek(offset + index, SeekOrigin.Begin);
            data.StartIndex = index;
            data.Position = offset;
            data.Length = input.Read(data.Content, index, data.Content.Length - index);

            var count = 0;
            index = 0;
            while ((data.Content[index] & 0xC0) == 0x80 && index <= data.Length)
                index++;
            data.StartIndex += index;

            while (count < outputSize && index < data.Content.Length && index + offset < inputLength) {

                count++;

                if (data.Content[index] < 0xC0)
                    index++;
                else if (data.Content[index] < 0xE0)
                    index += 2;
                else if (data.Content[index] < 0xF0)
                    index += 3;
                else
                    index += 4;

                if (index < data.Content.Length)
                    data.Length = index - data.StartIndex;
            }
        }

        private void MoveToPreviousPart() {
            var dataToDiscard = next;
            next = current;
            current = prev;
            prev = dataToDiscard;
            GetBytes(prev, current.Position + current.StartIndex - current.Content.Length);
            bufferIndex = current.Length - 1;
        }

        /// <summary>
        ///     move to the next buffer part
        /// </summary>
        public void MoveToNextPart() {
            var dataToDiscard = prev;
            prev = current;
            current = next;
            next = dataToDiscard;
            GetBytes(next, current.Position + current.StartIndex + current.Length);
            bufferIndex = current.StartIndex;
        }

        /// <summary>
        ///     file length
        /// </summary>
        public long Length
            => length;

        /// <summary>
        ///     get a number of chars from the input file
        /// </summary>
        /// <param name="target"></param>
        /// <param name="offset"></param>
        /// <param name="bufferSize">buffer size</param>
        /// <returns></returns>
        public int GetContent(char[] target, int bufferSize, long offset) {
            if (offset >= length || offset < 0)
                return 0;

            MoveToCharOffset(offset);

            if (offset >= length)
                return 0;

            return Encoding.UTF8.GetChars(current.Content, current.StartIndex, current.Length, target, 0);
        }

        private void MoveToCharOffset(long offset) {
            var step = Math.Sign(offset - charPosition);
            offset = Math.Max(0, Math.Min(Length, offset));

            while (charPosition != offset) {

                if (bufferIndex + 1 >= current.Length && current.Position + current.Length >= inputLength) {
                    length = charPosition + 1;
                    break;
                }

                bufferIndex += step;

                if (step > 0 && bufferIndex >= current.Length)
                    MoveToNextPart();

                else if (step < 0 && bufferIndex < current.StartIndex)
                    MoveToPreviousPart();

                if (current.Length < 1)
                    break;

                if ((current.Content[bufferIndex] & 0xC0) != 0x80)
                    charPosition += step;
            }
        }

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    input.Dispose();
                    input = default;

                    prev.Dispose();
                    prev = default;

                    current.Dispose();
                    current = default;

                    next.Dispose();
                    next = default;
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose stream source
        /// </summary>
        public void Dispose()
            => Dispose(true);


    }
}
