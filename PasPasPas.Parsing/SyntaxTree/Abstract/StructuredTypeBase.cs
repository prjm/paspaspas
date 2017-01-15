namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     structured type definition
    /// </summary>
    public class StructuredTypeBase : TypeSpecificationBase, IDeclaredSymbolTarget {

        /// <summary>
        ///     packed type
        /// </summary>
        public bool PackedType { get; set; }

        /// <summary>
        ///     declared symbols
        /// </summary>
        public SymbolTableBase<DeclaredSymbol> Symbols { get; }
            = new DeclaredSymbols();
    }
}
