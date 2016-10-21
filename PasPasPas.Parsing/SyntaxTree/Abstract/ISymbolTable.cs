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
        /// <returns><c>true</c> if added</returns>
        bool Add(T entry);

        /// <summary>
        ///     try to remove the symbol
        /// </summary>
        /// <param name="entry">entry to remove</param>
        /// <returns><c>true</c> if removed</returns>
        bool Remove(T entry);

    }
}
