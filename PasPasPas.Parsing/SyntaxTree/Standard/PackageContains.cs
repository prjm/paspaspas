namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     contains clause
    /// </summary>
    public class PackageContains : SyntaxPartBase {

        /// <summary>
        ///     contained units
        /// </summary>
        public NamespaceFileNameList ContainsList { get; set; }

    }
}