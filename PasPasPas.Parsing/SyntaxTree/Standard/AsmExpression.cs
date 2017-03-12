namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly expression term
    /// </summary>
    public class AsmExpression : SyntaxPartBase {

        /// <summary>
        ///     byte ppointer
        /// </summary>
        public AsmOperand BytePtr { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public AsmTerm LeftOperand { get; set; }

        /// <summary>
        ///     offset expression
        /// </summary>
        public AsmOperand Offset { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public AsmOperand RightOperand { get; internal set; }

        /// <summary>
        ///     type expression
        /// </summary>
        public AsmOperand TypeExpression { get; set; }

        /// <summary>
        ///     byte pointer kind
        /// </summary>
        public Identifier BytePtrKind { get; internal set; }
    }
}
