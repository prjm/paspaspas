using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     read from a combiniation of textfiles
    /// </summary>
    public class StackedFileReader {

        private readonly Stack<FileBufferItemOffset> files;
        private readonly FileBuffer buffer;

        /// <summary>
        ///     create a new stacked file reader
        /// </summary>
        /// <param name="fileBuffer"></param>
        public StackedFileReader(FileBuffer fileBuffer) {
            if (fileBuffer == null)
                ExceptionHelper.ArgumentIsNull(nameof(fileBuffer));

            buffer = fileBuffer;
            files = new Stack<FileBufferItemOffset>();
        }

        /// <summary>
        ///     adds a file to read
        /// </summary>
        /// <param name="input">input to add</param>
        public void AddFileToRead(IFileReference input) {
            if (input == null)
                ExceptionHelper.ArgumentIsNull(nameof(input));

            files.Push(new FileBufferItemOffset(this, buffer[input]));
        }

        /// <summary>
        ///     finish current file
        /// </summary>
        public FileBufferItemOffset FinishCurrentFile() {

            if (files.Count < 1)
                ExceptionHelper.InvalidOperation();

            files.Pop();

            if (files.Count == 0)
                return null;
            else
                return files.Peek();
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
            => files.Count > 0 ? files.Peek() : null;

        /// <summary>
        ///     get the current character value
        /// </summary>
        public char Value {
            get {
                if (files.Count > 0)
                    return files.Peek().Value;

                ExceptionHelper.InvalidOperation();
                return '\0';
            }
        }

        /// <summary>
        ///     check if the reader has reached eof
        /// </summary>
        public bool AtEof {
            get {
                if (files.Count > 0)
                    return files.Peek().AtEof;

                ExceptionHelper.InvalidOperation();
                return false;
            }
        }

        /// <summary>
        ///     current position
        /// </summary>
        public int Position
            => files.Count > 0 ? files.Peek().Position : -1;

        /// <summary>
        ///     fetch the ext char
        /// </summary>
        public void NextChar() {
            if (files.Count > 0)
                files.Peek().NextChar();
            else
                ExceptionHelper.InvalidOperation();
        }
    }
}
