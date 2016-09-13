namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     requires clause
    /// </summary>
    public class PackageRequires : SyntaxPartBase {
        /// <summary>
        ///     required packages
        /// </summary>
        public NamespaceNameList RequiresList { get; set; }
    }
}