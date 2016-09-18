namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     create a new for statement
    /// </summary>
    public class ForStatement : SyntaxPartBase {

        /// <summary>
        ///     iteration end
        /// </summary>
        public Expression EndExpression { get; set; }

        /// <summary>
        ///     iteration kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     iteration start
        /// </summary>
        public Expression StartExpression { get; set; }

        /// <summary>
        ///     iteration statement
        /// </summary>
        public Statement Statement { get; set; }

        /// <summary>
        ///     iteration variable
        /// </summary>
        public DesignatorStatement Variable { get; set; }

    }
}