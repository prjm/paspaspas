using System;
using System.Diagnostics;

namespace PasPasPas.Infrastructure.Files {

    [DebuggerDisplay("{Position}|->{Length}:{string.Join(\",\", Content)}")]
    internal class BufferData {
        internal int Length;
        internal long Position;
        internal char[] Content;

        public BufferData(int bufferSize)
            => Content = new char[bufferSize];
    }

    /// <summary>
    ///     variable-sized buffer to read char-based content
    /// </summary>
    public sealed class FileBuffer : IDisposable {

        private readonly IBufferSource source;
        private long position;
        private BufferData prev;
        private BufferData current;
        private BufferData next;

        /// <summary>
        ///     buffer content
        /// </summary>
        public char[] Content;

        /// <summary>
        ///     create a new buffer
        /// </summary>
        /// <param name="bufferSource">buffer source</param>
        /// <param name="bufferSize">buffer size</param>
        public FileBuffer(IBufferSource bufferSource, int bufferSize) {
            source = bufferSource ?? throw new ArgumentNullException(nameof(bufferSource));

            if (bufferSize < 1)
                throw new ArgumentException($"Invalid buffer size ${bufferSize}", nameof(bufferSize));

            BufferIndex = 0;
            prev = new BufferData(bufferSize);
            current = new BufferData(bufferSize);
            next = new BufferData(bufferSize);

            current.Length = source.GetContent(current.Content, 0);
            next.Position = current.Length;
            next.Length = source.GetContent(next.Content, next.Position);

            Content = current.Content;
        }

        /// <summary>
        ///     current char position
        /// </summary>
        public long Position {
            get => position;
            set {

                if (value < 0)
                    return;

                if (value > source.Length)
                    return;

                Seek(value - position);
            }
        }

        /// <summary>
        ///     seek to a specific position
        /// </summary>
        /// <param name="delta"></param>
        public void Seek(long delta) {
            if (delta == 0)
                return;

            var step = Math.Sign(delta);

            for (var offset = 0L; offset != delta && position >= 0 && position <= source.Length; offset += step) {
                position += step;
                BufferIndex += step;

                if (step > 0 && BufferIndex >= current.Length)
                    MoveToNextPart();

                else if (step < 0 && BufferIndex < 0)
                    MoveToPreviousPart();
            }
        }

        private void MoveToPreviousPart() {
            var dataToDiscard = next;
            next = current;
            current = prev;
            prev = dataToDiscard;
            prev.Length = source.GetContent(prev.Content, current.Position - current.Content.Length);
            prev.Position = current.Position - prev.Length;

            BufferIndex = current.Length - 1;
            Content = current.Content;
        }

        /// <summary>
        ///     move to the next buffer part
        /// </summary>
        public void MoveToNextPart() {
            var dataToDiscard = prev;
            prev = current;
            current = next;
            next = dataToDiscard;
            next.Position = current.Position + current.Length;
            next.Length = source.GetContent(next.Content, next.Position);

            BufferIndex = 0;
            Content = current.Content;
        }

        /// <summary>
        ///     check if the buffer is at the beginning of the input
        /// </summary>
        public bool IsAtBeginning
            => position <= 0;

        /// <summary>
        ///     check if the buffer read the end of input
        /// </summary>
        public bool IsAtEnd
            => position >= source.Length;

        /// <summary>
        ///     current buffer index
        /// </summary>
        public int BufferIndex { get; private set; }

        /// <summary>
        ///     file length
        /// </summary>
        public long Length
            => source.Length;

        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    source.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose the buffer and its source
        /// </summary>
        public void Dispose()
            => Dispose(true);
    }
}
