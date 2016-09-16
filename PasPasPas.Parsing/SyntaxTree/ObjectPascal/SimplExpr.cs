namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     simple expression
    /// </summary>
    public class SimpleExpression : SyntaxPartBase {

        /// <summary>
        ///     expression kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public Term LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SimpleExpression RightOperand { get; set; }

    }
}