using System;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     offset in a file buffer item
    /// </summary>
    public sealed class FileBufferItemOffset : IDisposable {

        private readonly FileBuffer input;
        private readonly StackedFileReader reader;
        private readonly FileReference fileReference;

        /// <summary>
        ///     creates a new file buffer item offset
        /// </summary>
        /// <param name="inputFile">input file</param>
        /// <param name="file"></param>
        /// <param name="owner">owner reader</param>
        public FileBufferItemOffset(StackedFileReader owner, FileReference file, IBufferSource inputFile) {
            input = new FileBuffer(inputFile, 1024);
            reader = owner;
            fileReference = file;
        }

        /// <summary>
        ///     <c>true</c> if the end of read file is reached (EOF)
        /// </summary>
        public bool AtEof
            => input.Position >= input.Length;

        /// <summary>
        ///     fetch the next char
        /// </summary>
        public char NextChar() {
            if (input.Position >= input.Length)
                return '\0';
            input.Position++;
            return Value;
        }

        /// <summary>
        ///     <c>true</c> if the begin of the read file is reached (BOF)
        /// </summary>
        public bool AtBof
            => input.Position < 0;

        /// <summary>
        ///     current reader value
        /// </summary>
        public char Value
            => input.Content[input.BufferIndex];

        /// <summary>
        ///     navigate to the previous char
        /// </summary>
        public char PreviousChar() {
            if (input.Position < 0)
                return '\0';
            input.Position--;
            return Value;
        }

        /// <summary>
        ///     file name
        /// </summary>
        public FileReference File
            => fileReference;

        /// <summary>
        ///     current position
        /// </summary>
        public long Position
            => input.Position;

        /// <summary>
        ///     look ahead one character
        /// </summary>
        /// <returns></returns>
        public char LookAhead(int number) {
            input.Position += number;
            var result = input.Content[input.BufferIndex];
            input.Position -= number;
            return result;
        }

        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    input.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose this stream
        /// </summary>
        public void Dispose()
            => Dispose(true);

    }
}