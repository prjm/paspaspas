namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     interface for syntax tree nodes with constant values
    /// </summary>
    public interface IConstantValueNode {

        /// <summary>
        ///     <c>true</c> if the node has a constant value
        /// </summary>
        bool IsConstant { get; }
    }
}
