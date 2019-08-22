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
        ///     flag, <c>true</c> to enable unit resolving
        /// </summary>
        bool ResolveIncludedFiles { get; set; }

        /// <summary>
        ///     parse input
        /// </summary>
        ISyntaxPart Parse();

    }
}
