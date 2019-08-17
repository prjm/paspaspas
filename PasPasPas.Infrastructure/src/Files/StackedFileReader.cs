using System;
using PasPasPas.Globals.Api;
using PasPasPas.Globals.Files;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     read from a combination of text files
    /// </summary>
    internal class StackedFileReader : IDisposable, IStackedFileReader {

        /// <summary>
        ///     helper class for nested input
        /// </summary>
        private class NestedInput {
            internal FileReference File;
            internal FileBuffer Input;
            internal NestedInput Parent;
        }

        private NestedInput input = null;

        public StackedFileReader(IReaderApi api, IInputResolver resolver) {
            Api = api;
            Resolver = resolver;
        }

        /// <summary>
        ///     add a string to read
        /// </summary>
        /// <param name="inputToRead">input source</param>
        public void AddInputToRead(FileReference inputToRead) {
            if (inputToRead.Path == null)
                throw new ArgumentException("", nameof(inputToRead));

            var data = Resolver.Resolve(Api, inputToRead);

            input = new NestedInput() {
                File = inputToRead,
                Input = new FileBuffer(data.CreateBufferSource(), 2 * data.BufferSize),
                Parent = input
            };
        }

        /// <summary>
        ///     finish current file
        /// </summary>
        public FileReference FinishCurrentFile() {
            var source = input?.Input;

            if (source == null)
                throw new InvalidOperationException("No input file.");

            source.Dispose();
            input.Input = null;
            input = input.Parent;

            return input?.File;
        }

        /// <summary>
        ///     access the current file
        /// </summary>
        public FileReference CurrentFile
            => input?.File;

        /// <summary>
        ///     get the current character value
        /// </summary>
        public char Value {
            get {
                if (input == null || input.Input == null)
                    throw new InvalidOperationException("No input file.");

                var source = input.Input;
                var position = source.Position;

                if (position < 0 || position >= source.Length)
                    return '\0';

                return source.Content[source.BufferIndex];
            }
        }

        /// <summary>
        ///     check if the reader has reached eof
        /// </summary>
        public bool AtEof {
            get {
                var source = input?.Input;

                if (source == null)
                    throw new InvalidOperationException("No input file.");

                return source.IsAtEnd;
            }
        }

        /// <summary>
        ///     look ahead one character
        /// </summary>
        ///
        public char LookAhead(int number) {
            var source = input?.Input;

            if (source == null)
                throw new InvalidOperationException("No input file.");

            if (source.Position + number < -1 || source.Position + number >= source.Length)
                return '\0';

            source.Position += number;
            var result = source.Content[source.BufferIndex];
            source.Position -= number;
            return result;
        }

        /// <summary>
        ///     move to the previous char
        /// </summary>
        public char PreviousChar() {
            var source = input?.Input;

            if (source == null)
                throw new InvalidOperationException("No input file.");

            var sourcePosition = source.Position;

            if (sourcePosition < 0)
                return '\0';

            source.Position = sourcePosition - 1;
            var bufferIndex = source.BufferIndex;

            if (bufferIndex < 0)
                return '\0';

            return source.Content[bufferIndex];
        }

        /// <summary>
        ///     current position
        /// </summary>
        public long Position
            => input != null ? input.Input.Position : -1;

        public IReaderApi Api { get; }

        /// <summary>
        ///     resolver
        /// </summary>
        public IInputResolver Resolver { get; }

        /// <summary>
        ///     fetch the next char
        /// </summary>
        public char NextChar() {
            var source = input?.Input;

            if (source == null)
                throw new InvalidOperationException("No input file.");

            var sourcePosition = source.Position;

            if (sourcePosition >= source.Length)
                return '\0';

            source.Position = 1 + sourcePosition;
            return source.Content[source.BufferIndex];
        }

        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    while (input != null) {
                        input.Input.Dispose();
                        input = input.Parent;
                    }
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose this stacked file reader
        /// </summary>
        public void Dispose() =>
            Dispose(true);

    }
}
