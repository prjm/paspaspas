namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     helper classes for type symbols
    /// </summary>
    public static class ITypeSymbolHelpers {

        /// <summary>
        ///     checks if this symbol is a constant value
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static bool IsConstant(this ITypeSymbol symbol)
            => symbol.SymbolKind == SymbolTypeKind.Constant;

    }
}
