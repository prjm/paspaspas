namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     repeat statement
    /// </summary>
    public class RepeatStatement : SyntaxPartBase {

        /// <summary>
        ///     condition
        /// </summary>
        public Expression Condition { get; set; }

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; set; }

    }
}