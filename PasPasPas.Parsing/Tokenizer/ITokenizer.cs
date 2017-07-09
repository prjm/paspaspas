using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     interface for classes which tokenize source code
    /// </summary>
    public interface ITokenizer {

        /// <summary>
        ///     fetch then next token if possible
        /// </summary>
        void FetchNextToken();

        /// <summary>
        ///     check if a next token exists
        /// </summary>
        bool HasNextToken { get; }

        /// <summary>
        ///     get the current token
        /// </summary>
        /// <returns>current token</returns>
        ref Token CurrentToken { get; }

    }
}
