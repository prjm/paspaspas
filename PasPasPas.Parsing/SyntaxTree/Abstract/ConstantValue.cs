namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a constant value
    /// </summary>
    public class ConstantValue : ExpressionBase {

        /// <summary>
        ///     constant value kind
        /// </summary>
        public ConstantValueKind Kind { get; set; }

        /// <summary>
        ///     integer value
        /// </summary>
        public int IntValue { get; internal set; }
    }
}
