namespace PasPasPas.Parsing.SyntaxTree.Standard {

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