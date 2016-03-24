﻿using System;

namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     interface for parser input
    /// </summary>
    public interface IParserInput : IDisposable {

        /// <summary>
        ///     tests if any input is left
        /// </summary>
        /// <returns><c>true</c> if some input is left</returns>
        bool AtEof { get; }

        /// <summary>
        ///     current file position offset
        /// </summary>
        long Position { get; set; }

        /// <summary>
        ///     get the next char from the input
        /// </summary>
        /// <returns>next char</returns>
        char NextChar();

        /// <summary>
        ///     close the file
        /// </summary>
        void Close();

        /// <summary>
        ///     open this file
        /// </summary>
        void Open();
    }
}