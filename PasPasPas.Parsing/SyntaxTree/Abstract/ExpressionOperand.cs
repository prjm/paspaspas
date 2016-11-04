namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     expression operand
    /// </summary>
    public class ExpressionOperand : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     operand valude
        /// </summary>
        public ExpressionBase Value { get; set; }

    }
}
