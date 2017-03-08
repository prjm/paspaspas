namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     label in <c>asm</c>
    /// </summary>
    public class AsmLabel : SyntaxPartBase {

        /// <summary>
        ///     asm label
        /// </summary>
        public LocalAsmLabel LocalLabel { get; set; }

        /// <summary>
        ///     generic label
        /// </summary>
        public Label Label { get; set; }
    }
}
