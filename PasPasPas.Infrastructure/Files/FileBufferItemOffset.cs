namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     offset in a file buffer item
    /// </summary>
    public sealed class FileBufferItemOffset {

        private readonly FileBufferItem input;
        private readonly StackedFileReader reader;
        private readonly int length;
        private int offset;

        /// <summary>
        ///     creates a new file buffer item offset
        /// </summary>
        /// <param name="inputFile">input file</param>
        /// <param name="owner">owner reader</param>
        public FileBufferItemOffset(StackedFileReader owner, FileBufferItem inputFile) {
            input = inputFile;
            length = input.Length;
            reader = owner;
            offset = -1;
        }

        /// <summary>
        ///     <c>true</c> if the end of read file is reached (EOF)
        /// </summary>
        public bool AtEof
            => offset >= length - 1;

        /// <summary>
        ///     fetch the next char
        /// </summary>
        public char NextChar() {
            if (offset >= length)
                return '\0';
            offset++;
            return Value;
        }

        /// <summary>
        ///     <c>true</c> if the begin of the read file is reached (BOF)
        /// </summary>
        public bool AtBof
            => offset < 0;

        /// <summary>
        ///     current reader value
        /// </summary>
        public char Value {
            get {
                if (offset < 0 || offset >= length)
                    return '\0';
                return input.CharAt(offset);
            }
        }

        /// <summary>
        ///     navigate to the previous char
        /// </summary>
        public char PreviousChar() {
            if (offset < 0)
                return '\0';
            offset--;
            return Value;
        }

        /// <summary>
        ///     file name
        /// </summary>
        public IFileReference File
            => input.File;

        /// <summary>
        ///     current position
        /// </summary>
        public int Position
            => offset;

        /// <summary>
        ///     look ahead one character
        /// </summary>
        /// <returns></returns>
        public char LookAhead(int number) {
            var position = offset + number;
            if (position < 0 || position >= length)
                return '\0';
            return input.CharAt(position);
        }

    }
}