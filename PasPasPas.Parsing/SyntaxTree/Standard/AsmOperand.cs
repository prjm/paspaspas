namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly operand
    /// </summary>
    public class AsmOperand : SyntaxPartBase {

        /// <summary>
        ///     left term
        /// </summary>
        public AsmExpression LeftTerm { get; internal set; }

        /// <summary>
        ///     not expression
        /// </summary>
        public AsmOperand NotExpression { get; set; }

        /// <summary>
        ///     right term
        /// </summary>
        public AsmOperand RightTerm { get; set; }

        /// <summary>
        ///     operand kind
        /// </summary>
        public int Kind { get; set; }

    }
}
