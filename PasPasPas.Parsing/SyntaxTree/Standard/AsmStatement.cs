namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm statement
    /// </summary>
    public class AsmStatement : SyntaxPartBase {

        /// <summary>
        ///     opcode
        /// </summary>
        public AsmOpCode OpCode { get; set; }

        /// <summary>
        ///     lock / segment prefix
        /// </summary>
        public AsmPrefix Prefix { get; set; }

        /// <summary>
        ///     label
        /// </summary>
        public AsmLabel Label { get; internal set; }

    }
}
