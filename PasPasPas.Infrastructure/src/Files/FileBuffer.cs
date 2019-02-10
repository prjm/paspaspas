using System;
using System.Buffers;
using System.Diagnostics;

namespace PasPasPas.Infrastructure.Files {

    [DebuggerDisplay("{Position}|->{Length}:{string.Join(\",\", Content)}")]
    internal sealed class BufferData : IDisposable {
        internal int Length;
        internal long Position;
        internal char[] Content;

        private bool disposedValue = false;

        public BufferData(int bufferSize)
            => Content = ArrayPool<char>.Shared.Rent(bufferSize);

        /// <summary>
        ///     dispose this file buffer and return the array to the pool
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    ArrayPool<char>.Shared.Return(Content, true);
                    Content = default;
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose this object
        /// </summary>
        public void Dispose() =>
            Dispose(true);
    }

    /// <summary>
    ///     variable-sized buffer to read char-based content
    /// </summary>
    public sealed class FileBuffer : IDisposable {

        private IBufferSource source;

        /// <summary>
        ///     buffer size
        /// </summary>
        public int BufferSize { get; }

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

            BufferSize = bufferSize;
            position = -1;
            BufferIndex = -1;
            prev = new BufferData(bufferSize);
            current = new BufferData(bufferSize);
            next = new BufferData(bufferSize);

            current.Length = source.GetContent(current.Content, bufferSize, 0);
            next.Position = current.Length;
            next.Length = source.GetContent(next.Content, bufferSize, next.Position);

            Content = current.Content;
        }

        /// <summary>
        ///     current char position
        /// </summary>
        public long Position {
            get => position;

            set {
                if (value >= -1 && value <= source.Length - 1)
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

            for (var offset = 0L; offset != delta && position >= -1 && position <= source.Length - 1; offset += step) {
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
            prev.Length = source.GetContent(prev.Content, BufferSize, current.Position - current.Content.Length);
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
            next.Length = source.GetContent(next.Content, BufferSize, next.Position);

            BufferIndex = 0;
            Content = current.Content;
        }

        /// <summary>
        ///     check if the buffer is at the beginning of the input
        /// </summary>
        public bool IsAtBeginning
            => position < 0;

        /// <summary>
        ///     check if the buffer read the end of input
        /// </summary>
        public bool IsAtEnd
            => position >= source.Length - 1;

        /// <summary>
        ///     current buffer index
        /// </summary>
        public int BufferIndex { get; private set; }

        /// <summary>
        ///     file length
        /// </summary>
        public long Length
            => source.Length;

        private bool disposedValue = false;

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    source.Dispose();
                    source = default;

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
        ///     dispose the buffer and its source
        /// </summary>
        public void Dispose()
            => Dispose(true);
    }
}
