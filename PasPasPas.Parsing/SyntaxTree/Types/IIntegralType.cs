namespace PasPasPas.Parsing.SyntaxTree.Types {
    /// <summary>
    ///     interface for integral types
    /// </summary>
    public interface IIntegralType : IOrdinalType {

        /// <summary>
        ///     true if this type is signed
        /// </summary>
        bool Signed { get; }
    }
}
