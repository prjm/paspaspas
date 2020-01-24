namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for type aliases
    /// </summary>
    public interface IAliasedType : ITypeDefinition {

        /// <summary>
        ///     base type (aliased type)
        /// </summary>
        ITypeDefinition BaseTypeDefinition { get; }

    }
}
