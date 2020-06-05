#nullable disable
using System;
using System.Collections.Generic;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;

namespace PasPasPas.Globals.Parsing {

    /// <summary>
    ///     interface for a tokenizer
    /// </summary>
    public interface ITokenizer : IDisposable {

        /// <summary>
        ///     fetch then next token if possible
        /// </summary>
        void FetchNextToken();

        /// <summary>
        ///     check if a next token exists
        /// </summary>
        bool AtEof { get; }

        /// <summary>
        ///     current token
        /// </summary>
        Token CurrentToken { get; }

        /// <summary>
        ///     defined keywords
        /// </summary>
        IDictionary<string, int> Keywords { get; }

        /// <summary>
        ///     current input
        /// </summary>
        IStackedFileReader Input { get; }

        /// <summary>
        ///     log manager
        /// </summary>
        ILogManager Log { get; }

        /// <summary>
        ///     position
        /// </summary>
        int Position { get; }
    }
}
