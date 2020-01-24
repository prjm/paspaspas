namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     array type
    /// </summary>
    public interface IArrayType : ITypeDefinition {

        /// <summary>
        ///     index types
        /// </summary>
        IOrdinalType IndexType { get; }

        /// <summary>
        ///     base type id
        /// </summary>
        ITypeDefinition BaseTypeDefinition { get; }

    }
}
