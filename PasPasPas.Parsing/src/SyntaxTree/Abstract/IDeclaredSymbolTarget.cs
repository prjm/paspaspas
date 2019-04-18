namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     symbols
    /// </summary>
    public interface IDeclaredSymbolTarget {

        /// <summary>
        ///     get symbols
        /// </summary>
        DeclaredSymbolCollection Symbols { get; }

    }
}
