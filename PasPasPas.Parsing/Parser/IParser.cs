using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPas.Parsing.Parser {

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
