namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     goto statement
    /// </summary>
    public class GoToStatement : SyntaxPartBase {

        /// <summary>
        ///     break statement
        /// </summary>
        public bool Break { get; set; }

        /// <summary>
        ///     continue statement
        /// </summary>
        public bool Continue { get; set; }

        /// <summary>
        ///     exit statement
        /// </summary>
        public bool Exit { get; set; }

        /// <summary>
        ///     exit expression
        /// </summary>
        public Expression ExitExpression { get; set; }

        /// <summary>
        ///     goto label
        /// </summary>
        public Label GoToLabel { get; set; }

    }
}