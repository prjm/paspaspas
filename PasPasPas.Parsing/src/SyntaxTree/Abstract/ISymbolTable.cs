using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     symbol table
    /// </summary>
    /// <typeparam name="T">symbol type, identified by names</typeparam>
    public interface ISymbolTable<T> : ISyntaxPart where T : ISymbolTableEntry {

        /// <summary>
        ///     try to add the symbol
        /// </summary>
        /// <param name="entry">entry to add</param>
        /// <param name="logSource">log source</param>
        /// <param name="numberOfTypeParameters">number of generic type paremeters</param>
        /// <returns><c>true</c> if added</returns>
        bool Add(T entry, ILogSource logSource, int numberOfTypeParameters = 0);

        /// <summary>
        ///     try to remove the symbol
        /// </summary>
        /// <param name="entry">entry to remove</param>
        /// <returns><c>true</c> if removed</returns>
        bool Remove(T entry);

    }
}
