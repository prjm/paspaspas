namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     symbol table entry
    /// </summary>
    public interface ISymbolTableEntry {

        /// <summary>
        ///     get the name of the symbol
        /// </summary>
        string SymbolName { get; }

    }
}