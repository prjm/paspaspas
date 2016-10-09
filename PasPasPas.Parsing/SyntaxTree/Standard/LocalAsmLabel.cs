namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     local asm label
    /// </summary>
    public class LocalAsmLabel : SyntaxPartBase {

        /// <summary>
        ///     label
        /// </summary>
        public Label Label { get; internal set; }
    }
}

