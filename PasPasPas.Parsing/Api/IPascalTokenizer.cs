using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Api {

    /// <summary>
    ///     interface for classes which tokenize source code
    /// </summary>
    public interface IPascalTokenizer {

        /// <summary>
        ///     test if a next token is availiable
        /// </summary>
        /// <returns><c>true</c> if a next token is available</returns>
        bool HasNextToken();

        /// <summary>
        ///     get the next token
        /// </summary>
        /// <returns>next pascal token</returns>
        PascalToken FetchNextToken();

        /// <summary>
        ///     get the current file
        /// </summary>
        IFile CurrentFile { get; }

    }
}
