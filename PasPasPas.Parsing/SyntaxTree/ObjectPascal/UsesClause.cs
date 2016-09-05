namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     uses clause
    /// </summary>
    public class UsesClause : SyntaxPartBase {

        /// <summary>
        ///     names of the units to use
        /// </summary>
        public NamespaceNameList UsesList { get; set; }

    }
}