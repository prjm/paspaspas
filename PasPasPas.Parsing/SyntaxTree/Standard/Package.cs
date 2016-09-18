namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     package definition
    /// </summary>
    public class Package : SyntaxPartBase {

        /// <summary>
        ///     contans clause
        /// </summary>
        public PackageContains ContainsClause { get; set; }

        /// <summary>
        ///     package head
        /// </summary>
        public PackageHead PackageHead { get; set; }

        /// <summary>
        ///     requires clause
        /// </summary>
        public PackageRequires RequiresClause { get; set; }

    }
}
