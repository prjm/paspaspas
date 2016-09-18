namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     compound statement
    /// </summary>
    public class CompoundStatement : SyntaxPartBase {

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; set; }

    }
}