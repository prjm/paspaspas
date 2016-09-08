namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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