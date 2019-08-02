using System;

namespace PasPasPas.Globals.Parsing.Parser {

    /// <summary>
    ///     interface for a parser
    /// </summary>
    public interface IParser : IDisposable {

        /// <summary>
        ///     tokenizer to use
        /// </summary>
        ITokenizer BaseTokenizer { get; }

        /// <summary>
        ///     parse input
        /// </summary>
        ISyntaxPart Parse();

    }
}
