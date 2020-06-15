namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for a pointer type
    /// </summary>
    public interface IPointerType : IFixedSizeType {

        /// <summary>
        ///     base type definition
        /// </summary>
        ITypeDefinition BaseTypeDefinition { get; }
    }
}
