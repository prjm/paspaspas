namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     set type definition
    /// </summary>
    public interface ISetType : ITypeDefinition {

        /// <summary>
        ///     set base type id
        /// </summary>
        IOrdinalType BaseTypeDefinition { get; }
    }
}
