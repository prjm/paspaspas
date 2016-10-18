namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for symbol table entries
    /// </summary>
    public abstract class SymbolTableEntryBase : ISymbolTableEntry {

        private string symbolName;

        /// <summary>
        ///     symbol name
        /// </summary>
        public string SymbolName
            => symbolName;

    }
}