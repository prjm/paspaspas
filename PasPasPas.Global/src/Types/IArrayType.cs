namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     array type
    /// </summary>
    public interface IArrayType : ITypeDefinition {

        /// <summary>
        ///     array kind
        /// </summary>
        ArrayTypeKind Kind { get; }

        /// <summary>
        ///     index types
        /// </summary>
        ITypeDefinition IndexType { get; }

        /// <summary>
        ///     base type id
        /// </summary>
        ITypeDefinition BaseTypeDefinition { get; }

        /// <summary>
        ///     packed array
        /// </summary>
        bool Packed { get; }
    }
}
