namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     compound statement
    /// </summary>
    public class CompoundStatement : SyntaxPartBase {

        /// <summary>
        ///     assembler block
        /// </summary>
        public Token AssemblerBlock { get; internal set; }

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; set; }

    }
}