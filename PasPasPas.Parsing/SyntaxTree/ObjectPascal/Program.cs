namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     syntax tree for a program
    /// </summary>
    public class Program : SyntaxPartBase {

        /// <summary>
        ///     main block
        /// </summary>
        public Block MainBlock { get; set; }

        /// <summary>
        ///     program header
        /// </summary>
        public ProgramHead ProgramHead { get; set; }

        /// <summary>
        ///     uses list
        /// </summary>
        public UsesFileClause Uses { get; set; }
    }
}
