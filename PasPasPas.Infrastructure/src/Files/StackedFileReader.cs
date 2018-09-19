using System;
using System.IO;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     read from a combination of text files
    /// </summary>
    public sealed class StackedFileReader : IDisposable {

        /// <summary>
        ///     helper class for nested input
        /// </summary>
        private class NestedInput {
            internal FileBufferItemOffset Input;
            internal NestedInput Parent;
        }

        private NestedInput input = null;

        /// <summary>
        ///     adds a file to read
        /// </summary>
        /// <param name="input">input to add</param>
        public void AddFileToRead(FileReference input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var bufferSize = 1024;
            var stream = new FileStream(input.Path, FileMode.Open, FileAccess.Read, FileShare.Read);

            this.input = new NestedInput() {
                Input = new FileBufferItemOffset(this, input, new Utf8StreamBufferSource(stream, bufferSize, bufferSize)),
                Parent = this.input
            };
        }

        /// <summary>
        ///     add a string to read
        /// </summary>
        /// <param name="path"></param>
        /// <param name="source"></param>
        public void AddStringToRead(FileReference path, string source) {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            input = new NestedInput() {
                Input = new FileBufferItemOffset(this, path, new StringBufferSource(source)),
                Parent = input
            };

        }

        /// <summary>
        ///     finish current file
        /// </summary>
        public FileBufferItemOffset FinishCurrentFile() {

            if (input == null)
                throw new InvalidOperationException("No input file.");

            input.Input.Dispose();

            input = input.Parent;

            if (input == null)
                return null;
            else
                return input.Input;
        }

        /// <summary>
        ///     access the current file
        /// </summary>
        public FileBufferItemOffset CurrentFile
            => input?.Input;

        /// <summary>
        ///     get the current character value
        /// </summary>
        public char Value {
            get {
                if (input != null)
                    return input.Input.Value;

                throw new InvalidOperationException("No input file.");
            }
        }

        /// <summary>
        ///     check if the reader has reached eof
        /// </summary>
        public bool AtEof {
            get {
                if (input != null)
                    return input.Input.AtEof;

                throw new InvalidOperationException("No input file");
            }
        }

        /// <summary>
        ///     look ahead one character
        /// </summary>
        public char LookAhead(int number)
            => input.Input.LookAhead(number);

        /// <summary>
        ///     move to the previous char
        /// </summary>
        public char PreviousChar() {
            if (input == null)
                throw new InvalidOperationException("No input file.");

            return input.Input.PreviousChar();
        }

        /// <summary>
        ///     current position
        /// </summary>
        public long Position
            => input != null ? input.Input.Position : -1;

        /// <summary>
        ///     fetch the ext char
        /// </summary>
        public char NextChar() {
            if (input == null)
                throw new InvalidOperationException("No input file.");

            var value = input.Input.Value;
            input.Input.NextChar();
            return value;
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
