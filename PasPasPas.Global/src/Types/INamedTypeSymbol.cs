namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     type symbol with name
    /// </summary>
    public interface INamedTypeSymbol : ITypeSymbol {


        /// <summary>
        ///     symbol name
        /// </summary>
        string Name { get; }

    }
}
