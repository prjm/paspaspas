namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for type aliases
    /// </summary>
    public interface IAliasedType : ITypeDefinition {

        /// <summary>
        ///     base type id
        /// </summary>
        int BaseTypeId { get; }

    }
}
