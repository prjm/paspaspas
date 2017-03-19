namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly factor
    /// </summary>
    public class AsmFactor : SyntaxPartBase {

        /// <summary>
        ///     hex number
        /// </summary>     
        public HexNumber HexNumber { get; internal set; }

        /// <summary>
        ///     identifier
        /// </summary>
        public Identifier Identifier { get; internal set; }

        /// <summary>
        ///     memory subexpression
        /// </summary>
        public AsmOperand MemorySubexpression { get; internal set; }

        /// <summary>
        ///     number
        /// </summary>
        public StandardInteger Number { get; internal set; }

        /// <summary>
        ///     quoted string
        /// </summary>
        public QuotedString QuotedString { get; internal set; }

        /// <summary>
        ///     segment subexpression
        /// </summary>
        public AsmOperand SegmentExpression { get; set; }

        /// <summary>
        ///     segment prefix
        /// </summary>
        public Identifier SegmentPrefix { get; set; }

        /// <summary>
        ///     subexpression
        /// </summary>
        public AsmOperand Subexpression { get; internal set; }

        /// <summary>
        ///     real number
        /// </summary>
        public RealNumber RealNumber { get; internal set; }

        /// <summary>
        ///     referenced label
        /// </summary>
        public LocalAsmLabel Label { get; internal set; }
    }
}
