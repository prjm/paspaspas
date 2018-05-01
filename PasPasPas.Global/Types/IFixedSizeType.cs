namespace PasPasPas.Global.Types {

    /// <summary>
    ///     types with a fixed size
    /// </summary>
    public interface IFixedSizeType : ITypeDefinition {

        /// <summary>
        ///     type size in bits
        /// </summary>
        uint BitSize { get; }


    }
}
