namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     array type
    /// </summary>
    public interface IArrayType : ITypeDefinition {

        /// <summary>
        ///     index types
        /// </summary>
        int IndexTypeId { get; }

        /// <summary>
        ///     base type id
        /// </summary>
        int BaseTypeId { get; }

    }
}
