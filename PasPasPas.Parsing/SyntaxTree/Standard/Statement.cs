namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     single statement
    /// </summary>
    public class Statement : SyntaxPartBase {

        /// <summary>
        ///     label
        /// </summary>
        public Label Label { get; set; }

        /// <summary>
        ///     statement part
        /// </summary>
        public StatementPart Part { get; set; }

    }
}