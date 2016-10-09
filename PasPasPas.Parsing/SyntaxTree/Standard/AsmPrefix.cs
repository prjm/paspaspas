namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     asm prefix
    /// </summary>
    public class AsmPrefix : SyntaxPartBase {

        /// <summary>
        ///     lock prefix
        /// </summary>
        public Identifier LockPrefix { get; set; }

        /// <summary>
        ///     segment prefix
        /// </summary>
        public Identifier SegmentPrefix { get; set; }
    }
}
