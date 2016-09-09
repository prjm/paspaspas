namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     library definition
    /// </summary>
    public class Library : SyntaxPartBase {

        /// <summary>
        ///     library head
        /// </summary>
        public LibraryHead LibraryHead { get; set; }

        /// <summary>
        ///     main block
        /// </summary>
        public Block MainBlock { get; set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesFileClause Uses { get; set; }


    }
}
