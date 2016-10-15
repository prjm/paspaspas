namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly term   
    /// </summary>
    public class AsmTerm : SyntaxPartBase {

        /// <summary>
        ///     left operand
        /// </summary>
        public AsmFactor LeftOperand { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public AsmOperand RightOperand { get; set; }

        /// <summary>
        ///     subtype
        /// </summary>
        public AsmOperand Subtype { get; internal set; }
    }
}
