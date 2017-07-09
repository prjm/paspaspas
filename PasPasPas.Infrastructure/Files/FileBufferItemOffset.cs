using System;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     offset in a file buffer item
    /// </summary>
    public class FileBufferItemOffset {

        private readonly FileBufferItem input;
        private readonly StackedFileReader reader;
        private int offset;

        /// <summary>
        ///     creates a new file buffer item offset
        /// </summary>
        /// <param name="inputFile">input file</param>
        /// <param name="owner">owner reader</param>
        public FileBufferItemOffset(StackedFileReader owner, FileBufferItem inputFile) {
            input = inputFile;
            reader = owner;
            offset = 0;
        }

        /// <summary>
        ///     <c>true</c> if the end of read file is reached (EOF)
        /// </summary>
        public bool AtEof
            => offset >= input.Length;

        /// <summary>
        ///     fetch the next char
        /// </summary>
        public void NextChar() {
            if (!AtEof) offset++;
        }

        /// <summary>
        ///     <c>true</c> if the begin of the read file is reached (BOF)
        /// </summary>
        public bool AtBof
            => offset == 0;

        /// <summary>
        ///     current reader value
        /// </summary>
        public char Value
            => input.CharAt(offset);

        /// <summary>
        ///     navigate to the previous char
        /// </summary>
        public void PreviousChar() {
            if (!AtBof) offset--;
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
    }
}