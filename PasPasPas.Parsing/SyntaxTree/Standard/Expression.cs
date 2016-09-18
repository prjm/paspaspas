namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     expression
    /// </summary>
    public class Expression : SyntaxPartBase {

        /// <summary>
        ///     closue expression
        /// </summary>
        public ClosureExpression ClosureExpression { get; set; }

        /// <summary>
        ///     relational operator kind
        /// </summary>
        public int Kind { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     simple expression
        /// </summary>
        public SimpleExpression LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SimpleExpression RightOperand { get; set; }

    }
}
