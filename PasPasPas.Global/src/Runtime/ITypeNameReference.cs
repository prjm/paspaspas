namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     reference to a type name
    /// </summary>
    public interface ITypeNameReference : ITypeReference {

        /// <summary>
        ///     base type id
        /// </summary>
        int BaseTypeId { get; }
    }
}
