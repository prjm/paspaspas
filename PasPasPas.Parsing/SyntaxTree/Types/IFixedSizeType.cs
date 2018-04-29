namespace PasPasPas.Parsing.SyntaxTree.Types {

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
