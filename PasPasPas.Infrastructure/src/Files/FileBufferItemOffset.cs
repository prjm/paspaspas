namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     offset in a file buffer item
    /// </summary>
    public sealed class FileBufferItemOffset {

        private readonly Buffer input;
        private readonly StackedFileReader reader;
        private long offset;
        private readonly FileReference fileReference;

        /// <summary>
        ///     creates a new file buffer item offset
        /// </summary>
        /// <param name="inputFile">input file</param>
        /// <param name="file"></param>
        /// <param name="owner">owner reader</param>
        public FileBufferItemOffset(StackedFileReader owner, FileReference file, IBufferSource inputFile) {
            input = new Buffer(inputFile, 1024);
            reader = owner;
            offset = -1;
            fileReference = file;
        }

        /// <summary>
        ///     <c>true</c> if the end of read file is reached (EOF)
        /// </summary>
        public bool AtEof
            => offset >= input.Length - 1;

        /// <summary>
        ///     fetch the next char
        /// </summary>
        public char NextChar() {
            if (offset >= input.Length)
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
        public char Value
            => input.Content[input.BufferIndex];

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
        public FileReference File
            => fileReference;

        /// <summary>
        ///     current position
        /// </summary>
        public long Position
            => offset;

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

    }
}