using System;

namespace PasPasPas.Globals.Files {

    /// <summary>
    ///     interface for a stacked file reader
    /// </summary>
    public interface IStackedFileReader : IDisposable {

        /// <summary>
        ///     current file
        /// </summary>
        FileReference CurrentFile { get; }

        /// <summary>
        ///     test if the reader is at the end of file
        /// </summary>
        bool AtEof { get; }

        /// <summary>
        ///     current character
        /// </summary>
        char Value { get; }

        /// <summary>
        ///     current position
        /// </summary>
        long Position { get; }

        /// <summary>
        ///     get the previous char
        /// </summary>
        /// <returns></returns>
        char PreviousChar();

        /// <summary>
        ///     get the next char
        /// </summary>
        /// <returns></returns>
        char NextChar();

        /// <summary>
        ///     finish current file
        /// </summary>
        /// <returns></returns>
        FileReference FinishCurrentFile();

        /// <summary>
        ///     look ahead some chars
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        char LookAhead(int number);

        /// <summary>
        ///     add input to read
        /// </summary>
        /// <param name="inputToRead"></param>
        void AddInputToRead(FileReference inputToRead);

    }
}
