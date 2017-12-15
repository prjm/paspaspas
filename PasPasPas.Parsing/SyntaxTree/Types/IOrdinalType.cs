namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     interface for ordinal types
    /// </summary>
    public interface IOrdinalType : IFixedSizeType {

        /// <summary>
        ///     highest element
        /// </summary>
        ulong HighestElement { get; }
    }
}