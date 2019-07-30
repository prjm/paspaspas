using System;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.Parser {

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
