using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     symbol table
    /// </summary>
    /// <typeparam name="T">symbol type, identifiert by names</typeparam>
    public interface ISymbolTable<T> : ISyntaxPart where T : ISymbolTableEntry {

        /// <summary>
        ///     try to add the symbol
        /// </summary>
        /// <param name="entry">entry to add</param>
        /// <param name="logSource">log source</param>
        /// <returns><c>true</c> if added</returns>
        bool Add(T entry, LogSource logSource);

        /// <summary>
        ///     try to remove the symbol
        /// </summary>
        /// <param name="entry">entry to remove</param>
        /// <returns><c>true</c> if removed</returns>
        bool Remove(T entry);

    }
}
