namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     string type definition
    /// </summary>
    public interface IStringType : ITypeDefinition {

        /// <summary>
        ///     string type kind
        /// </summary>
        StringTypeKind Kind { get; }

    }
}
