namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     type symbol with mangled name
    /// </summary>
    public interface IMangledNameTypeSymbol : INamedTypeSymbol {

        /// <summary>
        ///     mangled name
        /// </summary>
        string MangledName { get; }

    }
}
