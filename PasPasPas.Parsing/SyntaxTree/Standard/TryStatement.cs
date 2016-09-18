namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     try statement
    /// </summary>
    public class TryStatement : SyntaxPartBase {

        /// <summary>
        ///     finally part
        /// </summary>
        public StatementList Finally { get; set; }

        /// <summary>
        ///     except handlers
        /// </summary>
        public ExceptHandlers Handlers { get; set; }

        /// <summary>
        ///     try part
        /// </summary>
        public StatementList Try { get; set; }

    }
}