using System;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     read from a combiniation of textfiles
    /// </summary>
    public sealed class StackedFileReader {

        /// <summary>
        ///     helper class for nested input
        /// </summary>
        private class NestedInput {
            internal FileBufferItemOffset Input;
            internal NestedInput Parent;
        }

        private NestedInput input = null;
        private readonly FileBuffer buffer;

        /// <summary>
        ///     create a new stacked file reader
        /// </summary>
        /// <param name="fileBuffer">file buffer</param>
        public StackedFileReader(FileBuffer fileBuffer)
            => buffer = fileBuffer ?? throw new ArgumentNullException(nameof(fileBuffer));

        /// <summary>
        ///     adds a file to read
        /// </summary>
        /// <param name="input">input to add</param>
        public void AddFileToRead(IFileReference input) {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            this.input = new NestedInput() {
                Input = new FileBufferItemOffset(this, buffer[input]),
                Parent = this.input
            };
        }

        /// <summary>
        ///     finish current file
        /// </summary>
        public FileBufferItemOffset FinishCurrentFile() {

            if (input == null)
                throw new InvalidOperationException("No input file.");

            input = input.Parent;

            if (input == null)
                return null;
            else
                return input.Input;
        }

        /// <summary>
        ///     file buffer
        /// </summary>
        public FileBuffer Buffer
            => buffer;

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
        public int Position
            => input != null ? input.Input.Position : -1;

        /// <summary>
        ///     fetch the ext char
        /// </summary>
        public char NextChar() {
            if (input == null)
                throw new InvalidOperationException("No input file.");

            return input.Input.NextChar();
        }
    }
}
