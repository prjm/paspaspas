namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly op code
    /// </summary>
    public class AsmOpCode : SyntaxPartBase {

        /// <summary>
        ///     directive
        /// </summary>
        public Identifier Directive { get; set; }

        /// <summary>
        ///     op code
        /// </summary>
        public Identifier OpCode { get; set; }
    }
}
