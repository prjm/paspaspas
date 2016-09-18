namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///  while statement
    /// </summary>
    public class WhileStatement : SyntaxPartBase {

        /// <summary>
        ///     condition
        /// </summary>
        public Expression Condition { get; set; }

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; set; }

    }
}