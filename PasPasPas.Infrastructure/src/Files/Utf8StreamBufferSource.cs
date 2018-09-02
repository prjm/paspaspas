using System;
using System.IO;
using System.Text;

namespace PasPasPas.Infrastructure.Files {

    internal class StreamData {
        internal int Length;
        internal byte[] Content;

        public StreamData(int bufferSize)
            => Content = new byte[bufferSize];
    }

    public class Utf8StreamBufferSource : IBufferSource, IDisposable {

        private readonly Stream input;
        private long position;
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
        /// <param name="bufferSize"></param>
        public Utf8StreamBufferSource(Stream inputStream, int bufferSize) {
            input = inputStream ?? throw new System.ArgumentNullException(nameof(inputStream));
            prev = new StreamData(bufferSize);
            current = new StreamData(bufferSize);
            next = new StreamData(bufferSize);
            position = 0L;
            charPosition = 0L;
            bufferIndex = 0;
            length = long.MaxValue;

            GetBytes(current, 0);
            GetBytes(next, current.Length);
        }

        private void GetBytes(StreamData data, long offset) {
            if (offset >= 0 && offset < input.Length) {
                input.Seek(offset, SeekOrigin.Begin);
                data.Length = input.Read(data.Content, 0, data.Content.Length);
                if (data.Length < data.Content.Length)
                    Array.Clear(data.Content, data.Length, data.Content.Length - data.Length);
            }
            else {
                data.Length = 0;
                Array.Clear(data.Content, 0, data.Length);
            }
        }

        private void MoveToPreviousPart() {
            var dataToDiscard = next;
            next = current;
            current = prev;
            prev = dataToDiscard;
            GetBytes(prev, position - current.Length);

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
            GetBytes(next, position + current.Length);
            bufferIndex = 0;
        }

        public long Length
            => length;

        public int GetContent(char[] target, long offset) {
            MoveToCharOffset(offset);

            if (charPosition != offset && position < input.Length)
                throw new InvalidOperationException();

            return Encoding.UTF8.GetChars(current.Content, 0, current.Length, target, 0);
        }

        private void MoveToCharOffset(long offset) {
            var step = Math.Sign(offset - charPosition);

            while (charPosition != offset || (step < 0 && current.Content[bufferIndex] > 127)) {
                position += step;
                bufferIndex += step;

                if (step > 0 && bufferIndex >= current.Length)
                    MoveToNextPart();

                else if (step < 0 && bufferIndex < 0)
                    MoveToPreviousPart();

                if (current.Content[bufferIndex] <= 127)
                    charPosition += step;

                if (position >= input.Length) {
                    length = charPosition;
                    break;
                }
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
