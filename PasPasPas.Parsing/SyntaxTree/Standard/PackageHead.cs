namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     package head
    /// </summary>
    public class PackageHead : SyntaxPartBase {
        /// <summary>
        ///     package name
        /// </summary>
        public NamespaceName PackageName { get; set; }

    }
}