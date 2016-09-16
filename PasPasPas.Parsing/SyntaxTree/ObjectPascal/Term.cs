namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     term
    /// </summary>
    public class Term : SyntaxPartBase {

        /// <summary>
        ///     term kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public Factor LeftOperand { get; set; }

        /// <summary>
        ///     rihgt operand
        /// </summary>
        public Term RightOperand { get; set; }

    }
}