namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     except handlers
    /// </summary>
    public class ExceptHandlers : SyntaxPartBase {

        /// <summary>
        ///     else statements
        /// </summary>
        public StatementList ElseStatements { get; set; }

        /// <summary>
        ///     generic except handler statements
        /// </summary>
        public StatementList Statements { get; set; }

    }
}