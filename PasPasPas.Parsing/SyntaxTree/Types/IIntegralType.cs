namespace PasPasPas.Parsing.SyntaxTree.Types {
    /// <summary>
    ///     interface for integral types
    /// </summary>
    public interface IIntegralType : IFixedSizeType {

        /// <summary>
        ///     true if this type is signed
        /// </summary>
        bool Signed { get; }
    }
}
