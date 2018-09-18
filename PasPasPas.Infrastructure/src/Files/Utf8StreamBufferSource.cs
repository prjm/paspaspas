using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PasPasPas.Infrastructure.Files {

    [DebuggerDisplay("{Position}:{StartIndex}-{Length} {string.Join(\", \", Content)}")]
    internal class StreamData {
        internal int StartIndex;
        internal int Length;
        internal long Position;
        internal byte[] Content;

        public StreamData(int bufferSize)
            => Content = new byte[bufferSize];
    }

    /// <summary>
    ///     a buffered utf-8 stream source
    /// </summary>
    public class Utf8StreamBufferSource : IBufferSource, IDisposable {

        private readonly Stream input;
        private readonly int outputSize;
        private long charPosition;
        private int bufferIndex;
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
            outputSize = outputBufferSize;
            var size = inputBufferSize - (inputBufferSize % 4);

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

            while (data.Length >= 0 && data.Length > outputSize * 2)
                data.Length--;

            index = data.Length - 1;
            while (index >= 0 && (data.Content[index] & 0xC0) == 0x80)
                index--;

            if (index >= 0 && (data.Content[index] & 0xF0) == 0xF0 && index + 4 > data.Length)
                data.Length = index;
            else if (index >= 0 && (data.Content[index] & 0xE0) == 0xE0 && index + 3 > data.Length)
                data.Length = index;
            else if (index >= 0 && (data.Content[index] & 0xC0) == 0xC0 && index + 2 > data.Length)
                data.Length = index;

            index = 0;
            while ((data.Content[index] & 0xC0) == 0x80 && index <= data.Length)
                index++;

            data.StartIndex += index;
            data.Length -= index;
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

        public long Length
            => length;

        public int GetContent(char[] target, long offset) {
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

                if (bufferIndex + 1 >= current.Length && current.Position + current.Length >= input.Length) {
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

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    input.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
