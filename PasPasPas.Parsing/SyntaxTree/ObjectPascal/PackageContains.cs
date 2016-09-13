namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

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