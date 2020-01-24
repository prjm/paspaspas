namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     a typed symbol
    /// </summary>
    public interface ITypeSymbol {

        /// <summary>
        ///     get the matching type definition
        /// </summary>
        ITypeDefinition TypeDefinition { get; }

        /// <summary>
        ///     symbol type kind
        /// </summary>
        SymbolTypeKind SymbolKind { get; }

    }
}
