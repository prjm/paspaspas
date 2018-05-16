using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     interface for classes which tokenize source code
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
        StackedFileReader Input { get; }

        /// <summary>
        ///     log manager
        /// </summary>
        ILogManager Log { get; }
    }
}
