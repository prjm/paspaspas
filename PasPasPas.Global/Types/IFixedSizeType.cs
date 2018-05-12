namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface types with a fixed size
    /// </summary>
    public interface IFixedSizeType : ITypeDefinition {

        /// <summary>
        ///     type size in bits
        /// </summary>
        uint BitSize { get; }

    }
}
