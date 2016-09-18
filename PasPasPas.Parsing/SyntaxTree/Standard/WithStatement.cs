namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     with statement
    /// </summary>
    public class WithStatement : SyntaxPartBase {

        /// <summary>
        ///     statement
        /// </summary>
        public Statement Statement { get; set; }

    }
}