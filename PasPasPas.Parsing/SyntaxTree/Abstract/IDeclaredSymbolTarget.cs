namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     sybols
    /// </summary>
    public interface IDeclaredSymbolTarget {

        /// <summary>
        ///     get symbols
        /// </summary>
        SymbolTableBase<DeclaredSymbol> Symbols { get; }

    }
}
