namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     if statement
    /// </summary>
    public class IfStatement : SyntaxPartBase {

        /// <summary>
        ///     condition
        /// </summary>
        public Expression Condition { get; set; }

        /// <summary>
        ///     else part
        /// </summary>
        public Statement ElsePart { get; set; }

        /// <summary>
        ///     then part
        /// </summary>
        public Statement ThenPart { get; set; }

    }
}