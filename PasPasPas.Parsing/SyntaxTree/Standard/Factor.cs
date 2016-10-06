namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     factor
    /// </summary>
    public class Factor : SyntaxPartBase {

        /// <summary>
        ///     address of operator
        /// </summary>
        public Factor AddressOf { get; set; }

        /// <summary>
        ///     designator (inherited)
        /// </summary>
        public DesignatorStatement Designator { get; set; }

        /// <summary>
        ///     hex number
        /// </summary>
        public HexNumber HexValue { get; set; }

        /// <summary>
        ///     int value
        /// </summary>
        public StandardInteger IntValue { get; set; }

        /// <summary>
        ///     <c>false</c> literal
        /// </summary>
        public bool IsFalse { get; set; }

        /// <summary>
        ///     <c>nil</c> literal
        /// </summary>
        public bool IsNil { get; set; }

        /// <summary>
        ///     <c>true</c> literal
        /// </summary>
        public bool IsTrue { get; set; }

        /// <summary>
        ///     minus
        /// </summary>
        public Factor Minus { get; set; }

        /// <summary>
        ///     nor
        /// </summary>
        public Factor Not { get; set; }

        /// <summary>
        ///     parented expression
        /// </summary>
        public Expression ParenExpression { get; set; }

        /// <summary>
        ///     plus
        /// </summary>
        public Factor Plus { get; set; }

        /// <summary>
        ///     pointer to
        /// </summary>
        public Identifier PointerTo { get; set; }

        /// <summary>
        ///     real value
        /// </summary>
        public RealNumber RealValue { get; set; }

        /// <summary>
        ///     record helper calls
        /// </summary>
        public DesignatorStatement RecordHelper { get; set; }

        /// <summary>
        ///     set section
        /// </summary>
        public SetSection SetSection { get; set; }

        /// <summary>
        ///     string factor
        /// </summary>
        public QuotedString StringValue { get; set; }


    }
}