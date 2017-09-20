using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     helper class for nested input
    /// </summary>
    internal class NestedInput {
        internal FileBufferItemOffset Input;
        internal NestedInput Parent;
    }

    /// <summary>
    ///     read from a combiniation of textfiles
    /// </summary>
    public class StackedFileReader {

        private NestedInput input = null;
        private readonly FileBuffer buffer;

        /// <summary>
        ///     create a new stacked file reader
        /// </summary>
        /// <param name="fileBuffer">file buffer</param>
        public StackedFileReader(FileBuffer fileBuffer) {
            if (fileBuffer == null)
                ExceptionHelper.ArgumentIsNull(nameof(fileBuffer));

            buffer = fileBuffer;
        }

        /// <summary>
        ///     adds a file to read
        /// </summary>
        /// <param name="input">input to add</param>
        public void AddFileToRead(IFileReference input) {
            if (input == null)
                ExceptionHelper.ArgumentIsNull(nameof(input));

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
                ExceptionHelper.InvalidOperation();

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

                ExceptionHelper.InvalidOperation();
                return '\0';
            }
        }

        /// <summary>
        ///     check if the reader has reached eof
        /// </summary>
        public bool AtEof {
            get {
                if (input != null)
                    return input.Input.AtEof;

                ExceptionHelper.InvalidOperation();
                return false;
            }
        }

        /// <summary>
        ///     move to the previous char
        /// </summary>
        public char PreviousChar() {
            if (input == null)
                ExceptionHelper.InvalidOperation();

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
                ExceptionHelper.InvalidOperation();
            return input.Input.NextChar();
        }
    }
}
