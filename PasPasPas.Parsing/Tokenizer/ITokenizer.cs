using PasPasPas.Infrastructure.Log;
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
        bool AtEof { get; }

        /// <summary>
        ///     current token
        /// </summary>
        Token CurrentToken { get; }

        /// <summary>
        ///     current char
        /// </summary>
        char CurrentCharacter { get; }

        /// <summary>
        ///     current position
        /// </summary>
        int CurrentPosition { get; }

        /// <summary>
        ///     move to the next char
        /// </summary>
        void NextChar();

        /// <summary>
        ///     move to the previous char
        /// </summary>
        void PreviousChar();

        /// <summary>
        ///     prepare to fetch the next token
        /// </summary>
        void PrepareNextToken();

        /// <summary>
        ///     log
        /// </summary>
        ILogSource Log { get; }
    }
}
