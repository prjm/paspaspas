namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     uses clause with file names
    /// </summary>
    public class UsesFileClause : SyntaxPartBase {

        /// <summary>
        ///     Namespace files
        /// </summary>
        public NamespaceFileNameList Files { get; set; }

    }
}
