using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Api {

    /// <summary>
    ///     interface for a parser
    /// </summary>
    public interface IParser {

        /// <summary>
        ///     tokenizer to use
        /// </summary>
        ITokenizer BaseTokenizer { get; set; }

        /// <summary>
        ///     parse input
        /// </summary>
        ISyntaxPart Parse();
    }
}
